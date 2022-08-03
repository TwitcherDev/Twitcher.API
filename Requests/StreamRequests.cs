namespace Twitcher.API.Requests;

/// <summary>A class with extension methods for requesting streams</summary>
public static class StreamRequests
{
    /// <summary>Gets information about active streams. Streams are returned sorted by number of current viewers, in descending order</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="gameIds">Returns streams broadcasting a specified game ID. You can specify up to 100 IDs</param>
    /// <param name="languages">Stream language. You can specify up to 100 languages. A language value must be either the ISO 639-1 two-letter code</param>
    /// <param name="userIds">Returns streams broadcast by one or more specified user IDs. You can specify up to 100 IDs</param>
    /// <param name="userLogins">Returns streams broadcast by one or more specified user login names. You can specify up to 100 names</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="before">Cursor for backward pagination</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<StreamResponseBody[]>> GetStreams(this TwitcherAPI api, IEnumerable<string>? gameIds = null, IEnumerable<string>? languages = null, IEnumerable<string>? userIds = null, IEnumerable<string>? userLogins = null, int first = 20, string? before = null, string? after = null)
    {
        var request = new RestRequest("helix/streams", Method.Get);

        if (gameIds != null)
            foreach (var id in gameIds)
                request.AddQueryParameter("game_id", id);

        if (languages != null)
            foreach (var id in languages)
                request.AddQueryParameter("language", id);

        if (userIds != null)
            foreach (var id in userIds)
                request.AddQueryParameter("user_id", id);

        if (userLogins != null)
            foreach (var id in userLogins)
                request.AddQueryParameter("user_login", id);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (before != null)
            request.AddQueryParameter("before", before);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<StreamResponseBody[]>>(request);
        return response.Data!;
    }
}
