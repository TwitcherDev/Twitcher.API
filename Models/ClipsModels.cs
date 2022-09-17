namespace Twitcher.API.Models;

/// <param name="EditUrl">URL of the edit page for the clip</param>
/// <param name="Id">ID of the clip that was created</param>
public record ClipMetadata(string EditUrl, string Id);

/// <param name="Id">ID of the clip being queried</param>
/// <param name="Url">URL where the clip can be viewed</param>
/// <param name="EmbedUrl">URL to embed the clip</param>
/// <param name="BroadcasterId">User ID of the stream from which the clip was created</param>
/// <param name="BroadcasterName">Display name corresponding to <paramref name="BroadcasterId"/></param>
/// <param name="CreatorId">ID of the user who created the clip</param>
/// <param name="CreatorName">Display name corresponding to <paramref name="CreatorId"/></param>
/// <param name="VideoId">ID of the video from which the clip was created. <see cref="string.Empty"/> if the video is not available</param>
/// <param name="GameId">ID of the game assigned to the stream when the clip was created</param>
/// <param name="Language">Language of the stream from which the clip was created. A language value is either the ISO 639-1 two-letter code or "other"</param>
/// <param name="Title">Title of the clip</param>
/// <param name="ViewCount">Number of times the clip has been viewed</param>
/// <param name="CreatedAt">Date when the clip was created</param>
/// <param name="ThumbnailUrl">URL of the clip thumbnail</param>
/// <param name="Duration">Duration of the Clip in seconds</param>
/// <param name="VodOffset">The zero-based offset, in seconds, to where the clip starts in the video (VOD). Is <see langword="null"/> if the video is not available or hasn't been created yet from the live stream. See <paramref name="VideoId"/></param>
public record ClipData(string Id, string Url, string EmbedUrl, string BroadcasterId, string BroadcasterName, string CreatorId, string CreatorName, string VideoId, string GameId, string Language, string Title, int ViewCount, DateTime CreatedAt, string ThumbnailUrl, float Duration, int? VodOffset);