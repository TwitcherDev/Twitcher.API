namespace Twitcher.API.Requests;

/// <summary>Extension methods with chat requests</summary>
public static class ChatRequests
{
    /// <summary>Gets all emotes that the specified Twitch channel created</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">An ID that identifies the broadcaster to get the emotes from</param>
    /// <returns>A list of emotes that the specified channel created</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<GetEmotesResponse<ChannelEmoteData[]>> GetChannelEmotes(this TwitcherAPI api, string broadcasterId) =>
        api.APIRequest<GetEmotesResponse<ChannelEmoteData[]>>("helix/chat/emotes", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId));

    /// <summary>Gets all global emotes. Global emotes are Twitch-created emoticons that users can use in any Twitch chat</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <returns>The list of global emotes</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<GetEmotesResponse<EmoteData[]>> GetGlobalEmotes(this TwitcherAPI api) =>
        api.APIRequest<GetEmotesResponse<EmoteData[]>>("helix/chat/emotes/global", RequestMethod.Get);

    /// <summary>Gets emotes for specified emote set</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="emoteSetId">An ID that identifies the emote set</param>
    /// <returns>The list of emotes found in the specified emote set</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<GetEmotesResponse<SetEmoteData[]>> GetEmoteSet(this TwitcherAPI api, string emoteSetId) =>
        api.APIRequest<GetEmotesResponse<SetEmoteData[]>>("helix/chat/emotes/set", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("emote_set_id", emoteSetId));

    /// <summary>Gets emotes for specified emote sets</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="emoteSetIds">An IDs that identifies the emote sets. Maximum: 25</param>
    /// <returns>The list of emotes found in the specified emote sets</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<GetEmotesResponse<SetEmoteData[]>> GetEmoteSet(this TwitcherAPI api, IEnumerable<string> emoteSetIds) =>
        api.APIRequest<GetEmotesResponse<SetEmoteData[]>>("helix/chat/emotes/set", RequestMethod.Get, r => r
            .AddQueryParametersNotEmpty("emote_set_id", emoteSetIds));

    /// <summary>Gets a list of custom chat badges that can be used in chat for the specified channel</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster whose chat badges are being requested. Must match the User ID in <paramref name="api"/></param>
    /// <returns>An array of chat badge sets</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChatBadgeData[]> GetChannelChatBadges(this TwitcherAPI api, string broadcasterId) =>
        (await api.APIRequest<DataResponse<ChatBadgeData[]>>("helix/chat/badges", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)))
            .Data;

    /// <summary>Gets a list of chat badges that can be used in chat for any channel</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <returns>An array of chat badge sets</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChatBadgeData[]> GetGlobalChatBadges(this TwitcherAPI api) =>
        (await api.APIRequest<DataResponse<ChatBadgeData[]>>("helix/chat/badges/global", RequestMethod.Get))
            .Data;

    /// <summary>Gets the broadcaster’s chat settings</summary>
    /// <remarks>To include the NonModeratorChatDelay or NonModeratorChatDelayDuration settings in the response, you must specify a User access token with scope <see cref="Scopes.ModeratorReadChatSettings"/></remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster whose chat settings you want to get</param>
    /// <param name="moderatorId">Required only to access the NonModeratorChatDelay or NonModeratorChatDelayDuration settings. Must match the User ID in <paramref name="api"/></param>
    /// <returns>Chat settings</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChatSettingsData> GetChatSettings(this TwitcherAPI api, string broadcasterId, string? moderatorId = null) =>
        (await api.APIRequest<DataResponse<ChatSettingsData[]>>("helix/chat/settings", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterOrDefault("moderator_id", moderatorId)))
            .Data.Single();

    /// <summary>Updates the broadcaster’s chat settings</summary>
    /// <remarks>Required scope: <see cref="Scopes.ModeratorManageChatSettings"/></remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster whose chat settings you want to update</param>
    /// <param name="moderatorId">The ID of a user that has permission to moderate the broadcaster’s chat room. Must match the User ID in <paramref name="api"/>. If the broadcaster wants to update their own settings (instead of having the moderator do it), set this parameter to the broadcaster’s ID, too</param>
    /// <param name="emoteMode">A Boolean value that determines whether chat messages must contain only emotes</param>
    /// <param name="followerMode">A Boolean value that determines whether the broadcaster restricts the chat room to followers only, based on how long they've followed</param>
    /// <param name="followerModeDuration">The length of time, in minutes, that the followers must have followed the broadcaster to participate in the chat room. You may specify a value in the range: 0 (no restriction) through 129600 (3 months)</param>
    /// <param name="nonModeratorChatDelay">A Boolean value that determines whether the broadcaster adds a short delay before chat messages appear in the chat room. This gives chat moderators and bots a chance to remove them before viewers can see the message</param>
    /// <param name="nonModeratorChatDelayDuration">The amount of time, in seconds, that messages are delayed from appearing in chat. Possible values are: 2, 4, 6</param>
    /// <param name="slowMode">A Boolean value that determines whether the broadcaster limits how often users in the chat room are allowed to send messages. You may specify a value in the range: 3 (3 second delay) through 120 (2 minute delay)</param>
    /// <param name="slowModeWaitTime">The amount of time, in seconds, that users need to wait between sending messages</param>
    /// <param name="subscriberMode">A Boolean value that determines whether only users that subscribe to the broadcaster's channel can talk in the chat room</param>
    /// <param name="uniqueChatMode">A Boolean value that determines whether the broadcaster requires users to post only unique messages in the chat room</param>
    /// <returns>Chat settings</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<ChatSettingsData> UpdateChatSettings(this TwitcherAPI api, string broadcasterId, string moderatorId, bool? emoteMode = null, bool? followerMode = null, int? followerModeDuration = null, bool? nonModeratorChatDelay = null, int? nonModeratorChatDelayDuration = null, bool? slowMode = null, int? slowModeWaitTime = null, bool? subscriberMode = null, bool? uniqueChatMode = null) =>
        UpdateChatSettings(api, broadcasterId, moderatorId, new UpdateChatSettingsRequestBody(emoteMode, followerMode, followerModeDuration, nonModeratorChatDelay, nonModeratorChatDelayDuration, slowMode, slowModeWaitTime, subscriberMode, uniqueChatMode));

    /// <summary>Updates the broadcaster’s chat settings</summary>
    /// <remarks>Required scope: <see cref="Scopes.ModeratorManageChatSettings"/></remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster whose chat settings you want to update</param>
    /// <param name="moderatorId">The ID of a user that has permission to moderate the broadcaster’s chat room. Must match the User ID in <paramref name="api"/>. If the broadcaster wants to update their own settings (instead of having the moderator do it), set this parameter to the broadcaster’s ID, too</param>
    /// <param name="body">Request body</param>
    /// <returns>Chat settings</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<ChatSettingsData> UpdateChatSettings(this TwitcherAPI api, string broadcasterId, string moderatorId, UpdateChatSettingsRequestBody body) =>
        (await api.APIRequest<DataResponse<ChatSettingsData[]>>("helix/chat/settings", RequestMethod.Patch, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterNotNull("moderator_id", moderatorId)
            .AddBodyNotNull(body)))
            .Data.Single();

    /// <summary>Sends an announcement to the broadcaster’s chat room</summary>
    /// <remarks>Required scope: <see cref="Scopes.ModeratorManageAnnouncements"/></remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster that owns the chat room to send the announcement to</param>
    /// <param name="moderatorId">The ID of a user who has permission to moderate the broadcaster’s chat room. Must match the User ID in <paramref name="api"/>. If the broadcaster wants to update their own settings (instead of having the moderator do it), set this parameter to the broadcaster’s ID, too</param>
    /// <param name="message">The announcement to make in the broadcaster's chat room. Announcements are limited to a maximum of 500 characters; announcements longer than 500 characters are truncated</param>
    /// <param name="color">The color used to highlight the announcement. Default: <see cref="AnnouncementColor.Primary"/></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task SendChatAnnouncement(this TwitcherAPI api, string broadcasterId, string moderatorId, string message, AnnouncementColor? color = null) =>
        SendChatAnnouncement(api, broadcasterId, moderatorId, message, color?.ToString().ToLower());

    /// <summary>Sends an announcement to the broadcaster’s chat room</summary>
    /// <remarks>Required scope: <see cref="Scopes.ModeratorManageAnnouncements"/></remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster that owns the chat room to send the announcement to</param>
    /// <param name="moderatorId">The ID of a user who has permission to moderate the broadcaster’s chat room. Must match the User ID in <paramref name="api"/>. If the broadcaster wants to update their own settings (instead of having the moderator do it), set this parameter to the broadcaster’s ID, too</param>
    /// <param name="message">The announcement to make in the broadcaster's chat room. Announcements are limited to a maximum of 500 characters; announcements longer than 500 characters are truncated</param>
    /// <param name="color">The color used to highlight the announcement. Possible case-sensitive values are: 'blue', 'green', 'orange', 'purple', 'primary' (default)</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task SendChatAnnouncement(this TwitcherAPI api, string broadcasterId, string moderatorId, string message, string? color = null) =>
        api.APIRequest("helix/chat/announcements", RequestMethod.Post, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterNotNull("moderator_id", moderatorId)
            .AddBodyNotNull(new SendChatAnnouncementRequestBody(message, color)));

    /// <summary>Gets the color used for the user’s name in chat</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">The ID of the user whose color you want to get</param>
    /// <returns>User and the color code used for his name</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserColorData?> GetUserChatColor(this TwitcherAPI api, string userId) =>
        (await api.APIRequest<DataResponse<UserColorData[]?>>("helix/chat/color", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("user_id", userId)))
            .Data?.SingleOrDefault();

    /// <summary>Gets the color used for the user’s name in chat</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userIds">The IDs of the users whose colors you want to get. Maximum: 100</param>
    /// <returns>The list of users and the color code that’s used for their name</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserColorData[]> GetUserChatColor(this TwitcherAPI api, IEnumerable<string> userIds) =>
        (await api.APIRequest<DataResponse<UserColorData[]?>>("helix/chat/color", RequestMethod.Get, r => r
            .AddQueryParametersNotEmpty("user_id", userIds)))
            .Data ?? Array.Empty<UserColorData>();

    /// <summary>Updates the color used for the user’s name in chat</summary>
    /// <remarks>Required scope: <see cref="Scopes.UserManageChatColor"/></remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">The ID of the user whose chat color you want to update. Must match the User ID in <paramref name="api"/></param>
    /// <param name="color">The color to use for the user’s name in chat</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task UpdateUserChatColor(this TwitcherAPI api, string userId, string color) =>
        api.APIRequest("helix/chat/color", RequestMethod.Put, r => r
            .AddQueryParameterNotNull("user_id", userId)
            .AddQueryParameterNotNull("color", color));
}
