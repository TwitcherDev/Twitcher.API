namespace Twitcher.API.Requests;

/// <summary>Extension methods with clips requests</summary>
public static class ClipsRequests
{
    /// <summary>Creates a clip programmatically. This returns both an ID and an edit URL for the new clip.
    /// Required scope: '<inheritdoc cref="Scopes.ClipsEdit"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">ID of the stream from which the clip will be made</param>
    /// <param name="hasDelay">If <see langword="false"/>, the clip is captured from the live stream when the API is called; otherwise, a delay is added before the clip is captured (to account for the brief delay between the broadcaster’s stream and the viewer’s experience of that stream)</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ClipMetadata> CreateClip(this TwitcherAPI api, string broadcasterId, bool hasDelay = false) =>
        (await api.APIRequest<DataResponse<ClipMetadata[]>>("helix/clips", RequestMethod.Post, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterOrDefault("has_delay", hasDelay, false)))
            .Data.Single();

    /// <summary>Gets clip information by clip ID</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">ID of the clip being queried</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ClipData?> GetClips(this TwitcherAPI api, string id) =>
        (await api.APIRequest<DataResponse<ClipData[]?>>("helix/clips", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("id", id)))
            .Data?.SingleOrDefault();

    /// <summary>Gets clip information by clip IDs</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">IDs of the clips being queried. Limit: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ClipData[]> GetClips(this TwitcherAPI api, IEnumerable<string> ids) =>
        (await api.APIRequest<DataResponse<ClipData[]?>>("helix/clips", RequestMethod.Get, r => r
            .AddQueryParametersNotEmpty("id", ids)))
            .Data ?? Array.Empty<ClipData>();

    /// <summary>Gets clip information by broadcaster ID</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">ID of the broadcaster for whom clips are returned</param>
    /// <param name="startedAt">Starting date/time for returned clips. If this is specified, <paramref name="endedAt"/> also should be specified; otherwise, the <paramref name="endedAt"/> date/time will be 1 week after the <paramref name="startedAt"/> value</param>
    /// <param name="endedAt">Ending date/time for returned clips. If this is specified, <paramref name="startedAt"/> also must be specified; otherwise, the time period is ignored</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="before">Cursor for backward pagination</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<ClipData[]?>> GetClipsByBroadcaster(this TwitcherAPI api, string broadcasterId, DateTime? startedAt = null, DateTime? endedAt = null, int first = 20, string? before = null, string? after = null) =>
        await api.APIRequest<DataPaginationResponse<ClipData[]?>>("helix/clips", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterOrDefault("started_at", startedAt)
            .AddQueryParameterOrDefault("ended_at", endedAt)
            .AddQueryParameterOrDefault("first", first, 20)
            .AddQueryParameterOrDefault("before", before)
            .AddQueryParameterOrDefault("after", after));

    /// <summary>Gets clip information by game ID</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="gameId">ID of the game for which clips are returned</param>
    /// <param name="startedAt">Starting date/time for returned clips. If this is specified, <paramref name="endedAt"/> also should be specified; otherwise, the <paramref name="endedAt"/> date/time will be 1 week after the <paramref name="startedAt"/> value</param>
    /// <param name="endedAt">Ending date/time for returned clips. If this is specified, <paramref name="startedAt"/> also must be specified; otherwise, the time period is ignored</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="before">Cursor for backward pagination</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<ClipData[]?>> GetClipsByGame(this TwitcherAPI api, string gameId, DateTime? startedAt = null, DateTime? endedAt = null, int first = 20, string? before = null, string? after = null) =>
        await api.APIRequest<DataPaginationResponse<ClipData[]?>>("helix/clips", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("game_id", gameId)
            .AddQueryParameterOrDefault("started_at", startedAt)
            .AddQueryParameterOrDefault("ended_at", endedAt)
            .AddQueryParameterOrDefault("first", first, 20)
            .AddQueryParameterOrDefault("before", before)
            .AddQueryParameterOrDefault("after", after));
}
