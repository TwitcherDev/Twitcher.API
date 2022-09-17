namespace Twitcher.API.Models;

/// <param name="Data">A list of emotes</param>
/// <param name="Template">A templated URL. Use the values from id, format, scale, and theme_mode to replace the like-named placeholder strings in the templated URL to create a CDN (content delivery network) URL that you use to fetch the emote</param>
public record GetEmotesResponse<T>(T Data, string Template) : DataResponse<T>(Data);

/// <param name="Url1x">A URL to the small version (28px x 28px) of the emote</param>
/// <param name="Url2x">A URL to the medium version (56px x 56px) of the emote</param>
/// <param name="Url4x">A URL to the large version (112px x 112px) of the emote</param>
public record ChannelImages(string Url1x, string Url2x, string Url4x);

/// <param name="Id">An ID that identifies the emote</param>
/// <param name="Name">The name of the emote. This is the name that viewers type in the chat window to get the emote to appear</param>
/// <param name="Images">Contains the image URLs for the emote. These image URLs will always provide a static (i.e., non-animated) emote image with a light background. NOTE: The preference is for you to use the templated URL in the template field to fetch the image instead of using these URLs</param>
/// <param name="Format">The formats that the emote is available in. The possible formats are: 'animated', 'static'</param>
/// <param name="Scale">The sizes that the emote is available in. Possible sizes are: '1.0', '2.0', '3.0'</param>
/// <param name="ThemeMode">The background themes that the emote is available in. Possible themes are: 'light', 'dark'</param>
public record EmoteData(string Id, string Name, ChannelImages Images, string[] Format, string[] Scale, string[] ThemeMode);

/// <param name="Id">An ID that identifies the emote</param>
/// <param name="Name">The name of the emote. This is the name that viewers type in the chat window to get the emote to appear</param>
/// <param name="Images">Contains the image URLs for the emote. These image URLs will always provide a static (i.e., non-animated) emote image with a light background. NOTE: The preference is for you to use the templated URL in the template field to fetch the image instead of using these URLs</param>
/// <param name="Tier">The subscriber tier at which the emote is unlocked. This field contains the tier information only if <paramref name="EmoteType"/> is set to <see cref="EmoteType.Subscriptions"/>, otherwise, it's an <see cref="string.Empty"/></param>
/// <param name="EmoteType">The type of emote</param>
/// <param name="EmoteSetId">An ID that identifies the emote set that the emote belongs to</param>
/// <param name="Format">The formats that the emote is available in. The possible formats are: 'animated', 'static'</param>
/// <param name="Scale">The sizes that the emote is available in. Possible sizes are: '1.0', '2.0', '3.0'</param>
/// <param name="ThemeMode">The background themes that the emote is available in. Possible themes are: 'light', 'dark'</param>
public record ChannelEmoteData(string Id, string Name, ChannelImages Images, string Tier, EmoteType EmoteType, string EmoteSetId, string[] Format, string[] Scale, string[] ThemeMode) : EmoteData(Id, Name, Images, Format, Scale, ThemeMode);

/// <param name="Id">An ID that identifies the emote</param>
/// <param name="Name">The name of the emote. This is the name that viewers type in the chat window to get the emote to appear</param>
/// <param name="Images">Contains the image URLs for the emote. These image URLs will always provide a static (i.e., non-animated) emote image with a light background. NOTE: The preference is for you to use the templated URL in the template field to fetch the image instead of using these URLs</param>
/// <param name="OwnerId">The ID of the broadcaster who owns the emote</param>
/// <param name="EmoteType">The type of emote</param>
/// <param name="EmoteSetId">An ID that identifies the emote set that the emote belongs to</param>
/// <param name="Format">The formats that the emote is available in. The possible formats are: 'animated', 'static'</param>
/// <param name="Scale">The sizes that the emote is available in. Possible sizes are: '1.0', '2.0', '3.0'</param>
/// <param name="ThemeMode">The background themes that the emote is available in. Possible themes are: 'light', 'dark'</param>
public record SetEmoteData(string Id, string Name, ChannelImages Images, EmoteType EmoteType, string EmoteSetId, string OwnerId, string[] Format, string[] Scale, string[] ThemeMode) : EmoteData(Id, Name, Images, Format, Scale, ThemeMode);

/// <param name="SetId">ID for the chat badge set</param>
/// <param name="Versions">Contains chat badge objects for the set</param>
public record ChatBadgeData(string SetId, ChatBadgeVersion[] Versions);

/// <param name="Id">ID of the chat badge version</param>
/// <param name="ImageUrl1x">Small image URL</param>
/// <param name="ImageUrl2x">Medium image URL</param>
/// <param name="ImageUrl4x">Large image URL</param>
public record ChatBadgeVersion(string Id, string ImageUrl1x, string ImageUrl2x, string ImageUrl4x);

