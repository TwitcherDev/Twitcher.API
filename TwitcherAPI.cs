using Microsoft.Extensions.Logging;
using Twitcher.API.Events;

namespace Twitcher.API;

/// <summary>Class for requests to helix twitch API</summary>
public class TwitcherAPI
{
    private static readonly RestClient _idClient = new RestClient("https://id.twitch.tv").UseSerializer<TwitcherJsonSerializer>();
    private static readonly RestClient _apiClient = new RestClient("https://api.twitch.tv").UseSerializer<TwitcherJsonSerializer>();

    private readonly string _clientId;
    private readonly string _clientSecret;
    private bool _isValidated = false;

    /// <summary>Twitch id of the token owner</summary>
    public string? UserId { get; private set; }
    /// <summary>Twitch login of the token owner</summary>
    public string? Login { get; private set; }
    /// <summary>Access token</summary>
    public string AccessToken { get; private set; }
    /// <summary>Refresh token</summary>
    public string? RefreshToken { get; private set; }
    /// <summary>Access token expiration time</summary>
    public DateTime ExpiresIn { get; private set; }
    /// <summary>Token scopes</summary>
    public string[]? Scopes { get; private set; }
    /// <summary>Logger for raw trace requests / responses data</summary>
    public ILogger? Logger { get; set; }
    /// <summary>Allows you to find out if the token has been changed since the last <see cref="SaveTokens"/></summary>
    public bool IsRefreshed { get; private set; } = false;

    /// <summary>Returns tokens, sets <see cref="IsRefreshed"/> to <see langword="false"/></summary>
    /// <returns>Tokens in 'access:refresh' format or only access token if no refresh</returns>
    public string SaveTokens()
    {
        IsRefreshed = false;
        if (RefreshToken != null)
            return AccessToken + ':' + RefreshToken;
        return AccessToken;
    }

    /// <summary>Invoked when the token has been changed, an alternative way to save tokens</summary>
    public event EventHandler<APITokenRefreshedArgs>? TokenRefreshed;

    /// <summary>Create an instance of TwitcherAPI using tokens</summary>
    /// <param name="tokens">Access and refresh tokens in 'access:refresh' format or only access token if no refresh</param>
    /// <param name="clientId">Id of the application that created the token</param>
    /// <param name="clientSecret">Secret of the application that created the token</param>
    /// <param name="userId">Twitch id of the token owner if known</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TwitcherAPI(string tokens, string clientId, string clientSecret, string? userId = default)
    {
        if (string.IsNullOrEmpty(tokens))
            throw new ArgumentNullException(nameof(tokens));
        
        var s = tokens.Split(':');
        if (s.Length == 1)
            AccessToken = s[0];
        else if (s.Length == 2)
            (AccessToken, RefreshToken) = (s[0], s[1]);
        else
            throw new ArgumentException("Tokens format: 'access:refresh' or 'access'", nameof(tokens));

        UserId = userId;
        ExpiresIn = DateTime.UtcNow;

        _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
    }

    /// <summary>Using authorization code grant flow for generate new token</summary>
    /// <param name="code">Code</param>
    /// <param name="redirectUri">Redirect uri</param>
    /// <param name="clientId">Id of the application that created the token</param>
    /// <param name="clientSecret">Secret of the application that created the token</param>
    /// <param name="logger">Logger for raw trace requests / responses data</param>
    /// <returns>Instance of TwitchAPI created from the received token</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<TwitcherAPI> AuthorizeCode(string code, string redirectUri, string clientId, string clientSecret, ILogger? logger = null)
    {
        var request = new RestRequest("oauth2/token", Method.Post)
            .AddQueryParameter("grant_type", "authorization_code")
            .AddQueryParameter("client_id", clientId)
            .AddQueryParameter("client_secret", clientSecret)
            .AddQueryParameter("code", code)
            .AddQueryParameter("redirect_uri", redirectUri);

        var response = await _idClient.ExecuteAsync(request);
        logger?.LogTrace("Authorization code response {status}: {content}", response.StatusCode, response.Content);

        if (!response.IsSuccessful)
        {
            var error = _idClient.Deserialize<ErrorResponse>(response).Data;
            throw GenerateTwitchErrorException(response.StatusCode, error);
        }

        var data = _idClient.Deserialize<AuthorizationCodeResponseBody>(response).Data;
        if (data == null)
            throw new NullReferenceException("Twitch return success token response without body");

        return new TwitcherAPI(data.AccessToken + ':' + data.RefreshToken, clientId, clientSecret) { Logger = logger };
    }

