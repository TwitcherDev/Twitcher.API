using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>A class with extension methods for requesting stream tags page by page</summary>
public static class StreamTagEnumerators
{
    /// <summary>Enumerates the list of all stream tags that Twitch defines.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="first">The maximum number of tags to return per page. Maximum: 100</param>
    /// <returns>An enumerator that enumerates all stream tags, requesting a new page every <paramref name="first"/> tags</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<StreamTagResponseBody> EnumerateAllStreamTags(this TwitcherAPI api, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetAllStreamTags(first, cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var stream in response.Data)
                yield return stream;
        }
        while (cursor != null);
    }
}
