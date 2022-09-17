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
    public static async Task<GoalResponseBody[]> GetCreatorGoals(this TwitcherAPI api, string broadcasterId) =>
        (await api.APIRequest<DataResponse<GoalResponseBody[]?>>("helix/goals", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)))
            .Data ?? Array.Empty<GoalResponseBody>();
}
