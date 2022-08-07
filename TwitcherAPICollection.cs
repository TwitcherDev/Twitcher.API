using Microsoft.Extensions.Logging;
using Twitcher.API.Events;

namespace Twitcher.API;

/// <summary>Managing <see cref="TwitcherAPI"/> instances in the application for constant access without creating unnecessary instances</summary>
public class TwitcherAPICollection
{
    private readonly Dictionary<string, TwitcherAPI> _apis;
    private readonly ILoggerFactory? _loggerFactory;
    private readonly ILogger? _logger;

    /// <summary>Id of the application</summary>
    public string ClientId { get; set; }
    /// <summary>Secret of the application</summary>
    public string ClientSecret { get; set; }

    /// <summary>Invoked when the token of one of the <see cref="TwitcherAPI"/> instances in collection has been changed</summary>
    public event EventHandler<APITokenRefreshedArgs>? TokenRefreshed;

    /// <summary>Create an instance of <see cref="TwitcherApplication"/></summary>
    /// <param name="clientId">Id of the application</param>
    /// <param name="clientSecret">Secret of the application</param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/> for logging</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TwitcherAPICollection(string clientId, string clientSecret, ILoggerFactory? loggerFactory = null)
    {
        _loggerFactory = loggerFactory;
        if (_loggerFactory != default)
            _logger = _loggerFactory.CreateLogger<TwitcherAPICollection>();

        ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
        _apis = new Dictionary<string, TwitcherAPI>();
    }

    /// <summary>Creates a new instance of the <see cref="TwitcherAPI"/>, validates it, and adds it to the collection. If an instance with this name exists, it replaces it with</summary>
    /// <param name="name">Any string to identify the <see cref="TwitcherAPI"/>, you can just use token owner's id or login</param>
    /// <param name="tokens">Access and refresh tokens in 'access:refresh' format, which will be used to create a new instance of the <see cref="TwitcherAPI"/></param>
    /// <returns>Created <see cref="TwitcherAPI"/> instance</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public Task<TwitcherAPI> CreateAPI(string name, string tokens)
    {
        var api = new TwitcherAPI(tokens, ClientId, ClientSecret, _loggerFactory?.CreateLogger<TwitcherAPI>());
        return AddAPI(name, api);
    }

    /// <summary>Creates a new instance of the <see cref="TwitcherAPI"/>, validates it, and adds it to the collection. If an instance with this name exists, it replaces it with</summary>
    /// <param name="name">Any string to identify the <see cref="TwitcherAPI"/>, you can just use token owner's id or login</param>
    /// <param name="access">Access token, which will be used to create a new instance of the <see cref="TwitcherAPI"/></param>
    /// <param name="refresh">Refresh token pair for <paramref name="access"/></param>
    /// <returns>Created <see cref="TwitcherAPI"/> instance</returns>
    /// <exception cref="TwitchErrorException"></exception>
    public Task<TwitcherAPI> CreateAPI(string name, string access, string refresh)
    {
        var api = new TwitcherAPI(access, refresh, ClientId, ClientSecret, _loggerFactory?.CreateLogger<TwitcherAPI>());
        return AddAPI(name, api);
    }

    /// <summary>Adds existing <see cref="TwitcherAPI"/> instance to the collection and validates it</summary>
    /// <param name="name">Any string to identify the <see cref="TwitcherAPI"/>, you can just use token owner's id or login</param>
    /// <param name="api"><see cref="TwitcherAPI"/> instance to add</param>
    /// <returns><paramref name="api"/></returns>
    public async Task<TwitcherAPI> AddAPI(string name, TwitcherAPI api)
    {
        api.TokenRefreshed += (s, e) => TokenRefreshed?.Invoke(this, new APITokenRefreshedArgs(name, e.AccessToken, e.RefreshToken, e.UserId));

        await api.Validate();

        bool exist;
        lock (_apis)
        {
            exist = _apis.ContainsKey(name);
            _apis[name] = api;
        }
        if (!exist)
            _logger?.LogDebug("'{name}' api created. Owner: '{id}'", name, api.UserId);
        else
            _logger?.LogDebug("'{name}' api replaced. New owner: '{id}'", name, api.UserId);

        return api;
    }

    /// <summary>Returns an <see cref="TwitcherAPI"/> instance by name or creates a new one using tokens</summary>
    /// <param name="name">Any string to identify the <see cref="TwitcherAPI"/>, you can just use token owner's id or login</param>
    /// <param name="tokens">Access and refresh tokens in 'access:refresh' format, which will be used to create a new instance of the <see cref="TwitcherAPI"/> if it is not found. If this value is not specified, an <see cref="KeyNotFoundException"/> will be thrown</param>
    /// <returns>An existing instance of the <see cref="TwitcherAPI"/> or a new one created by the <paramref name="tokens"/></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public async Task<TwitcherAPI> GetAPI(string name, string? tokens = null)
    {
        TwitcherAPI? api;
        lock (_apis)
            api = _apis.GetValueOrDefault(name);

        if (api != null)
            return api;

        if (tokens == null)
            throw new KeyNotFoundException();

        return await CreateAPI(name, tokens);
    }

    /// <summary>Returns an <see cref="TwitcherAPI"/> instance by name or creates a new one using tokens</summary>
    /// <param name="name">Any string to identify the <see cref="TwitcherAPI"/>, you can just use token owner's id or login</param>
    /// <param name="access">Access token, which will be used to create a new instance of the <see cref="TwitcherAPI"/> if it is not found. If this value is not specified, an <see cref="KeyNotFoundException"/> will be thrown</param>
    /// <param name="refresh">Refresh token pair for <paramref name="access"/></param>
    /// <returns>An existing instance of the <see cref="TwitcherAPI"/> or a new one created by the <paramref name="access"/> and <paramref name="refresh"/></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public async Task<TwitcherAPI> GetAPI(string name, string? access = null, string? refresh = null)
    {
        TwitcherAPI? api;
        lock (_apis)
            api = _apis.GetValueOrDefault(name);

        if (api != null)
            return api;

        if (access == null || refresh == null)
            throw new KeyNotFoundException();

        return await CreateAPI(name, access, refresh);
    }

    /// <summary>Removes <see cref="TwitcherAPI"/> instance from the collection by <paramref name="name"/></summary>
    /// <param name="name">The name of the <see cref="TwitcherAPI"/> instance to remove</param>
    /// <returns><see langword="true"/> if the element is successfully found and removed; otherwise, <see langword="false"/>. This method returns <see langword="false"/> if name is not found</returns>
    public bool RemoveAPI(string name)
    {
        bool isRemoved;
        lock (_apis)
            isRemoved = _apis.Remove(name);

        if (isRemoved)
            _logger?.LogDebug("'{name}' api removed", name);
        return isRemoved;
    }
}
