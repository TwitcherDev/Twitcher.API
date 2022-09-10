namespace Twitcher.API.Requests;

/// <summary>A class with extension methods for requesting teams</summary>
public static class TeamRequests
{
    /// <summary>Gets information for a specific Twitch Team. <paramref name="id"/> or <paramref name="name"/> must be specified</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">Team ID</param>
    /// <param name="name">Team name</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<TeamResponseBody> GetTeams(this TwitcherAPI api, string? id = null, string? name = null)
    {
        var request = new RestRequest("helix/teams", Method.Get);

        if (id != null)
            request.AddQueryParameter("id", id);

        if (name != null)
            request.AddQueryParameter("name", name);

        var response = await api.APIRequest<DataResponse<TeamResponseBody[]>>(request);
        return response.Data!.Data.Single();
    }

    /// <summary>Retrieves a list of Twitch Teams of which the specified channel/broadcaster is a member</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID for a Twitch user</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChannelTeamResponseBody[]> GetChannelTeams(this TwitcherAPI api, string broadcasterId)
    {
        var request = new RestRequest("helix/teams/channel", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        var response = await api.APIRequest<DataResponse<ChannelTeamResponseBody[]>>(request);
        return response.Data!.Data;
    }
}
