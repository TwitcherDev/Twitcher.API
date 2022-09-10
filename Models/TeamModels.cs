namespace Twitcher.API.Models;

/// <param name="Id">Team ID</param>
/// <param name="TeamName">Team name</param>
/// <param name="TeamDisplayName">Team display name</param>
/// <param name="Info">Team description</param>
/// <param name="Users">Users in the specified Team</param>
/// <param name="CreatedAt">Date and time the Team was created</param>
/// <param name="UpdatedAt">Date and time the Team was last updated</param>
/// <param name="BackgroundImageUrl">URL of the Team background image</param>
/// <param name="Banner">URL for the Team banner</param>
/// <param name="ThumbnailUrl">Image URL for the Team logo</param>
public record TeamResponseBody(string Id, string TeamName, string TeamDisplayName, string Info, TeamUser[] Users, DateTime CreatedAt, DateTime UpdatedAt, string? BackgroundImageUrl, string? Banner, string? ThumbnailUrl);

/// <param name="UserId">User ID of a Team member</param>
/// <param name="UserLogin">Login of a Team member</param>
/// <param name="UserName">Display name of a Team member</param>
public record TeamUser(string UserId, string UserLogin, string UserName);

/// <param name="BroadcasterId">User ID of the broadcaster</param>
/// <param name="BroadcasterLogin">Login of the broadcaster</param>
/// <param name="BroadcasterName">Display name of the broadcaster</param>
/// <param name="BackgroundImageUrl">URL for the Team background image</param>
/// <param name="Banner">URL for the Team banner</param>
/// <param name="CreatedAt">Date and time the Team was created</param>
/// <param name="UpdatedAt">Date and time the Team was last updated</param>
/// <param name="Info">Team description</param>
/// <param name="ThumbnailUrl">Image URL for the Team logo</param>
/// <param name="TeamName">Team name</param>
/// <param name="TeamDisplayName">Team display name</param>
/// <param name="Id">Team ID</param>
public record ChannelTeamResponseBody(string BroadcasterId, string BroadcasterLogin, string BroadcasterName, string? BackgroundImageUrl, string? Banner, DateTime CreatedAt, DateTime UpdatedAt, string Info, string? ThumbnailUrl, string TeamName, string TeamDisplayName, string Id);
