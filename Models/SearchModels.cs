namespace Twitcher.API.Models;

/// <param name="BoxArtUrl">Template URL for the game's box art</param>
/// <param name="Name">Game/category name</param>
/// <param name="Id">Game/category ID</param>
public record SearchCategoryMetadata(string BoxArtUrl, string Name, string Id);

/// <param name="BroadcasterLanguage">Channel language. A language value is either the ISO 639-1 two-letter code or "other"</param>
/// <param name="BroadcasterLogin">Login of the broadcaster</param>
/// <param name="DisplayName">Display name of the broadcaster</param>
/// <param name="GameId">ID of the game being played on the stream</param>
/// <param name="GameName">Name of the game being played on the stream</param>
/// <param name="Id">Channel ID</param>
/// <param name="IsLive">Indicates if the channel is currenty live</param>
/// <param name="TagIds">Tag IDs that apply to the stream. This array only contains strings when a channel is live. For all possibilities, see List of All Tags. Category Tags are not returned</param>
/// <param name="ThumbnailUrl">Thumbnail URL of the stream. All image URLs have variable width and height. You can replace {width} and {height} with any values to get that size image</param>
/// <param name="Title">Stream title</param>
/// <param name="StartedAt">UTC timestamp. Returns an empty string if the channel is not live</param>
public record SearchChannelMetadata(string BroadcasterLanguage, string BroadcasterLogin, string DisplayName, string GameId, string GameName, string Id, bool IsLive, string[] TagIds, string ThumbnailUrl, string Title, DateTime StartedAt);