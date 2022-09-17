namespace Twitcher.API.Models;

/// <param name="BroadcasterId">Twitch User ID of this channel owner</param>
/// <param name="BroadcasterLogin">Broadcaster's user login name</param>
/// <param name="BroadcasterName">Twitch user display name of this channel owner</param>
/// <param name="GameName">Name of the game being played on the channel</param>
/// <param name="GameId">Current game ID being played on the channel</param>
/// <param name="BroadcasterLanguage">Language of the channel. A language value is either the ISO 639-1 two-letter code or "other"</param>
/// <param name="Title">Title of the stream</param>
/// <param name="Delay">Stream delay in seconds</param>
public record ChannelMetadata(string BroadcasterId, string BroadcasterLogin, string BroadcasterName, string GameName, string GameId, string BroadcasterLanguage, string Title, int Delay);

/// <param name="GameId">The current game ID being played on the channel. Use "0" or <see cref="string.Empty"/> to unset the game</param>
/// <param name="BroadcasterLanguage">The language of the channel. A language value must be either the ISO 639-1 two-letter code or "other"</param>
/// <param name="Title">The title of the stream. Value must not be an empty</param>
/// <param name="Delay">Stream delay in seconds. Stream delay is a Twitch Partner feature</param>
public record ModifyChannelRequestBody(string? GameId, string? BroadcasterLanguage, string? Title, int? Delay);

/// <param name="UserId">User ID of the editor</param>
/// <param name="UserName">Display name of the editor</param>
/// <param name="CreatedAt">Date and time the editor was given editor permissions</param>
public record ChannelEditorData(string UserId, string UserName, DateTime CreatedAt);
