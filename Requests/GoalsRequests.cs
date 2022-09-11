namespace Twitcher.API.Requests;

/// <summary>Extension methods with goals requests</summary>
public static class GoalsRequests
{
    /// <summary>Gets the broadcaster’s list of active goals. Use this to get the current progress of each goal.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadGoals"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster that created the goals. Must match the User ID in <paramref name="api"/></param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<HypeTrainResponseBody[]?>> GetCreatorGoals(this TwitcherAPI api, string broadcasterId)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/goals", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId);

        var response = await api.APIRequest<DataPaginationResponse<HypeTrainResponseBody[]?>>(request);
        //return response.Data!;
        throw new NotImplementedException();
    }
}
