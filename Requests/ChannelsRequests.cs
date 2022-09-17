namespace Twitcher.API.Requests;

/// <summary>Extension methods with channels requests</summary>
public static class ChannelsRequests
{
    /// <summary>Gets channel information for users</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster whose channel you want to get</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChannelMetadata> GetChannelInformation(this TwitcherAPI api, string broadcasterId) =>
        (await api.APIRequest<DataResponse<ChannelMetadata[]>>("helix/channels", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)))
            .Data.Single();

    /// <summary>Gets channels information for users</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterIds">The IDs of the broadcasters whose channels you want to get. Maximum: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChannelMetadata[]> GetChannelInformation(this TwitcherAPI api, IEnumerable<string> broadcasterIds) =>
        (await api.APIRequest<DataResponse<ChannelMetadata[]>>("helix/channels", RequestMethod.Get, r => r
            .AddQueryParametersNotEmpty("broadcaster_id", broadcasterIds)))
            .Data ?? Array.Empty<ChannelMetadata>();

    /// <summary>Gets channels information for users.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">ID of the channel to be updated</param>
    /// <param name="gameId">The current game ID being played on the channel. Use "0" or <see cref="string.Empty"/> to unset the game</param>
    /// <param name="broadcasterLanguage">The language of the channel. A language value must be either the ISO 639-1 two-letter code or "other"</param>
    /// <param name="title">The title of the stream. Value must not be an empty</param>
    /// <param name="delay">Stream delay in seconds. Stream delay is a Twitch Partner feature</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task ModifyChannelInformation(this TwitcherAPI api, string broadcasterId, string? gameId = null, string? broadcasterLanguage = null, string? title = null, int? delay = null) =>
        ModifyChannelInformation(api, broadcasterId, new ModifyChannelRequestBody(gameId, broadcasterLanguage, title, delay));

    /// <summary>Gets channels information for users.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">ID of the channel to be updated</param>
    /// <param name="body">Request body</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task ModifyChannelInformation(this TwitcherAPI api, string broadcasterId, ModifyChannelRequestBody body) =>
        await api.APIRequest("helix/channels", RequestMethod.Patch, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddBodyNotNull(body));

    /// <summary>Gets a list of users who have editor permissions for a specific channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadEditors"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">Broadcaster’s user ID associated with the channel</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChannelEditorData[]> GetChannelEditors(this TwitcherAPI api, string broadcasterId) =>
        (await api.APIRequest<DataResponse<ChannelEditorData[]>>("helix/channels/editors", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId))).Data;
}
