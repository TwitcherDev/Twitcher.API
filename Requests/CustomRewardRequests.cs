namespace Twitcher.API.Requests;

public static class CustomRewardRequests
{
    #region Rewards
    /// <summary>Creates a Custom Reward on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="title">The title of the reward</param>
    /// <param name="cost">The cost of the reward</param>
    /// <param name="prompt">The prompt for the viewer when redeeming the reward</param>
    /// <param name="isEnable">Is the reward currently enabled, if false the reward won’t show up to viewers</param>
    /// <param name="backgroundColor">Custom background color for the reward. Format: Hex with # prefix</param>
    /// <param name="isUserInputRequired">Does the user need to enter information when redeeming the reward</param>
    /// <param name="maxPerStream">The maximum number per stream. If <see langword="null"/>, then off</param>
    /// <param name="maxPerUserPerStream">The maximum number per user per stream. If <see langword="null"/>, then off</param>
    /// <param name="globalCooldownSeconds">The cooldown in seconds. If <see langword="null"/>, then off</param>
    /// <param name="shouldRedemptionsSkipRequestQueue">Should redemptions be set to <see cref="RedemptionStatus.FULFILLED" /> status immediately when redeemed and skip the request queue instead of the normal <see cref="RedemptionStatus.Unfulfilled" /> status</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<CustomRewardResponseBody> CreateCustomReward(this TwitcherAPI api, string broadcasterId, string title, int cost, string? prompt = null, bool isEnable = true, string? backgroundColor = null, bool isUserInputRequired = false, int? maxPerStream = null, int? maxPerUserPerStream = null, int? globalCooldownSeconds = null, bool shouldRedemptionsSkipRequestQueue = false)
    {
        var requestBody = new CreateCustomRewardRequestBody(title, cost, prompt,
            isEnable != true ? isEnable : null,
            backgroundColor,
            isUserInputRequired != false ? isUserInputRequired : null,
            maxPerStream != null,
            maxPerStream,
            maxPerUserPerStream != null,
            maxPerUserPerStream,
            globalCooldownSeconds != null,
            globalCooldownSeconds,
            shouldRedemptionsSkipRequestQueue != false ? shouldRedemptionsSkipRequestQueue : null);

        return CreateCustomReward(api, broadcasterId, requestBody);
    }

    /// <summary>Creates a Custom Reward on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="body">Request body. <see cref="CreateCustomRewardRequestBody.Title"/> and <see cref="CreateCustomRewardRequestBody.Cost"/> are required</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<CustomRewardResponseBody> CreateCustomReward(this TwitcherAPI api, string broadcasterId, CreateCustomRewardRequestBody body)
    {
        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Post)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddBody(body);

