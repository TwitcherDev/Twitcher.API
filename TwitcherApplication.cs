﻿using Microsoft.Extensions.Logging;

namespace Twitcher.API;

/// <summary>Combines tools for creating, storing, and working with <see cref="TwitcherAPI"/> in the application</summary>
public class TwitcherApplication
{
    private readonly StateCollection? _states;
    private readonly TwitcherAPICollection? _collection;
    private readonly ILoggerFactory? _loggerFactory;
    private readonly ILogger? _logger;

    /// <summary>Id of the application</summary>
    public string ClientId { get; }
    /// <summary>Secret of the application</summary>
    public string ClientSecret { get; }

    /// <summary>Managing <see cref="TwitcherAPI"/> instances in the application for constant access without creating unnecessary instances</summary>
    public TwitcherAPICollection Collection => _collection ??
            throw new NotSupportedException($"Not enabled. Use {nameof(TwitcherApplicationBuilder.UseAPICollection)} in builder for enable");

    internal TwitcherApplication(string clientId, string clientSecret, StateCollection? stateCollection, TwitcherAPICollection? twitcherAPICollection, ILoggerFactory? loggerFactory = null, ILogger? logger = null)
    {
        _states = stateCollection;
        _collection = twitcherAPICollection;
        _loggerFactory = loggerFactory;
        _logger = logger ?? _loggerFactory?.CreateLogger<TwitcherApplication>();

        ClientId = clientId;
        ClientSecret = clientSecret;
    }

    /// <summary>Generate randomly state for secure authorization</summary>
    /// <returns>New state</returns>
    /// <exception cref="NullReferenceException"></exception>
    public string GenerateState()
    {
        if (_states == default)
            throw new NotSupportedException($"Not enabled. Use {nameof(TwitcherApplicationBuilder.UseAuthorizeStates)} in builder for enable");
        var state = _states.CreateState();
        _logger?.LogTrace("State created: {state}", state);
        return state;
    }

    /// <summary>Creates a link to generate a new token using Authorization code grant flow. Adds random state</summary>
    /// <param name="redirectUri">Redirect uri</param>
    /// <param name="scopes">Scopes</param>
    /// <returns>Created uri</returns>
    public string GenerateAuthorizeLink(string redirectUri, IEnumerable<string> scopes) => GenerateAuthorizeLink(redirectUri, string.Join(' ', scopes));

    /// <summary>Creates a link to generate a new token using Authorization code grant flow. Adds random state</summary>
    /// <param name="redirectUri">Redirect uri</param>
    /// <param name="scopes">Scopes separated by a space</param>
    /// <returns>Created uri</returns>
    public string GenerateAuthorizeLink(string redirectUri, string scopes)
    {
        if (_states != null)
            return $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientId}&redirect_uri={redirectUri}&scope={scopes}&state={GenerateState()}";
        return $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientId}&redirect_uri={redirectUri}&scope={scopes}";
    }

    /// <summary>Using authorization code grant flow with state for generate new token</summary>
    /// <param name="code">Code</param>
    /// <param name="redirectUri">Redirect uri</param>
    /// <param name="state">State that was generated by this application</param>
    /// <returns>Instance of <see cref="TwitcherAPI"/> created from the received token</returns>
    /// <exception cref="WrongStateException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public Task<TwitcherAPI> AuthorizeCode(string code, string redirectUri, string state)
    {
        if (_states == default)
            throw new NotSupportedException($"Not enabled. Use {nameof(TwitcherApplicationBuilder.UseAuthorizeStates)} in builder for enable");

        if (!_states.PassingState(state))
        {
            _logger?.LogTrace("Wrong state: {state}", state);
            throw new WrongStateException();
        }

        _logger?.LogTrace("State validated: {state}", state);

        return AuthorizeCode(code, redirectUri);
    }

    /// <summary>Using authorization code grant flow for generate new token</summary>
    /// <param name="code">Code</param>
    /// <param name="redirectUri">Redirect uri</param>
    /// <returns>Instance of <see cref="TwitcherAPI"/> created from the received token</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public Task<TwitcherAPI> AuthorizeCode(string code, string redirectUri)
    {
        return TwitcherAPI.AuthorizeCode(code, redirectUri, ClientId, ClientSecret, _loggerFactory?.CreateLogger<TwitcherAPI>() ?? _logger);
    }

    /// <summary>Creates a new instance of the <see cref="TwitcherAPI"/> and validates it</summary>
    /// <param name="tokens">Access and refresh tokens in 'access:refresh' format, which will be used to create a new instance of the <see cref="TwitcherAPI"/></param>
    /// <returns>Created <see cref="TwitcherAPI"/> instance</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public Task<TwitcherAPI> CreateAPI(string tokens)
    {
        var api = new TwitcherAPI(tokens, ClientId, ClientSecret, _loggerFactory?.CreateLogger<TwitcherAPI>() ?? _logger);
        return ValidateAPI(api);
    }

    /// <summary>Creates a new instance of the <see cref="TwitcherAPI"/> and validates it</summary>
    /// <param name="access">Access token, which will be used to create a new instance of the <see cref="TwitcherAPI"/></param>
    /// <param name="refresh">Refresh token pair for <paramref name="access"/></param>
    /// <returns>Created <see cref="TwitcherAPI"/> instance</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public Task<TwitcherAPI> CreateAPI(string access, string refresh)
    {
        var api = new TwitcherAPI(access, refresh, ClientId, ClientSecret, _loggerFactory?.CreateLogger<TwitcherAPI>() ?? _logger);
        return ValidateAPI(api);
    }

    private static async Task<TwitcherAPI> ValidateAPI(TwitcherAPI api)
    {
        await api.Validate();
        return api;
    }
}
