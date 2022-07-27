namespace Twitcher.API.Requests;

public static class BitsRequests
{
    /// <summary>Gets a ranked list of Bits leaderboard information for an authorized broadcaster.
    /// Required scope: '<inheritdoc cref="Scopes.BitsRead"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">ID of the user whose results are returned</param>
    /// <param name="count">Number of results to be returned. Maximum: 100. Default: 10</param>
    /// <param name="period">Time period over which data is aggregated (PST time zone). This parameter interacts with <paramref name="startedAt" /></param>
    /// <param name="startedAt">Timestamp for the period over which the returned data is aggregated</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<BitsLeaderboardResponse?> GetBitsLeaderboard(this TwitcherAPI api, string? userId = null, int count = 10, LeaderboardTimePeriod period = LeaderboardTimePeriod.All, DateTime startedAt = default)
    {
        var request = new RestRequest("helix/bits/leaderboard", Method.Get);

        if (userId != default)
            request.AddQueryParameter("user_id", userId);

        if (count != 10)
            request.AddQueryParameter("count", count);

        if (period != LeaderboardTimePeriod.All)
            request.AddQueryParameter("period", period);

        if (startedAt != default)
            request.AddQueryParameter("started_at", startedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));

        var response = await api.APIRequest<BitsLeaderboardResponse>(request);

        return response.Data;
    }

    /// <summary>Retrieves the list of available Cheermotes, animated emotes to which viewers can assign Bits, to cheer in chat. Cheermotes returned are available throughout Twitch, in all Bits-enabled channels</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">ID for the broadcaster who might own specialized Cheermotes</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<CheermotesResponse[]?> GetCheermotes(this TwitcherAPI api, string? broadcasterId = null)
    {
        var request = new RestRequest("helix/bits/cheermotes", Method.Get);

        if (broadcasterId != default)
            request.AddQueryParameter("broadcaster_id", broadcasterId);

        var response = await api.APIRequest<DataResponse<CheermotesResponse[]>>(request);

        return response.Data?.Data;
    }
}
