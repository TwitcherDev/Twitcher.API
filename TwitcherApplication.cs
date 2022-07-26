using Microsoft.Extensions.Logging;
using Twitcher.API.Events;

namespace Twitcher.API;

public class TwitcherApplication
{
    private readonly RestClient _idClient;
    private readonly RestClient _apiClient;

    private readonly List<string> _authStates;
    private readonly List<(string tag, string userId, TwitcherAPI api)> _apis;

    private readonly CancellationToken _cancellationToken;

    public string ClientId { get; }
    public string ClientSecret { get; }
    public ILogger? Logger { get; set; }

    public event EventHandler<TokenCreatedArgs>? TokenCreated;
    public event EventHandler<TokenRefreshedArgs>? TokenRefreshed;
    public event EventHandler<TokenDeadArgs>? TokenDead;

    public TwitcherApplication(string clientId, string clientSecret, CancellationToken token = default)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        _idClient = new RestClient("https://id.twitch.tv").UseSerializer<JsonSnakeSerializer>();
        _apiClient = new RestClient("https://api.twitch.tv").UseSerializer<JsonSnakeSerializer>();
        _authStates = new List<string>();
        _apis = new List<(string prefix, string userId, TwitcherAPI api)>();
        _cancellationToken = token;
    }

    public string GenerateAuthorizeLink(string redirectUri, IEnumerable<string> scopes) => GenerateAuthorizeLink(redirectUri, string.Join(' ', scopes));
    public string GenerateAuthorizeLink(string redirectUri, string scopes)
    {
        return $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientId}&redirect_uri={redirectUri}&scope={scopes}";
    }

    public string GenerateUniqueState()
    {
        var state = GenerateState(32);
        _authStates.Add(state);
        Logger?.LogTrace("Generate state: {state}", state);
        return state;
    }

    public string GenerateUniqueAuthorizeLink(string redirectUri, IEnumerable<string> scopes) => GenerateUniqueAuthorizeLink(redirectUri, string.Join(' ', scopes));
    public string GenerateUniqueAuthorizeLink(string redirectUri, string scopes)
    {
        var state = GenerateUniqueState();
        return $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientId}&redirect_uri={redirectUri}&scope={scopes}&state={state}";
    }

    private static string GenerateState(int length) => new(Enumerable.Range(0, length).Select(e => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[Random.Shared.Next(62)]).ToArray());

    /// <summary></summary>
    /// <param name="tag"></param>
    /// <param name="code"></param>
    /// <param name="redirectUri"></param>
    /// <param name="state"></param>
    /// <returns>userId if successful; otherwise, null</returns>
    /// <exception cref="WrongStateException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public Task<string?> CreateAPI(string tag, string code, string redirectUri, string state)
    {
        if (!_authStates.Remove(state))
            throw new WrongStateException();
        Logger?.LogTrace("State validated: {state}", state);

        return CreateAPI(tag, code, redirectUri);
    }

    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    /// <returns>userId if successful; otherwise, null</returns>
    public async Task<string?> CreateAPI(string tag, string code, string redirectUri)
    {
        var request = new RestRequest("oauth2/token", Method.Post)
            .AddQueryParameter("grant_type", "authorization_code")
            .AddQueryParameter("client_id", ClientId)
            .AddQueryParameter("client_secret", ClientSecret)
            .AddQueryParameter("code", code)
            .AddQueryParameter("redirect_uri", redirectUri);

        var response = await _idClient.ExecuteAsync<AuthorizationCodeResponseBody>(request);
        Logger?.LogTrace("Token response {status}: {content}", response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new BadRequestException();

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            throw new InternalServerException();

        if (response.Data == default ||
            string.IsNullOrEmpty(response.Data.AccessToken) ||
            string.IsNullOrEmpty(response.Data.RefreshToken))
            return null;

        var tokens = response.Data.AccessToken + ':' + response.Data.RefreshToken;

        var userId = await RegisterAPI(tag, tokens);
        if (string.IsNullOrEmpty(userId))
            return null;

        TokenCreated?.Invoke(this, new TokenCreatedArgs(tag, userId, tokens));
        Logger?.LogDebug("User '{userId}' ('{tag}') token created", userId, tag);
        return userId;
    }

    /// <summary></summary>
    /// <param name="tag"></param>
    /// <param name="tokens"></param>
    /// <returns></returns>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<string?> RegisterAPI(string tag, string tokens, string? userId = default)
    {
        var api = new TwitcherAPI(tokens, ClientId, ClientSecret, _idClient, _apiClient, userId) { Logger = Logger };
        api.TokenRefreshed += (s, e) => Api_TokenRefreshed(tag, (TwitcherAPI?)s, e.Tokens);
        api.TokenDead += (s, e) => Api_TokenDead(tag, (TwitcherAPI?)s);

        var response = await api.Validate();
        if (string.IsNullOrEmpty(response?.UserId))
            return null;

        _apis.Add((tag, response.UserId, api));

        return response?.UserId;
    }

    private void Api_TokenRefreshed(string tag, TwitcherAPI? api, string tokens)
    {
        if (api == default || string.IsNullOrEmpty(api.UserId))
            return;
        TokenRefreshed?.Invoke(this, new TokenRefreshedArgs(tag, api.UserId, tokens));
        Logger?.LogDebug("User '{userId}' ('{tag}') token refreshed", api.UserId, tag);
    }

    private void Api_TokenDead(string tag, TwitcherAPI? api)
    {
        if (api == default)
            return;
        TokenDead?.Invoke(this, new TokenDeadArgs(tag, api.UserId));
        Logger?.LogDebug("User '{userId}' ('{tag}') token dead", api.UserId, tag);
        if (string.IsNullOrEmpty(api.UserId))
            return;
        _apis.Remove((tag, api.UserId, api));
    }

    public IEnumerable<(string userId, TwitcherAPI api)> GetAPIsByTag(string tag) => _apis.Where(a => a.tag == tag).Select(a => (a.userId, a.api));

    public TwitcherAPI? GetAPI(string tag, string userId) => _apis.FirstOrDefault(a => a.tag == tag && a.userId == userId).api;

    public int RemoveAPIsByTag(string tag)
    {
        var count = 0;
        foreach (var (_, userId, api) in _apis.Where(a => a.tag == tag))
        {
            _apis.Remove((tag, userId, api));
            count += 1;
        }
        return count;
    }

    public bool RemoveAPI(string tag, string userId)
    {
        var api = _apis.FirstOrDefault(a => a.tag == tag && a.userId == userId).api;
        if (api == default)
            return false;

        _apis.Remove((tag, userId, api));
        return true;
    }

    /// <summary>Run validation of all tokens every hour. Can be overridden by a cancellationToken from the constructor</summary>
    public void RunValidating()
    {
        Task.Run(() => Validating(), _cancellationToken);
    }

    private async Task Validating()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(60 + 5 * (Random.Shared.NextDouble() - 0.5)), _cancellationToken);
            foreach (var api in _apis.Select(a => a.api).Where(a => a != default))
            {
                try
                {
                    _ = await api.Validate();
                }
                catch (DeadTokenException) { }
                catch (InternalServerException) { }
            }
        }
    }
}
