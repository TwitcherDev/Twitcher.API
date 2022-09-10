using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>Extension methods with search requests enumerators</summary>
public static class SearchEnumerators
{
    /// <summary>Enumerates games and categories that match the query via name either entirely or partially.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="query">Search query</param>
    /// <param name="first">Number of results to be returned per page. Limit: 100</param>
    /// <returns>An enumerator that enumerates all categories, requesting a new page every <paramref name="first"/> categories</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<SearchCategoryMetadata> EnumerateSearchCategories(this TwitcherAPI api, string query, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.SearchCategories(query, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var category in response.Data)
                yield return category;
        }
        while (cursor != null);
    }

    /// <summary>Enumerates channels that match the query via name either entirely or partially.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="query">Search query</param>
    /// <param name="liveOnly">Filter results for live streams only</param>
    /// <param name="first">Number of results to be returned per page. Limit: 100</param>
    /// <returns>An enumerator that enumerates all channels, requesting a new page every <paramref name="first"/> channels</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<SearchChannelMetadata> EnumerateSearchChannels(this TwitcherAPI api, string query, bool liveOnly = false, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.SearchChannels(query, liveOnly, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var channel in response.Data)
                yield return channel;
        }
        while (cursor != null);
    }
}