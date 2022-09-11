namespace Twitcher.API.Linq;

/// <summary>Linq style request methods</summary>
public static class TwitcherAPIExtensions
{
    /// <summary>Request to api.twitch.tv with authorization</summary>
    /// <typeparam name="TResult">Response body type</typeparam>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<RestResponse<TResult>> APIRequest<TResult>(this TwitcherAPI api, string resource, Method method, Action<RestRequest>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        return api.APIRequest<TResult>(CreateRequest(resource, method, parameters));
    }

    /// <summary>Request to api.twitch.tv with authorization</summary>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<RestResponse> APIRequest(this TwitcherAPI api, string resource, Method method, Action<RestRequest>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        return api.APIRequest(CreateRequest(resource, method, parameters));
    }

    private static RestRequest CreateRequest(string resource, Method method, Action<RestRequest>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(resource);
        ArgumentNullException.ThrowIfNull(parameters);

        var request = new RestRequest(resource, method);
        parameters?.Invoke(request);
        return request;
    }
}
