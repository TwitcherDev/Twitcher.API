using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>A class with extension methods for requesting streams page by page</summary>
public static class StreamEnumerators
{
    /// <summary>Enumerates information about active streams. Streams are returned sorted by number of current viewers, in descending order.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="gameIds">Returns streams broadcasting a specified game ID. You can specify up to 100 IDs</param>
    /// <param name="languages">Stream language. You can specify up to 100 languages. A language value must be either the ISO 639-1 two-letter code</param>
    /// <param name="first">Number of results to be returned per page. Limit: 50</param>
    /// <returns>An enumerator that enumerates all active streams, requesting a new page every <paramref name="first"/> streams</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<StreamResponseBody> EnumerateStreams(this TwitcherAPI api, IEnumerable<string>? gameIds = null, IEnumerable<string>? languages = null, int first = 50)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetStreams(gameIds, languages, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var stream in response.Data)
                yield return stream;
        }
        while (cursor != null);
    }

    /// <summary>Enumerates information about active streams belonging to channels that the authenticated user follows. Streams are returned sorted by number of current viewers, in descending order. Across multiple pages of results, there may be duplicate or missing streams, as viewers join and leave streams.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadFollows"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">Results will only include active streams from the channels that this Twitch user follows. <paramref name="userId"/> must match the User ID in <paramref name="api"/></param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <returns>An enumerator that enumerates streams, requesting a new page every <paramref name="first"/> streams</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<FollowedStreamResponseBody> EnumerateFollowedStreams(this TwitcherAPI api, string userId, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetFollowedStreams(userId, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var stream in response.Data)
                yield return stream;
        }
        while (cursor != null);
    }

    /// <summary>Enumerates markers for either a specified user’s most recent stream or a specified VOD/video (stream), ordered by recency. A marker is an arbitrary point in a stream that the broadcaster wants to mark; e.g., to easily return to later. The only markers returned are those created by the user identified by the Bearer token.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">ID of the broadcaster from whose stream markers are returned. Only one of <paramref name="userId"/> and <paramref name="videoId"/> must be specified</param>
    /// <param name="videoId">ID of the VOD/video whose stream markers are returned. Only one of <paramref name="userId"/> and <paramref name="videoId"/> must be specified</param>
    /// <param name="first">Number of values to be returned when getting videos by user or game ID. Limit: 100</param>
    /// <returns>An enumerator that enumerates stream markers, requesting a new page every <paramref name="first"/> stream markers</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<GetStreamMarkerResponseBody> EnumerateStreamMarkers(this TwitcherAPI api, string? userId = null, string? videoId = null, int first = 20)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetStreamMarkers(userId, videoId, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var marker in response.Data)
                yield return marker;
        }
        while (cursor != null);
    }
}
