using Twitcher.API.Requests;

namespace Twitcher.API.Paginators;

/// <summary>A class with extension methods for requesting custom rewards page by page</summary>
public static class CustomRewardPaginators
{
    /// <summary>Returns all Custom Reward Redemption objects for a Custom Reward on a channel that was created by the same client_id.
    /// Automatically requesting a new page when overriding.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadRedemptions"/>' or '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="rewardId">The id of the custom reward on <paramref name="broadcasterId"/> channel for which you need to get redemptions</param>
    /// <param name="status">Filters the paginated Custom Reward Redemption objects for redemptions with the matching status</param>
    /// <param name="sort">Sort order of redemptions</param>
    /// <param name="first">Number of results to be returned per page. Limit: 50</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async IAsyncEnumerable<CustomRewardRedemptionResponseBody> EnumerateAllCustomRewardRedemptions(this TwitcherAPI api, string broadcasterId, Guid rewardId, RedemptionStatus status, SortOrder sort = SortOrder.Oldest, int first = 50)
    {
        string? cursor = null;
        do
        {
            var response = await api.GetCustomRewardRedemption(broadcasterId, rewardId, status: status, sort: sort, first: first, after: cursor);
            cursor = response.Pagination?.Cursor;
            foreach (var redemption in response.Data)
                yield return redemption;
        }
        while (cursor != null);
    }
}
