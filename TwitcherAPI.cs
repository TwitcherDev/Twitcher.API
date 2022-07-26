using Microsoft.Extensions.Logging;
using Twitcher.API.Events;

namespace Twitcher.API;

/// <summary></summary>
public class TwitcherAPI
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly RestClient _idClient;
    private readonly RestClient _apiClient;

    private bool _isValidated = false;

    /// <summary>Twitch id of the token owner</summary>
    public string? UserId { get; private set; }
    /// <summary>Twitch login of the token owner</summary>
    public string? Login { get; private set; }
    /// <summary>Access token</summary>
    public string AccessToken { get; private set; }
    /// <summary>Refresh token</summary>
    public string RefreshToken { get; private set; }
    /// <summary>Access token expiration time</summary>
    public DateTime ExpiresIn { get; private set; }
    /// <summary>Token scopes</summary>
    public string[]? Scopes { get; private set; }
    /// <summary>Logger</summary>
    public ILogger? Logger { get; set; }

    public bool IsRefreshed { get; private set; } = false;
    public string SaveTokens()
    {
        IsRefreshed = false;
        return AccessToken + ':' + RefreshToken;
    }

    public event EventHandler<APITokenRefreshedArgs>? TokenRefreshed;
    public event EventHandler? TokenDead;

    /// <summary></summary>
    /// <param name="tokens">Access and refresh token in the format: 'access:refresh'</param>
    /// <param name="clientId">Application client id</param>
    /// <param name="clientSecret">Application client secret</param>
    /// <param name="userId">Twitch id of the token owner if known</param>
    public TwitcherAPI(string tokens, string clientId, string clientSecret, string? userId = default) :
        this(tokens, clientId, clientSecret, new RestClient("https://id.twitch.tv").UseSerializer<JsonSnakeSerializer>(), new RestClient("https://api.twitch.tv").UseSerializer<JsonSnakeSerializer>(), userId) { }

    internal TwitcherAPI(string tokens, string clientId, string clientSecret, RestClient idClient, RestClient apiClient, string? userId = default)
    {
        if (string.IsNullOrEmpty(tokens))
            throw new ArgumentNullException(nameof(tokens));
        
        var s = tokens.Split(':', StringSplitOptions.RemoveEmptyEntries);
        if (s.Length != 2)
            throw new ArgumentException("Tokens format: 'access:refresh'", nameof(tokens));

        UserId = userId;
        AccessToken = s[0];
        RefreshToken = s[1];
        ExpiresIn = DateTime.UtcNow;

        _clientId = clientId;
        _clientSecret = clientSecret;
        _idClient = idClient;
        _apiClient = apiClient;
    }

    /// <summary>Refresh access token request</summary>
    /// <returns>Refresh response</returns>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<RefreshResponseBody?> Refresh()
    {
        var request = new RestRequest("oauth2/token", Method.Post)
            .AddQueryParameter("grant_type", "refresh_token")
            .AddQueryParameter("client_id", _clientId)
            .AddQueryParameter("client_secret", _clientSecret)
            .AddQueryParameter("refresh_token", RefreshToken);

        var response = await _idClient.ExecuteAsync<RefreshResponseBody>(request);
        Logger?.LogTrace("User '{id}' refresh response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            TokenDead?.Invoke(this, EventArgs.Empty);
            throw new DeadTokenException();
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            throw new InternalServerException();

        if (response.Data != default &&
            !string.IsNullOrEmpty(response.Data.AccessToken) &&
            !string.IsNullOrEmpty(response.Data.RefreshToken))
        {
            AccessToken = response.Data.AccessToken;
            RefreshToken = response.Data.RefreshToken;
            Scopes = response.Data.Scopes;
            ExpiresIn = DateTime.UtcNow.AddSeconds(response.Data.ExpiresIn);
            IsRefreshed = true;
            _isValidated = true;
            TokenRefreshed?.Invoke(this, new APITokenRefreshedArgs(AccessToken + ':' + RefreshToken));
            Logger?.LogDebug("User '{id}' token refreshed", UserId);
        }

        return response.Data;
    }

    /// <summary>Validate request</summary>
    /// <returns>Validate response</returns>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<ValidateResponseBody?> Validate()
    {
        var request = new RestRequest("oauth2/validate", Method.Get)
            .AddHeader("Authorization", "Bearer " + AccessToken);

        var response = await _idClient.ExecuteAsync<ValidateResponseBody>(request);
        Logger?.LogTrace("User '{id}' validate response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Refresh();
            request.AddOrUpdateHeader("Authorization", "Bearer " + AccessToken);
            response = await _idClient.ExecuteAsync<ValidateResponseBody>(request);
            Logger?.LogTrace("User '{id}' second validate response {status}: {content}", UserId, response.StatusCode, response.Content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                TokenDead?.Invoke(this, EventArgs.Empty);
                throw new DeadTokenException();
            }
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            throw new InternalServerException();

        if (response.Data != default)
        {
            Scopes = response.Data.Scopes;
            ExpiresIn = DateTime.UtcNow.AddSeconds(response.Data.ExpiresIn);
            UserId = response.Data.UserId;
            Login = response.Data.Login;
            _isValidated = true;
            Logger?.LogDebug("User '{id}' token validated. Scopes: [{scopes}]. ExpiresIn: {expiresIn}", UserId, string.Join(", ", Scopes ?? new[] { "empty" }), response.Data.ExpiresIn);
        }

        return response.Data;
    }

    /// <summary>Validate tokens</summary>
    /// <returns><see langword="true" /> if the validate successful; otherwise, <see langword="false" /></returns>
    /// <exception cref="InternalServerException"></exception>
    public async Task<bool> Check()
    {
        try
        {
            var response = await Validate();
            return response != default;
        }
        catch (DeadTokenException)
        {
            return false;
        }
    }

    /// <summary>Request to api.twitch.tv</summary>
    /// <typeparam name="TResult">Response type</typeparam>
    /// <param name="request">Request data</param>
    /// <returns>Response data</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<RestResponse> APIRequest(RestRequest request)
    {
        if (!_isValidated)
            throw new NotValidatedException();

        request.AddHeader("Client-Id", _clientId);
        request.AddHeader("Authorization", "Bearer " + AccessToken);

        Logger?.LogTrace("User '{id}' api request {method} {uri}", UserId, request.Method, _apiClient.BuildUri(request));
        var response = await _apiClient.ExecuteAsync(request);
        Logger?.LogTrace("User '{id}' api response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Refresh();
            request.AddOrUpdateHeader("Authorization", "Bearer " + AccessToken);
            response = await _apiClient.ExecuteAsync(request);
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                TokenDead?.Invoke(this, EventArgs.Empty);
                throw new DeadTokenException();
            }
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            Logger?.LogError("Bad request {method} {uri}: {result}", request.Method, _apiClient.BuildUri(request), response.Content);
            throw new BadRequestException();
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            throw new InternalServerException();

        return response;
    }

    /// <summary>Request to api.twitch.tv</summary>
    /// <typeparam name="TResult">Response type</typeparam>
    /// <param name="request">Request data</param>
    /// <returns>Response data</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<RestResponse<TResult>> APIRequest<TResult>(RestRequest request)
    {
        if (!_isValidated)
            throw new NotValidatedException();

        request.AddHeader("Client-Id", _clientId);
        request.AddHeader("Authorization", "Bearer " + AccessToken);

        Logger?.LogTrace("User '{id}' api request {method} {uri}", UserId, request.Method, _apiClient.BuildUri(request));
        var response = await _apiClient.ExecuteAsync<TResult>(request);
        Logger?.LogTrace("User '{id}' api response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Refresh();
            request.AddOrUpdateHeader("Authorization", "Bearer " + AccessToken);
            response = await _apiClient.ExecuteAsync<TResult>(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                TokenDead?.Invoke(this, EventArgs.Empty);
                throw new DeadTokenException();
            }
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            Logger?.LogError("Bad request {method} {uri}: {result}", request.Method, _apiClient.BuildUri(request), response.Content);
            throw new BadRequestException();
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            throw new InternalServerException();

        return response;
    }
}
