namespace Twitcher.API.Requests;

/// <summary>Extension methods with raid requests</summary>
public static class RaidRequests
{
    /// <summary>Raid another channel by sending the broadcaster’s viewers to the targeted channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRaids"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="fromBroadcasterId">The ID of the broadcaster that’s sending the raiding party. Must match the user ID in <paramref name="api"/></param>
    /// <param name="toBroadcasterId">The ID of the broadcaster to raid</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<StartRaidMetadata> StartRaid(this TwitcherAPI api, string fromBroadcasterId, string toBroadcasterId)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(fromBroadcasterId);
        ArgumentNullException.ThrowIfNull(toBroadcasterId);

        var request = new RestRequest("helix/raids", Method.Post)
            .AddQueryParameter("from_broadcaster_id", fromBroadcasterId)
            .AddQueryParameter("to_broadcaster_id", toBroadcasterId);

        var response = await api.APIRequest<DataResponse<StartRaidMetadata[]>>(request);
        return response.Data!.Data.Single();
    }

    /// <summary>Cancel a pending raid.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageRaids"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster that sent the raiding party. Must match the user ID in <paramref name="api"/></param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task CancelRaid(this TwitcherAPI api, string broadcasterId)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);

        var request = new RestRequest("helix/raids", Method.Delete)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        _ = await api.APIRequest(request);
    }
}
