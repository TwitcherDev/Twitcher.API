using Twitcher.API.Requests;

namespace Twitcher.API.Enumerators;

/// <summary>Extension methods with event subs enumerators</summary>
public static class EventSubEnumerators
{
    /// <summary>Enumerates event subs subscriptions.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <returns>An enumerator that enumerates all event sub subscriptions, requesting a new page when the current one ends</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<EventSubData> EnumerateEventSubSubscriptions(this TwitcherAPI api)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetEventSubSubscriptions(after: cursor);
            cursor = response.Pagination?.Cursor;
            if (response.Data is not null)
                foreach (var subscription in response.Data)
                    yield return subscription;
        }
        while (cursor != null);
    }

    /// <summary>Enumerates event subs subscriptions.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="status">Filter subscriptions by its status</param>
    /// <returns>An enumerator that enumerates all event sub subscriptions, requesting a new page when the current one ends</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<EventSubData> EnumerateEventSubSubscriptionsByStatus(this TwitcherAPI api, EventSubStatus status)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetEventSubSubscriptionsByStatus(status, after: cursor);
            cursor = response.Pagination?.Cursor;
            if (response.Data is not null)
                foreach (var subscription in response.Data)
                    yield return subscription;
        }
        while (cursor != null);
    }

    /// <summary>Enumerates event subs subscriptions.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="type">Filter subscriptions by subscription type</param>
    /// <returns>An enumerator that enumerates all event sub subscriptions, requesting a new page when the current one ends</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<EventSubData> EnumerateEventSubSubscriptionsByType(this TwitcherAPI api, string type)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetEventSubSubscriptionsByType(type, after: cursor);
            cursor = response.Pagination?.Cursor;
            if (response.Data is not null)
                foreach (var subscription in response.Data)
                    yield return subscription;
        }
        while (cursor != null);
    }

    /// <summary>Enumerates event subs subscriptions.
    /// Automatically requesting a new page when overriding</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">Filter subscriptions by user ID</param>
    /// <returns>An enumerator that enumerates all event sub subscriptions, requesting a new page when the current one ends</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<EventSubData> EnumerateEventSubSubscriptionsByUserId(this TwitcherAPI api, string userId)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetEventSubSubscriptionsByUserId(userId, after: cursor);
            cursor = response.Pagination?.Cursor;
            if (response.Data is not null)
                foreach (var subscription in response.Data)
                    yield return subscription;
        }
        while (cursor != null);
    }
}

