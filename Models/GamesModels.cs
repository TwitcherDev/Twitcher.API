namespace Twitcher.API.Models;

/// <param name="Id">Game ID</param>
/// <param name="Name">Game name</param>
/// <param name="BoxArtUrl">Template URL for a game's box art</param>
public record GameMetadata(string Id, string Name, string BoxArtUrl);
