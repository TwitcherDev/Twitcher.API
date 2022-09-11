namespace Twitcher.API.Models;

/// <param name="Track">The track that's currently playing</param>
/// <param name="Source">The source of the track that's currently playing. For example, a playlist or station</param>
public record SoundtrackCurrentTrack(SoundtrackTrack Track, SoundtrackSource Source);

/// <param name="Album">The album that includes the track</param>
/// <param name="Artists">The artists included on the track</param>
/// <param name="Duration">The duration of the track, in seconds</param>
/// <param name="Id">The track's ASIN (Amazon Standard Identification Number)</param>
/// <param name="Isrc">The track's ISRC (International Standard Recording Code)</param>
/// <param name="Title">The track's title. If the track contains explicit content, the title will contain [Explicit] in the string. For example, Let It Die [Explicit]</param>
public record SoundtrackTrack(SoundtrackAlbum Album, SoundtrackArtist[] Artists, int Duration, string Id, string Isrc, string Title);

/// <param name="Id">The album's ASIN (Amazon Standard Identification Number)</param>
/// <param name="ImageUrl">A URL to the album's cover art</param>
/// <param name="Name">The album's name. If the album contains explicit content, the name will contain [Explicit] in the string. For example, Let It Die [Explicit]</param>
public record SoundtrackAlbum(string Id, string ImageUrl, string Name);

/// <param name="CreatorChannelId">The ID of the Twitch user that created the track. Is empty if a Twitch user didn't create the track</param>
/// <param name="Id">The artist's ASIN (Amazon Standard Identification Number)</param>
/// <param name="Name">The artist's name. This can be the band's name or the solo artist's name</param>
public record SoundtrackArtist(string CreatorChannelId, string Id, string Name);

/// <param name="ContentType">The type of content that id maps to. Possible values are:</param>
/// <param name="Id">The playlist's or station's ASIN (Amazon Standard Identification Number)</param>
/// <param name="ImageUrl">A URL to the playlist's or station's image art</param>
/// <param name="SoundtrackUrl">A URL to the playlist on Soundtrack. The string is empty if content-type is STATION</param>
/// <param name="SpotifyUrl">A URL to the playlist on Spotify. The string is empty if content-type is STATION</param>
/// <param name="Title">The playlist's or station's title</param>
public record SoundtrackSource(SoundtrackContentType ContentType, string Id, string ImageUrl, string SoundtrackUrl, string SpotifyUrl, string Title);

/// <param name="Description">A short description about the music that the playlist includes</param>
/// <param name="Id">The playlist's ASIN (Amazon Standard Identification Number)</param>
/// <param name="ImageUrl">A URL to the playlist's image art. Is empty if the playlist doesn't include art</param>
/// <param name="Title">The playlist's title</param>
public record SoundtrackPlaylistMetadata(string Description, string Id, string ImageUrl, string Title);