/// <param name="BroadcasterId">The ID of the broadcaster specified in the request</param>
/// <param name="EmoteMode">A Boolean value that determines whether chat messages must contain only emotes</param>
/// <param name="FollowerMode">A Boolean value that determines whether the broadcaster restricts the chat room to followers only, based on how long they've followed</param>
/// <param name="FollowerModeDuration">The length of time, in minutes, that the followers must have followed the broadcaster to participate in the chat room. Is <see langword="null"/> if <paramref name="FollowerMode"/> is <see langword="false"/></param>
/// <param name="ModeratorId">The moderator's ID. The response includes this field only if the request specifies a User access token that includes the moderator:read:chat_settings scope</param>
/// <param name="NonModeratorChatDelay">A Boolean value that determines whether the broadcaster adds a short delay before chat messages appear in the chat room. This gives chat moderators and bots a chance to remove them before viewers can see the message</param>
/// <param name="NonModeratorChatDelayDuration">The amount of time, in seconds, that messages are delayed from appearing in chat. Is <see langword="null"/> if <paramref name="NonModeratorChatDelay"/> is <see langword="false"/></param>
/// <param name="SlowMode">A Boolean value that determines whether the broadcaster limits how often users in the chat room are allowed to send messages</param>
/// <param name="SlowModeWaitTime">The amount of time, in seconds, that users need to wait between sending messages. Is <see langword="null"/> if <paramref name="SlowMode"/> is <see langword="false"/></param>
/// <param name="SubscriberMode">A Boolean value that determines whether only users that subscribe to the broadcaster's channel can talk in the chat room</param>
/// <param name="UniqueChatMode">A Boolean value that determines whether the broadcaster requires users to post only unique messages in the chat room</param>
public record ChatSettingsData(string BroadcasterId, bool EmoteMode, bool FollowerMode, int? FollowerModeDuration, string? ModeratorId, bool? NonModeratorChatDelay, int? NonModeratorChatDelayDuration, bool SlowMode, int? SlowModeWaitTime, bool SubscriberMode, bool UniqueChatMode);

/// <param name="EmoteMode">A Boolean value that determines whether chat messages must contain only emotes</param>
/// <param name="FollowerMode">A Boolean value that determines whether the broadcaster restricts the chat room to followers only, based on how long they've followed</param>
/// <param name="FollowerModeDuration">The length of time, in minutes, that the followers must have followed the broadcaster to participate in the chat room. You may specify a value in the range: 0 (no restriction) through 129600 (3 months)</param>
/// <param name="NonModeratorChatDelay">A Boolean value that determines whether the broadcaster adds a short delay before chat messages appear in the chat room. This gives chat moderators and bots a chance to remove them before viewers can see the message</param>
/// <param name="NonModeratorChatDelayDuration">The amount of time, in seconds, that messages are delayed from appearing in chat. Possible values are: 2, 4, 6</param>
/// <param name="SlowMode">A Boolean value that determines whether the broadcaster limits how often users in the chat room are allowed to send messages. You may specify a value in the range: 3 (3 second delay) through 120 (2 minute delay)</param>
/// <param name="SlowModeWaitTime">The amount of time, in seconds, that users need to wait between sending messages</param>
/// <param name="SubscriberMode">A Boolean value that determines whether only users that subscribe to the broadcaster's channel can talk in the chat room</param>
/// <param name="UniqueChatMode">A Boolean value that determines whether the broadcaster requires users to post only unique messages in the chat room</param>
public record UpdateChatSettingsRequestBody(bool? EmoteMode, bool? FollowerMode, int? FollowerModeDuration, bool? NonModeratorChatDelay, int? NonModeratorChatDelayDuration, bool? SlowMode, int? SlowModeWaitTime, bool? SubscriberMode, bool? UniqueChatMode);

/// <param name="Message">The announcement to make in the broadcaster's chat room. Announcements are limited to a maximum of 500 characters; announcements longer than 500 characters are truncated</param>
/// <param name="Color">The color used to highlight the announcement. Possible case-sensitive values are: 'blue', 'green', 'orange', 'purple', 'primary' (default)</param>
public record SendChatAnnouncementRequestBody(string Message, string? Color);

/// <param name="UserId">The ID of the user</param>
/// <param name="UserLogin">The user's login name</param>
/// <param name="UserName">The user's display name</param>
/// <param name="Color">The Hex color code that the user uses in chat for their name. If the user hasn't specified a color in their settings, the string is <see cref="string.Empty"/></param>
public record UserColorData(string UserId, string UserLogin, string UserName, string Color);
