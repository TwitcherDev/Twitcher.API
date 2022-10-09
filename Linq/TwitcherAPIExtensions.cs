namespace Twitcher.API.Linq;

/// <summary>Linq style request methods</summary>
public static class TwitcherAPIExtensions
{
    /// <summary>Custom request to api.twitch.tv with authorization</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="resource">The resource on <see href="https://api.twitch.tv"/> that you need to make a request to. For example: 'helix/users'</param>
    /// <param name="method">HTTP method you need to make the request</param>
    /// <param name="parameters">Action to add parameters to the request</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TokenRevokedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task APIRequest(this TwitcherAPI api, string resource, RequestMethod method, Action<TwitchRequest>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        return api.APIRequest(CreateRequest(resource, method, parameters));
    }

    /// <summary>Custom request to api.twitch.tv with authorization</summary>
    /// <typeparam name="TResult">Response body type</typeparam>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="resource">The resource on <see href="https://api.twitch.tv"/> that you need to make a request to. For example: 'helix/users'</param>
    /// <param name="method">HTTP method you need to make the request</param>
    /// <param name="parameters">Action to add parameters to the request</param>
    /// <returns>Response body</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TokenRevokedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    /// <exception cref="TwitchEmptyBodyException"></exception>
    public static async Task<TResult> APIRequest<TResult>(this TwitcherAPI api, string resource, RequestMethod method, Action<TwitchRequest>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        return (await api.APIRequest<TResult>(CreateRequest(resource, method, parameters))).Data ??
            throw new TwitchEmptyBodyException();
    }

    private static RestRequest CreateRequest(string resource, RequestMethod method, Action<TwitchRequest>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(resource);

        var request = new TwitchRequest(resource, method);
        parameters?.Invoke(request);
        return request;
    }
}
