namespace Twitcher.API.Models;

/// <param name="CreatedAt">The UTC date and time, when the raid request was created</param>
/// <param name="IsMature">A Boolean value that indicates whether the channel being raided contains mature content</param>
public record StartRaidMetadata(DateTime CreatedAt, bool IsMature);
