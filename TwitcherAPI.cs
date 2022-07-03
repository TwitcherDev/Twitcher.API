using Microsoft.Extensions.Logging;
using RestSharp;
using System.Net;
using Twitcher.API.Exceptions;
using Twitcher.API.Models.Events;
using Twitcher.API.Models.Responses;

namespace Twitcher.API;

public class TwitcherAPI
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly RestClient _idClient;
    private readonly RestClient _apiClient;

    public string? UserId { get; private set; }
    public string? Login { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime ExpiresIn { get; private set; }
    public string[]? Scopes { get; private set; }
    public ILogger? Logger { get; set; }

    public event EventHandler<APITokenRefreshedArgs>? TokenRefreshed;
    public event EventHandler? TokenDead;

    public TwitcherAPI(string tokens, string clientId, string clientSecret, string? userId = default) :
        this(tokens, clientId, clientSecret, new RestClient("https://id.twitch.tv"), new RestClient("https://api.twitch.tv"), userId) { }

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

    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<RefreshResponse?> Refresh()
    {
        var request = new RestRequest("oauth2/token", Method.Post)
            .AddQueryParameter("grant_type", "refresh_token")
            .AddQueryParameter("client_id", _clientId)
            .AddQueryParameter("client_secret", _clientSecret)
            .AddQueryParameter("refresh_token", RefreshToken);

        var response = await _idClient.ExecuteAsync<RefreshResponse>(request);
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
            TokenRefreshed?.Invoke(this, new APITokenRefreshedArgs(AccessToken + ':' + RefreshToken));
            Logger?.LogDebug("User '{id}' token refreshed", UserId);
        }

        return response.Data;
    }

    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<ValidateResponse?> Validate()
    {
        var request = new RestRequest("oauth2/validate", Method.Get)
            .AddHeader("Authorization", "Bearer " + AccessToken);

        var response = await _idClient.ExecuteAsync<ValidateResponse>(request);
        Logger?.LogTrace("User '{id}' validate response {status}: {content}", UserId, response.StatusCode, response.Content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await Refresh();
            request.AddOrUpdateHeader("Authorization", "Bearer " + AccessToken);
            response = await _idClient.ExecuteAsync<ValidateResponse>(request);
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
            Logger?.LogDebug("User '{id}' token validated. Scopes: [{scopes}]. ExpiresIn: {expiresIn}", UserId, string.Join(", ", Scopes ?? new[] { "empty" }), response.Data.ExpiresIn);
        }

        return response.Data;
    }

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

    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public async Task<RestResponse<TResult>> APIRequest<TResult>(RestRequest request)
    {
        request.AddHeader("Client-Id", _clientId);
        request.AddHeader("Authorization", "Bearer " + AccessToken);

        Logger?.LogTrace("User '{id}' api request {uri}", UserId, _apiClient.BuildUri(request));
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
            Logger?.LogError("Bad request {uri}: {result}", _apiClient.BuildUri(request), response.Content);
            throw new BadRequestException();
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            throw new InternalServerException();

        return response;
    }
}
