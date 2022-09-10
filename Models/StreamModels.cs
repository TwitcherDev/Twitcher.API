namespace Twitcher.API.Models;

/// <param name="StreamKey">Stream key for the channel</param>
public record StreamKeyResponseBody(string StreamKey);

/// <param name="Id">Stream ID</param>
/// <param name="UserId">ID of the user who is streaming</param>
/// <param name="UserLogin">Login of the user who is streaming</param>
/// <param name="UserName">Display name corresponding to <paramref name="UserId"/></param>
/// <param name="GameId">ID of the game being played on the stream</param>
/// <param name="GameName">Name of the game being played</param>
/// <param name="Type">Stream type: "live" or "" (in case of error)</param>
/// <param name="Title">Stream title</param>
/// <param name="ViewerCount">Number of viewers watching the stream at the time of the query</param>
/// <param name="StartedAt">UTC start stream time</param>
/// <param name="Language">Stream language. A language value is either the ISO 639-1 two-letter code</param>
/// <param name="ThumbnailUrl">Thumbnail URL of the stream. All image URLs have variable width and height. You can replace {width} and {height} with any values to get that size image</param>
/// <param name="TagIds">Shows tag IDs that apply to the stream</param>
/// <param name="IsMature">Indicates if the broadcaster has specified their channel contains mature content that may be inappropriate for younger audiences</param>
public record StreamResponseBody(string Id, string UserId, string UserLogin, string UserName,
    string GameId, string GameName, string Type, string Title, int ViewerCount, DateTime StartedAt,
    string Language, string ThumbnailUrl, string[] TagIds, bool IsMature);

/// <param name="GameId">ID of the game being played on the stream</param>
/// <param name="GameName">Name of the game being played</param>
/// <param name="Id">Stream ID</param>
/// <param name="Language">Stream language. A language value is either the ISO 639-1 two-letter code for a supported stream language or "other"</param>
/// <param name="StartedAt">UTC timestamp</param>
/// <param name="TagIds">Shows tag IDs that apply to the stream</param>
/// <param name="ThumbnailUrl">Thumbnail URL of the stream. All image URLs have variable width and height. You can replace {width} and {height} with any values to get that size image</param>
/// <param name="Title">Stream title</param>
/// <param name="Type">Stream type: "live" or "" (in case of error)</param>
/// <param name="UserId">ID of the user who is streaming</param>
/// <param name="UserLogin">Login of the user who is streaming</param>
/// <param name="UserName">Display name corresponding to <paramref name="UserId"/></param>
/// <param name="ViewerCount">Number of viewers watching the stream at the time of the query</param>
public record FollowedStreamResponseBody(string GameId, string GameName, string Id, string Language, DateTime StartedAt, string[] TagIds, string ThumbnailUrl, string Title, string Type, string UserId, string UserLogin, string UserName, int ViewerCount);

/// <param name="Id">ID of the marker</param>
/// <param name="CreatedAt">RFC3339 timestamp of the marker</param>
/// <param name="Description">Description of the marker</param>
/// <param name="PositionSeconds">Relative offset (in seconds) of the marker, from the beginning of the stream</param>
/// <param name="Url">A link to the stream with a query parameter that is a timestamp of the marker's location</param>
/// <param name="UserId">ID of the user whose markers are returned</param>
/// <param name="UserName">Display name corresponding to user_id</param>
/// <param name="UserLogin">Login corresponding to user_id</param>
/// <param name="VideoId">ID of the stream (VOD/video) that was marked</param>
public record GetStreamMarkerResponseBody(string Id, DateTime CreatedAt, string? Description, int PositionSeconds, [property: JsonProperty("URL")] string Url, string UserId, string UserName, string UserLogin, string VideoId);

/// <param name="UserId">ID of the broadcaster in whose live stream the marker is created</param>
/// <param name="Description">Description of or comments on the marker. Max length is 140 characters</param>
public record StreamMarkerRequestBody(string UserId, string? Description);

/// <param name="CreatedAt">RFC3339 timestamp of the marker</param>
/// <param name="Description">Description of the marker</param>
/// <param name="Id">Unique ID of the marker</param>
/// <param name="PositionSeconds">Relative offset (in seconds) of the marker, from the beginning of the stream</param>
public record StreamMarkerResponseBody(DateTime CreatedAt, string? Description, string Id, int PositionSeconds);
