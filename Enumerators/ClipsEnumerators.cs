using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>Extension methods with clips enumerators</summary>
public static class ClipsEnumerators
{
    /// <summary>Enumerates clips by clip IDs.
    /// Splits the <paramref name="ids"/> collection into chunks of the <paramref name="first"/> id in each. For each chunk calls a new request</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">IDs of the clips being queried</param>
    /// <param name="first">Number of results to be returned per page. Limit: 100</param>
    /// <returns>An enumerator that enumerates all clips, requesting a new page every <paramref name="first"/> clips</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<ClipData> EnumerateClips(this TwitcherAPI api, IEnumerable<string> ids, int first = 100)
    {
        foreach (var idsBucket in ids.UnsafeBatch(first))
            foreach (var clip in await api.GetClips(idsBucket))
                yield return clip;
    }

    /// <summary>Enumerates clips by broadcaster ID.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">ID of the broadcaster for whom clips are returned</param>
    /// <param name="startedAt">Starting date/time for returned clips. If this is specified, <paramref name="endedAt"/> also should be specified; otherwise, the <paramref name="endedAt"/> date/time will be 1 week after the <paramref name="startedAt"/> value</param>
    /// <param name="endedAt">Ending date/time for returned clips. If this is specified, <paramref name="startedAt"/> also must be specified; otherwise, the time period is ignored</param>
    /// <param name="first">Number of results to be returned per page. Limit: 100</param>
    /// <returns>An enumerator that enumerates all clips, requesting a new page every <paramref name="first"/> clips</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<ClipData> EnumerateClipsByBroadcaster(this TwitcherAPI api, string broadcasterId, DateTime? startedAt = null, DateTime? endedAt = null, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetClipsByBroadcaster(broadcasterId, startedAt, endedAt, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            if (response.Data is not null)
                foreach (var clip in response.Data)
                    yield return clip;
        }
        while (cursor != null);
    }

    /// <summary>Enumerates clips by game ID.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="gameId">ID of the game for which clips are returned</param>
    /// <param name="startedAt">Starting date/time for returned clips. If this is specified, <paramref name="endedAt"/> also should be specified; otherwise, the <paramref name="endedAt"/> date/time will be 1 week after the <paramref name="startedAt"/> value</param>
    /// <param name="endedAt">Ending date/time for returned clips. If this is specified, <paramref name="startedAt"/> also must be specified; otherwise, the time period is ignored</param>
    /// <param name="first">Number of results to be returned per page. Limit: 100</param>
    /// <returns>An enumerator that enumerates all clips, requesting a new page every <paramref name="first"/> clips</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<ClipData> EnumerateClipsByGame(this TwitcherAPI api, string gameId, DateTime? startedAt = null, DateTime? endedAt = null, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetClipsByGame(gameId, startedAt, endedAt, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            if (response.Data is not null)
                foreach (var clip in response.Data)
                    yield return clip;
        }
        while (cursor != null);
    }
}
