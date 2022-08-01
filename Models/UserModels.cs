namespace Twitcher.API.Models;

/// <param name="Id">User ID</param>
/// <param name="Login">User login name</param>
/// <param name="DisplayName">User display name</param>
/// <param name="Type">User type</param>
/// <param name="BroadcasterType">User broadcaster type</param>
/// <param name="Description">User channel description</param>
/// <param name="ProfileImageUrl">URL of the user profile image</param>
/// <param name="OfflineImageUrl">URL of the user offline image</param>
/// <param name="Email">User verified email address. Returned if the request includes the '<inheritdoc cref="Scopes.UserReadEmail"/>' scope</param>
/// <param name="CreatedAt">Date when the user was created</param>
public record UserResponseBody(string Id, string Login, string DisplayName, UserType Type, BroadcasterType BroadcasterType, string? Description, string? ProfileImageUrl, string? OfflineImageUrl, string? Email, DateTime CreatedAt);

/// <param name="Data">Response data</param>
/// <param name="Total">Total number of items returned</param>
/// <param name="Pagination">A cursor value, to be used in a subsequent request to specify the starting point of the next set of results</param>
public record UserFollowsResponseBody(UserFollowsPair[] Data, int Total, Pagination Pagination) : DataPaginationResponse<UserFollowsPair[]>(Data, Pagination);

/// <param name="FromId">ID of the user following the <paramref name="ToId"/> user</param>
/// <param name="FromLogin">Login of the user following the <paramref name="ToId"/> user</param>
/// <param name="FromName">Display name corresponding to <paramref name="FromId"/></param>
/// <param name="ToId">ID of the user being followed by the <paramref name="FromId"/> user</param>
/// <param name="ToLogin">Login of the user being followed by the <paramref name="FromId"/> user</param>
/// <param name="ToName">Display name corresponding to <paramref name="ToId"/></param>
/// <param name="FollowedAt">Date and time when the <paramref name="FromId"/> user followed the <paramref name="ToId"/> user</param>
public record UserFollowsPair(string FromId, string FromLogin, string FromName, string ToId, string ToLogin, string ToName, DateTime FollowedAt);

/// <param name="UserId">User ID of the blocked user</param>
/// <param name="UserLogin">Login of the blocked user</param>
/// <param name="DisplayName">Display name of the blocked user</param>
public record UserBlockResponseBody(string UserId, string UserLogin, string DisplayName);

/// <param name="Id">ID of the extension</param>
/// <param name="Version">Version of the extension</param>
/// <param name="Name">Name of the extension</param>
/// <param name="CanActivate">Indicates whether the extension is configured such that it can be activated</param>
/// <param name="Type">Types for which the extension can be activated</param>
public record UserExtensionsResponseBody(string Id, string Version, string Name, bool CanActivate, ExtensionType[] Type);

/// <param name="Component">Contains data for video-component Extensions</param>
/// <param name="Panel">Contains data for panel Extensions</param>
/// <param name="Overlay">Contains data for video-overlay Extensions</param>
public record UserActiveExtensionsResponseBody(Dictionary<string, UserActiveExtensionComponent> Component, Dictionary<string, UserActiveExtension> Panel, Dictionary<string, UserActiveExtension> Overlay);

/// <param name="Active">Activation state of the extension, for each extension type (component, overlay, mobile, panel). If <see langword="false"/>, no other data is provided</param>
/// <param name="Id">ID of the extension</param>
/// <param name="Version">Version of the extension</param>
/// <param name="Name">Name of the extension</param>
public record UserActiveExtension(bool Active, string? Id, string? Version, string? Name);

/// <param name="Active">Activation state of the extension, for each extension type (component, overlay, mobile, panel). If <see langword="false"/>, no other data is provided</param>
/// <param name="Id">ID of the extension</param>
/// <param name="Version">Version of the extension</param>
/// <param name="Name">Name of the extension</param>
/// <param name="X">X-coordinate of the placement of the extension</param>
/// <param name="Y">Y-coordinate of the placement of the extension</param>
public record UserActiveExtensionComponent(bool Active, string? Id, string? Version, string? Name, int X, int Y) : UserActiveExtension(Active, Id, Version, Name);
