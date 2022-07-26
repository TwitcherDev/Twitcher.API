namespace Twitcher.API.Models;

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