using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>Extension methods with games enumerators</summary>
public static class GamesEnumerators
{
    /// <summary>Enumerates top games.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="first">Number of results to be returned per page. Limit: 100</param>
    /// <returns>An enumerator that enumerates all top games, requesting a new page every <paramref name="first"/> games</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<GameMetadata> EnumerateTopGames(this TwitcherAPI api, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetTopGames(first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            if (response.Data is not null)
                foreach (var game in response.Data)
                    yield return game;
        }
        while (cursor != null);
    }
}
