namespace Twitcher.API.Requests;

/// <summary>A class with extension methods for requesting subscriptions</summary>
public static class SubscriptionRequests
{
    /// <summary>Gets a list of users that subscribe to the specified broadcaster.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadSubscriptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID of the broadcaster. Must match the User ID in <paramref name="api"/></param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<BroadcasterSubscriptionResponseBody> GetBroadcasterSubscriptions(this TwitcherAPI api, string broadcasterId, int first = 20, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);

        var request = new RestRequest("helix/subscriptions", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<BroadcasterSubscriptionResponseBody>(request);
        return response.Data!;
    }

    /// <summary>Gets a list of users that subscribe to the specified broadcaster.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadSubscriptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID of the broadcaster. Must match the User ID in <paramref name="api"/></param>
    /// <param name="userId">User ID of the specified subscriber</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<BroadcasterSubscription?> GetBroadcasterSubscriptions(this TwitcherAPI api, string broadcasterId, string userId)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);
        ArgumentNullException.ThrowIfNull(userId);

        var request = new RestRequest("helix/subscriptions", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("user_id", userId);

        var response = await api.APIRequest<DataResponse<BroadcasterSubscription[]>>(request);
        return response.Data!.Data.SingleOrDefault();
    }

    /// <summary>Gets a list of users that subscribe to the specified broadcaster.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadSubscriptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID of the broadcaster. Must match the User ID in <paramref name="api"/></param>
    /// <param name="userIds">Filters the list to include only the specified subscribers. You may specify a maximum of 100 subscribers</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<BroadcasterSubscription[]> GetBroadcasterSubscriptions(this TwitcherAPI api, string broadcasterId, IEnumerable<string> userIds)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);
        ArgumentNullException.ThrowIfNull(userIds);

        var request = new RestRequest("helix/subscriptions", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        var isAny = false;
        foreach (var id in userIds)
        {
            request.AddQueryParameter("user_id", id);
            isAny = true;
        }
        if (!isAny)
            throw new ArgumentException("Cannot be empty", nameof(userIds));

        var response = await api.APIRequest<DataResponse<BroadcasterSubscription[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Checks if a specific user <paramref name="userId"/> is subscribed to a specific channel <paramref name="broadcasterId"/>.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadSubscriptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID of an Affiliate or Partner broadcaster</param>
    /// <param name="userId">User ID of a Twitch viewer</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserSubscription> CheckUserSubscription(this TwitcherAPI api, string broadcasterId, string userId)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);
        ArgumentNullException.ThrowIfNull(userId);

        var request = new RestRequest("helix/subscriptions/user", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("user_id", userId);

        var response = await api.APIRequest<DataResponse<UserSubscription[]>>(request);
        return response.Data!.Data.Single();
    }
}
