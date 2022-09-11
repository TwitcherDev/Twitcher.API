using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>Extension methods with music enumerators</summary>
public static class MusicEnumerators
{
    /// <summary>Enumerates the tracks of a Soundtrack playlist.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">The ID of the Soundtrack playlist to get</param>
    /// <param name="first">Number of results to be returned per page. Limit: 100</param>
    /// <returns>An enumerator that enumerates all tracks, requesting a new page every <paramref name="first"/> tracks</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<SoundtrackTrack> EnumerateSoundtrackPlaylist(this TwitcherAPI api, string id, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetSoundtrackPlaylist(id, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var soundtrack in response.Data)
                yield return soundtrack;
        }
        while (cursor != null);
    }
}
