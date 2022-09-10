using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>A class with extension methods for requesting subscriptions page by page</summary>
public static class SubscriptionEnumerators
{
    /// <summary>Enumerates the list of all broadcaster subscriptions.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID of the broadcaster. Must match the User ID in <paramref name="api"/></param>
    /// <param name="first">The maximum number of tags to return per page. Maximum: 100</param>
    /// <returns>An enumerator that enumerates all subscriptions, requesting a new page every <paramref name="first"/> subscriptions</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<BroadcasterSubscription> EnumeratetBroadcasterSubscriptions(this TwitcherAPI api, string broadcasterId, int first = 100)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetBroadcasterSubscriptions(broadcasterId, first, cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var subscription in response.Data)
                yield return subscription;
        }
        while (cursor != null);
    }
}
