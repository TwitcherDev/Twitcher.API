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
public record UserResponse(string Id, string Login, string DisplayName, UserType Type, BroadcasterType BroadcasterType, string? Description, string? ProfileImageUrl, string? OfflineImageUrl, string? Email, DateTime CreatedAt);

/// <param name="Data">Response data</param>
/// <param name="Total">Total number of items returned</param>
/// <param name="Pagination">A cursor value, to be used in a subsequent request to specify the starting point of the next set of results</param>
public record UserFollowsResponse(UserFollowsPair[] Data, int Total, Pagination Pagination) : DataPaginationResponse<UserFollowsPair[]>(Data, Pagination);

/// <param name="FromId">ID of the user following the <paramref name="ToId"/> user</param>
/// <param name="FromLogin">Login of the user following the <paramref name="ToId"/> user</param>
/// <param name="FromName">Display name corresponding to <paramref name="FromId"/></param>
/// <param name="ToId">ID of the user being followed by the <paramref name="FromId"/> user</param>
/// <param name="ToLogin">Login of the user being followed by the <paramref name="FromId"/> user</param>
/// <param name="ToName">Display name corresponding to <paramref name="ToId"/></param>
/// <param name="FollowedAt">Date and time when the <paramref name="FromId"/> user followed the <paramref name="ToId"/> user</param>
public record UserFollowsPair(string FromId, string FromLogin, string FromName, string ToId, string ToLogin, string ToName, DateTime FollowedAt);