    /// <summary>Refresh access token</summary>
    /// <returns>Refresh response</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public async Task<RefreshResponseBody> Refresh()
    {
        if (RefreshToken == null)
            throw new NotSupportedException("Refresh not supported without refresh token");

        var request = new RestRequest("oauth2/token", Method.Post)
            .AddQueryParameter("grant_type", "refresh_token")
            .AddQueryParameter("client_id", _clientId)
            .AddQueryParameter("client_secret", _clientSecret)
            .AddQueryParameter("refresh_token", RefreshToken);

        var response = await _idClient.ExecuteAsync(request);
        Logger?.LogTrace("User '{id}' refresh response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (!response.IsSuccessful)
        {
            var error = _idClient.Deserialize<ErrorResponse>(response).Data;
            throw GenerateTwitchErrorException(response.StatusCode, error);
        }

        var data = _idClient.Deserialize<RefreshResponseBody>(response).Data;
        if (data == null)
            throw new NullReferenceException("Twitch returns success refresh response without body");

        AccessToken = data.AccessToken;
        RefreshToken = data.RefreshToken;
        Scopes = data.Scopes;
        ExpiresIn = DateTime.UtcNow.AddSeconds(data.ExpiresIn);
        IsRefreshed = true;
        _isValidated = true;
        TokenRefreshed?.Invoke(this, new APITokenRefreshedArgs(AccessToken + ':' + RefreshToken));
        return data;
    }

    /// <summary>Validate access token</summary>
    /// <returns>Validate response</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public async Task<ValidateResponseBody> Validate()
    {
        var request = new RestRequest("oauth2/validate", Method.Get)
            .AddHeader("Authorization", "Bearer " + AccessToken);

        var response = await _idClient.ExecuteAsync(request);
        Logger?.LogTrace("User '{id}' validate response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Refresh();
            request.AddOrUpdateHeader("Authorization", "Bearer " + AccessToken);
            response = await _idClient.ExecuteAsync(request);
            Logger?.LogTrace("User '{id}' validate response {status}: {content}", UserId, response.StatusCode, response.Content);
        }
        if (!response.IsSuccessful)
        {
            var error = _idClient.Deserialize<ErrorResponse>(response).Data;
            throw GenerateTwitchErrorException(response.StatusCode, error);
        }

        var data = _idClient.Deserialize<ValidateResponseBody>(response).Data;
        if (data == null)
            throw new NullReferenceException("Twitch returns success validate response without body");

        Scopes = data.Scopes;
        ExpiresIn = DateTime.UtcNow.AddSeconds(data.ExpiresIn);
        UserId = data.UserId;
        Login = data.Login;
        _isValidated = true;
        return data;
    }

    /// <summary>Request to api.twitch.tv</summary>
    /// <typeparam name="TResult">Response type</typeparam>
    /// <param name="request">Request</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public async Task<RestResponse<TResult>> APIRequest<TResult>(RestRequest request) => _apiClient.Deserialize<TResult>(await APIRequest(request));

    /// <summary>Request to api.twitch.tv</summary>
    /// <param name="request">Request</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public async Task<RestResponse> APIRequest(RestRequest request)
    {
        if (!_isValidated)
            throw new NotValidatedException();

        request.AddHeader("Client-Id", _clientId);
        request.AddHeader("Authorization", "Bearer " + AccessToken);

        var uri = _apiClient.BuildUri(request);

        Logger?.LogTrace("User '{id}' api request {method} {uri}", UserId, request.Method, uri);
        var response = await _apiClient.ExecuteAsync(request);
        Logger?.LogTrace("User '{id}' api response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Refresh();
            request.AddOrUpdateHeader("Authorization", "Bearer " + AccessToken);
            Logger?.LogTrace("User '{id}' api request {method} {uri}", UserId, request.Method, uri);
            response = await _apiClient.ExecuteAsync(request);
            Logger?.LogTrace("User '{id}' api response {status}: {content}", UserId, response.StatusCode, response.Content);
        }
        if (!response.IsSuccessful)
        {
            var error = _apiClient.Deserialize<ErrorResponse>(response).Data;
            throw GenerateTwitchErrorException(response.StatusCode, error);
        }
        return response;
    }

    private static TwitchErrorException GenerateTwitchErrorException(HttpStatusCode status, ErrorResponse? error)
    {
        if (status == HttpStatusCode.BadRequest)
            return new BadRequestException(error?.Message);

        if (status == HttpStatusCode.Unauthorized)
            return new UnauthorizedException(error?.Message);

        if (status == HttpStatusCode.Forbidden)
            return new ForbiddenException(error?.Message);

        if (status == HttpStatusCode.NotFound)
            return new NotFoundException(error?.Message);

        if (500 <= (int)status && (int)status < 600)
            return new ServerErrorException((int)status, error?.Error, error?.Message);

        return new TwitchErrorException((int)status, error?.Error, error?.Message);
    }
}
