namespace Twitcher.API.Requests;

public static class CustomRewardRequests
{
    #region Rewards
    /// <summary>Creates a Custom Reward on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="body">Request body. <see cref="CustomRewardRequestBody.Title"/> and <see cref="CustomRewardRequestBody.Cost"/> are required</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    public static async Task<CustomRewardResponseBody?> CreateCustomReward(this TwitcherAPI api, string broadcasterId, CustomRewardRequestBody body)
    {
        if (api.Scopes == default || !api.Scopes.Contains(Scopes.ChannelManageRedemptions))
            throw new ScopeRequireException(Scopes.ChannelManageRedemptions);

        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Post)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddBody(body);

        var response = await api.APIRequest<DataResponse<CustomRewardResponseBody[]>>(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenException("Channel Points are not available for the broadcaster");

        return response.Data?.Data?.FirstOrDefault();
    }

    /// <summary>Deletes a Custom Reward on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="id">ID of the Custom Reward to delete, must match a Custom Reward on <paramref name="broadcasterId" /> channel</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public static async Task<bool> DeleteCustomReward(this TwitcherAPI api, string broadcasterId, Guid id)
    {
        if (api.Scopes == default || !api.Scopes.Contains(Scopes.ChannelManageRedemptions))
            throw new ScopeRequireException(Scopes.ChannelManageRedemptions);

        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Delete)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("id", id);

        var response = await api.APIRequest(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenException("The Custom Reward was created by a different ClientId or Channel Points are not available for the broadcaster");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("The Custom Reward doesn’t exist with the id and BroadcasterId specified");

        return response.IsSuccessful;
    }

    /// <summary>Returns a list of Custom Reward objects for the Custom Rewards on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="ids">When used, this parameter filters the results and only returns reward objects for the Custom Rewards with matching ID. Maximum: 50</param>
    /// <param name="onlyManageableRewards">When set to true, only returns custom rewards that the calling client_id can manage</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public static async Task<CustomRewardResponseBody[]?> GetCustomReward(this TwitcherAPI api, string broadcasterId, IEnumerable<Guid>? ids = null, bool onlyManageableRewards = false)
    {
        if (api.Scopes == default || !api.Scopes.Contains(Scopes.ChannelReadRedemptions))
            throw new ScopeRequireException(Scopes.ChannelReadRedemptions);

        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        if (ids != null)
            foreach (var id in ids)
                request.AddQueryParameter("id", id);

        if (onlyManageableRewards)
            request.AddQueryParameter("only_manageable_rewards", onlyManageableRewards);

        var response = await api.APIRequest<DataResponse<CustomRewardResponseBody[]>>(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenException("The Custom Reward was created by a different ClientId or Channel Points are not available for the broadcaster");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("The Custom Reward doesn’t exist with the id and BroadcasterId specified");

        return response.Data?.Data;
    }

    /// <summary>Updates a Custom Reward created on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="id">ID of the Custom Reward to update. Must match a Custom Reward on the channel of the <paramref name="broadcasterId"/></param>
    /// <param name="body">Request body</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public static async Task<CustomRewardResponseBody?> UpdateCustomReward(this TwitcherAPI api, string broadcasterId, Guid id, CustomRewardRequestBody body)
    {
        if (api.Scopes == default || !api.Scopes.Contains(Scopes.ChannelManageRedemptions))
            throw new ScopeRequireException(Scopes.ChannelManageRedemptions);

        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Patch)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("id", id)
            .AddBody(body);

        var response = await api.APIRequest<DataResponse<CustomRewardResponseBody[]>>(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenException("Channel Points are not available for the broadcaster");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("The Custom Reward doesn’t exist with the id and BroadcasterId specified");

        return response.Data?.Data?.FirstOrDefault();
    }

    #endregion

    #region Redemptions
    /// <summary>Returns Custom Reward Redemption objects for a Custom Reward on a channel that was created by the same client_id.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="rewardId">When <paramref name="ids"/> is not provided, this parameter returns paginated Custom Reward Redemption objects for redemptions of the Custom Reward with ID <paramref name="rewardId"/></param>
    /// <param name="ids">When used, this param filters the results and only returns Custom Reward Redemption objects for the redemptions with matching ID. Maximum: 50</param>
    /// <param name="status">When <paramref name="ids"/> is not provided, this param is required and filters the paginated Custom Reward Redemption objects for redemptions with the matching status</param>
    /// <param name="sort">Sort order of redemptions returned when getting the paginated Custom Reward Redemption objects for a reward</param>
    /// <param name="first">Number of results to be returned when getting the paginated Custom Reward Redemption objects for a reward. Limit: 50</param>
    /// <param name="after">Cursor for forward pagination. This applies only to queries without <paramref name="ids"/></param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public static async Task<DataPaginationResponse<CustomRewardRedemptionResponseBody[]>?> GetCustomRewardRedemption(this TwitcherAPI api, string broadcasterId, Guid rewardId, IEnumerable<Guid>? ids = null, RedemptionStatus? status = null, SortOrder sort = SortOrder.OLDEST, int first = 20, string? after = null)
    {
        //if (api.Scopes == default || !api.Scopes.Contains(Scopes.ChannelReadRedemptions))
        //    throw new ScopeRequireException(Scopes.ChannelReadRedemptions);

        var request = new RestRequest("helix/channel_points/custom_rewards/redemptions", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("reward_id", rewardId);

        if (ids != null)
            foreach (var id in ids)
                request.AddQueryParameter("id", id);

        if (status != null)
            request.AddQueryParameter("status", (RedemptionStatus)status);

        if (sort != SortOrder.OLDEST)
            request.AddQueryParameter("sort", sort);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<CustomRewardRedemptionResponseBody[]>>(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenException("The Custom Reward was created by a different ClientId or Channel Points are not available for the broadcaster");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("The Custom Reward Redemptions doesn’t exist with the id and BroadcasterId specified");

        return response.Data;
    }

    /// <summary>Updates the status of Custom Reward Redemption objects on a channel that are in the <see cref="RedemptionStatus.UNFULFILLED"/> status.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="rewardId">ID of the Custom Reward the redemptions to be updated are for</param>
    /// <param name="ids">ID of the Custom Reward Redemption to update, must match a Custom Reward Redemption on <paramref name="broadcasterId"/> channel. Maximum: 50</param>
    /// <param name="status">The new status to set redemptions to. Updating to <see cref="RedemptionStatus.CANCELED"/> will refund the user their Channel Points</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public static async Task<bool> UpdateRedemptionStatus(this TwitcherAPI api, string broadcasterId, Guid rewardId, IEnumerable<Guid> ids, RedemptionStatus status)
    {
        if (api.Scopes == default || !api.Scopes.Contains(Scopes.ChannelManageRedemptions))
            throw new ScopeRequireException(Scopes.ChannelManageRedemptions);

        var request = new RestRequest("helix/channel_points/custom_rewards/redemptions", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("reward_id", rewardId)
            .AddBody(new CustomRewardRedemptionStatusRequestBody(status));

        foreach (var id in ids)
            request.AddQueryParameter("id", id);

        var response = await api.APIRequest(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenException("The Custom Reward was created by a different ClientId or Channel Points are not available for the broadcaster");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("The Custom Reward Redemptions doesn’t exist with the id and BroadcasterId specified");

        return response.IsSuccessful;
    }
    #endregion
}