        var response = await api.APIRequest<DataResponse<CustomRewardResponseBody[]>>(request);
        return response.Data!.Data!.FirstOrDefault()!;
    }

    /// <summary>Deletes a Custom Reward on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="id">ID of the Custom Reward to delete, must match a Custom Reward on <paramref name="broadcasterId" /> channel</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task DeleteCustomReward(this TwitcherAPI api, string broadcasterId, Guid id)
    {
        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Delete)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("id", id);

        _ = await api.APIRequest(request);
    }

    /// <summary>Returns a list of Custom Reward objects for the Custom Rewards on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadRedemptions"/>' or '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="ids">When used, this parameter filters the results and only returns reward objects for the Custom Rewards with matching ID. Maximum: 50</param>
    /// <param name="onlyManageableRewards">When set to true, only returns custom rewards that the calling client_id can manage</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<CustomRewardResponseBody[]> GetCustomReward(this TwitcherAPI api, string broadcasterId, IEnumerable<Guid>? ids = null, bool onlyManageableRewards = false)
    {
        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        if (ids != null)
            foreach (var id in ids)
                request.AddQueryParameter("id", id);

        if (onlyManageableRewards)
            request.AddQueryParameter("only_manageable_rewards", onlyManageableRewards);

        var response = await api.APIRequest<DataResponse<CustomRewardResponseBody[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Updates a Custom Reward created on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="id">ID of the Custom Reward to update. Must match a Custom Reward on the channel of the <paramref name="broadcasterId"/></param>
    /// <param name="title">The title of the reward</param>
    /// <param name="cost">The cost of the reward</param>
    /// <param name="prompt">The prompt for the viewer when redeeming the reward</param>
    /// <param name="isEnable">Is the reward currently enabled, if false the reward won’t show up to viewers</param>
    /// <param name="backgroundColor">Custom background color for the reward. Format: Hex with # prefix</param>
    /// <param name="isUserInputRequired">Does the user need to enter information when redeeming the reward</param>
    /// <param name="isMaxPerStreamEnabled">Whether a maximum per stream is enabled</param>
    /// <param name="maxPerStream">The maximum number per stream</param>
    /// <param name="isMaxPerUserPerStreamEnabled">Whether a maximum per user per stream is enabled</param>
    /// <param name="maxPerUserPerStream">The maximum number per user per stream</param>
    /// <param name="isGlobalCooldownEnabled">Whether a cooldown is enabled</param>
    /// <param name="globalCooldownSeconds">The cooldown in seconds</param>
    /// <param name="shouldRedemptionsSkipRequestQueue">Should redemptions be set to <see cref="RedemptionStatus.FULFILLED" /> status immediately when redeemed and skip the request queue instead of the normal <see cref="RedemptionStatus.Unfulfilled" /> status</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<CustomRewardResponseBody> UpdateCustomReward(this TwitcherAPI api, string broadcasterId, Guid id, string? title = null, int? cost = null, string? prompt = null, bool? isEnable = null, string? backgroundColor = null, bool? isUserInputRequired = null, bool? isMaxPerStreamEnabled = null, int? maxPerStream = null, bool? isMaxPerUserPerStreamEnabled = null, int? maxPerUserPerStream = null, bool? isGlobalCooldownEnabled = null, int? globalCooldownSeconds = null, bool? isPaused = null, bool? shouldRedemptionsSkipRequestQueue = null)
    {
        var requestBody = new UpdateCustomRewardRequestBody(title, cost, prompt,
            isEnable,
            backgroundColor,
            isUserInputRequired,
            isMaxPerStreamEnabled,
            maxPerStream,
            isMaxPerUserPerStreamEnabled,
            maxPerUserPerStream,
            isGlobalCooldownEnabled,
            globalCooldownSeconds,
            isPaused,
            shouldRedemptionsSkipRequestQueue);

        return UpdateCustomReward(api, broadcasterId, id, requestBody);
    }

    /// <summary>Updates a Custom Reward created on a channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="id">ID of the Custom Reward to update. Must match a Custom Reward on the channel of the <paramref name="broadcasterId"/></param>
    /// <param name="body">Request body</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<CustomRewardResponseBody> UpdateCustomReward(this TwitcherAPI api, string broadcasterId, Guid id, UpdateCustomRewardRequestBody body)
    {
        var request = new RestRequest("helix/channel_points/custom_rewards", Method.Patch)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("id", id)
            .AddBody(body);

        var response = await api.APIRequest<DataResponse<CustomRewardResponseBody[]>>(request);
        return response.Data!.Data!.FirstOrDefault()!;
    }
    #endregion

    #region Redemptions
    /// <summary>Returns Custom Reward Redemption objects for a Custom Reward on a channel that was created by the same client_id.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadRedemptions"/>' or '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="rewardId">When <paramref name="ids"/> is not provided, this parameter returns paginated Custom Reward Redemption objects for redemptions of the Custom Reward with ID <paramref name="rewardId"/></param>
    /// <param name="ids">When used, this param filters the results and only returns Custom Reward Redemption objects for the redemptions with matching ID. Maximum: 50</param>
    /// <param name="status">When <paramref name="ids"/> is not provided, this param is required and filters the paginated Custom Reward Redemption objects for redemptions with the matching status</param>
    /// <param name="sort">Sort order of redemptions returned when getting the paginated Custom Reward Redemption objects for a reward</param>
    /// <param name="first">Number of results to be returned when getting the paginated Custom Reward Redemption objects for a reward. Limit: 50</param>
    /// <param name="after">Cursor for forward pagination. This applies only to queries without <paramref name="ids"/></param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<CustomRewardRedemptionResponseBody[]>> GetCustomRewardRedemption(this TwitcherAPI api, string broadcasterId, Guid rewardId, IEnumerable<Guid>? ids = null, RedemptionStatus? status = null, SortOrder sort = SortOrder.Oldest, int first = 20, string? after = null)
    {
        var request = new RestRequest("helix/channel_points/custom_rewards/redemptions", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("reward_id", rewardId);

        if (ids != null)
            foreach (var id in ids)
                request.AddQueryParameter("id", id);

        if (status.HasValue)
            request.AddQueryParameter("status", status.Value.ToString().ToUpper());

        if (sort != SortOrder.Oldest)
            request.AddQueryParameter("sort", sort.ToString().ToUpper());

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<CustomRewardRedemptionResponseBody[]>>(request);
        return response.Data!;
    }

    /// <summary>Updates the status of Custom Reward Redemption objects on a channel that are in the <see cref="RedemptionStatus.Unfulfilled"/> status.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRedemptions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Provided <paramref name="broadcasterId" /> must match the userId in the user OAuth token</param>
    /// <param name="rewardId">ID of the Custom Reward the redemptions to be updated are for</param>
    /// <param name="ids">ID of the Custom Reward Redemption to update, must match a Custom Reward Redemption on <paramref name="broadcasterId"/> channel. Maximum: 50</param>
    /// <param name="status">The new status to set redemptions to. Updating to <see cref="RedemptionStatus.Canceled"/> will refund the user their Channel Points</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task UpdateRedemptionStatus(this TwitcherAPI api, string broadcasterId, Guid rewardId, IEnumerable<Guid> ids, RedemptionStatus status)
    {
        var request = new RestRequest("helix/channel_points/custom_rewards/redemptions", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddQueryParameter("reward_id", rewardId)
            .AddBody(new CustomRewardRedemptionStatusRequestBody(status));

        foreach (var id in ids)
            request.AddQueryParameter("id", id);

        _ = await api.APIRequest(request);
    }
    #endregion
}
