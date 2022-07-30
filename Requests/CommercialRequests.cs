namespace Twitcher.API.Requests;

public static class CommercialRequests
{
    /// <summary>Starts a commercial on a specified channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelEditCommercial"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">ID of the channel requesting a commercial</param>
    /// <param name="length">Desired length of the commercial in seconds. Valid options are 30, 60, 90, 120, 150, 180</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<StartCommercialResponseBody> StartCommercial(this TwitcherAPI api, string broadcasterId, int length) =>
        StartCommercial(api, new StartCommercialRequestBody(broadcasterId, length));

    /// <summary>Starts a commercial on a specified channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelEditCommercial"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="body">Request body</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<StartCommercialResponseBody> StartCommercial(this TwitcherAPI api, StartCommercialRequestBody body)
    {
        var request = new RestRequest("helix/channels/commercial", Method.Post)
            .AddBody(body);

        var response = await api.APIRequest<DataResponse<StartCommercialResponseBody[]>>(request);

        return response.Data!.Data!.FirstOrDefault()!;
    }
}
