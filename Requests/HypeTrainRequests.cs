namespace Twitcher.API.Requests;

/// <summary>Extension methods with hype train requests</summary>
public static class HypeTrainRequests
{
    /// <summary>Gets the information of the most recent Hype Train of the given <paramref name="broadcasterId"/>. When there is currently an active Hype Train, it returns information about that Hype Train. When there is currently no active Hype Train, it returns information about the most recent Hype Train. After 5 days, if no Hype Train has been active, the endpoint will return an empty response.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadHypeTrain"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID of the broadcaster. Must match the User ID in <paramref name="api"/></param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<HypeTrainResponseBody[]?>> GetHypeTrainEvents(this TwitcherAPI api, string broadcasterId, int first = 1, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/hypetrain/events", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterDefault("first", first, 1)
            .AddQueryParameterDefault("cursor", after);

        var response = await api.APIRequest<DataPaginationResponse<HypeTrainResponseBody[]?>>(request);
        return response.Data!;
    }
}
