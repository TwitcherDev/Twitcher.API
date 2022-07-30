namespace Twitcher.API.Models;

/// <param name="Id">ID of the video</param>
/// <param name="StreamId">ID of the stream that the video originated from if the <paramref name="Type"/> is <see cref="VideoType.Archive"/>. Otherwise set to <see langword="null"/></param>
/// <param name="UserId">ID of the user who owns the video</param>
/// <param name="UserLogin">Login of the user who owns the video</param>
/// <param name="UserName">Display name corresponding to <paramref name="UserId"/></param>
/// <param name="Title">Title of the video</param>
/// <param name="Description">Description of the video</param>
/// <param name="CreatedAt">Date when the video was created</param>
/// <param name="PublishedAt">Date when the video was published</param>
/// <param name="Url">URL of the video</param>
/// <param name="ThumbnailUrl">Template URL for the thumbnail of the video</param>
/// <param name="Viewable">Indicates whether the video is publicly viewable</param>
/// <param name="ViewCount">Number of times the video has been viewed</param>
/// <param name="Language">Language of the video. A language value is either the ISO 639-1 two-letter code</param>
/// <param name="Type">Type of video</param>
/// <param name="Duration">Length of the video</param>
/// <param name="MutedSegments">Array of muted segments in the video. If there are no muted segments, the value will be <see langword="null"/></param>
public record VideoResponseBody(string Id, string? StreamId, string UserId, string UserLogin, string UserName, string Title, string Description,
    DateTime CreatedAt, DateTime PublishedAt, string Url, string ThumbnailUrl, ViewableType Viewable, int ViewCount, string Language, VideoType Type, string Duration,
    VideoMutedSegment[]? MutedSegments);

/// <param name="Duration">Duration of the muted segment</param>
/// <param name="Offset">Offset in the video at which the muted segment begins</param>
public record VideoMutedSegment(int Duration, int Offset);
