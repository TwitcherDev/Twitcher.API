namespace Twitcher.API.Requests;

/// <summary>A class with extension methods for requesting streams</summary>
public static class StreamRequests
{
    /// <summary>Gets the channel stream key for a user.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadStreamKey"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID of the broadcaster</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<StreamKeyResponseBody> GetStreamKey(this TwitcherAPI api, string broadcasterId)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);

        var request = new RestRequest("helix/streams/key", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        var response = await api.APIRequest<DataResponse<StreamKeyResponseBody[]>>(request);
        return response.Data!.Data.Single();
    }

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
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<StreamResponseBody[]>> GetStreams(this TwitcherAPI api, IEnumerable<string>? gameIds = null, IEnumerable<string>? languages = null, IEnumerable<string>? userIds = null, IEnumerable<string>? userLogins = null, int first = 20, string? before = null, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);

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

    /// <summary>Gets information about active streams belonging to channels that the authenticated user follows. Streams are returned sorted by number of current viewers, in descending order. Across multiple pages of results, there may be duplicate or missing streams, as viewers join and leave streams.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadFollows"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">Results will only include active streams from the channels that this Twitch user follows. <paramref name="userId"/> must match the User ID in <paramref name="api"/></param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<FollowedStreamResponseBody[]>> GetFollowedStreams(this TwitcherAPI api, string userId, int first = 100, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(userId);

        var request = new RestRequest("helix/streams/followed", Method.Get)
            .AddQueryParameter("user_id", userId);

        if (first != 100)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<FollowedStreamResponseBody[]>>(request);
        return response.Data!;
    }

    /// <summary>Gets a list of markers for either a specified user’s most recent stream or a specified VOD/video (stream), ordered by recency. A marker is an arbitrary point in a stream that the broadcaster wants to mark; e.g., to easily return to later. The only markers returned are those created by the user identified by the Bearer token.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">ID of the broadcaster from whose stream markers are returned. Only one of <paramref name="userId"/> and <paramref name="videoId"/> must be specified</param>
    /// <param name="videoId">ID of the VOD/video whose stream markers are returned. Only one of <paramref name="userId"/> and <paramref name="videoId"/> must be specified</param>
    /// <param name="first">Number of values to be returned when getting videos by user or game ID. Limit: 100</param>
    /// <param name="before">Cursor for backward pagination</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<GetStreamMarkerResponseBody[]>> GetStreamMarkers(this TwitcherAPI api, string? userId = null, string? videoId = null, int first = 20, string? before = null, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        if (userId == null == (videoId == null))
            throw new ArgumentException($"Only one of {nameof(userId)} and {nameof(videoId)} must be specified");

        var request = new RestRequest("helix/streams/markers", Method.Get);

        if (userId != null)
            request.AddQueryParameter("user_id", userId);

        if (videoId != null)
            request.AddQueryParameter("video_id", videoId);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (before != null)
            request.AddQueryParameter("before", before);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<GetStreamMarkerResponseBody[]>>(request);
        return response.Data!;
    }

    /// <summary>Creates a marker in the stream of a user specified by user ID.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">ID of the broadcaster in whose live stream the marker is created</param>
    /// <param name="description">Description of or comments on the marker. Max length is 140 characters</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<StreamMarkerResponseBody> CreateStreamMarker(this TwitcherAPI api, string userId, string? description = null)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(userId);

        var request = new RestRequest("helix/streams/markers", Method.Post)
            .AddBody(new StreamMarkerRequestBody(userId, description));

        var response = await api.APIRequest<DataResponse<StreamMarkerResponseBody[]>>(request);
        return response.Data!.Data.Single();
    }
}
