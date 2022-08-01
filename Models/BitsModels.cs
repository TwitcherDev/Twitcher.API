namespace Twitcher.API.Models;

#region Bits
/// <param name="Data">Array of leaderboard users</param>
/// <param name="DateRange">Date range for returned data</param>
/// <param name="Total">Total number of results (users) returned. This is count or the total number of entries in the leaderboard, whichever is less</param>
public record BitsLeaderboardResponseBody(BitsLeaderboardMember[] Data, BitsLeaderboardRange DateRange, int Total) : DataResponse<BitsLeaderboardMember[]>(Data);

/// <param name="UserId">ID of the user (viewer) in the leaderboard entry</param>
/// <param name="UserLogin">User login name</param>
/// <param name="UserName">Display name corresponding to <paramref name="UserId" /></param>
/// <param name="Rank">Leaderboard rank of the user</param>
/// <param name="Score">Leaderboard score (number of Bits) of the user</param>
public record BitsLeaderboardMember(string UserId, string UserLogin, string UserName, int Rank, int Score);

/// <param name="StartedAt">Start of the date range for the returned data</param>
/// <param name="EndedAt">End of the date range for the returned data</param>
public record BitsLeaderboardRange(DateTime? StartedAt, DateTime? EndedAt);
#endregion

#region Cheermotes
/// <param name="Prefix">The string used to Cheer that precedes the Bits amount</param>
/// <param name="Tiers">An array of Cheermotes with their metadata</param>
/// <param name="Type">Emote type</param>
/// <param name="Order">Order of the emotes as shown in the bits card, in ascending order</param>
/// <param name="LastUpdated">The data when this Cheermote was last updated</param>
/// <param name="IsCharitable">Indicates whether or not this emote provides a charity contribution match during charity campaigns</param>
public record CheermotesResponseBody(string Prefix, CheermoteTier[] Tiers, CheermoteType Type, int Order, DateTime LastUpdated, bool IsCharitable);

/// <param name="MinBits">Minimum number of bits needed to be used to hit the given tier of emote</param>
/// <param name="Id">ID of the emote tier. Possible tiers are: 1,100,500,1000,5000, 10k, or 100k</param>
/// <param name="Color">Hex code for the color associated with the bits of that tier. Grey, Purple, Teal, Blue, or Red color to match the base bit type</param>
/// <param name="Images">Structure containing both animated and static image sets, sorted by light and dark</param>
/// <param name="CanCheer">Indicates whether or not emote information is accessible to users</param>
/// <param name="ShowInBitsCard">Indicates whether or not we hide the emote from the bits card</param>
public record CheermoteTier(int MinBits, string Id, string Color, CheermoteImages Images, bool CanCheer, bool ShowInBitsCard);

/// <param name="Dark">Dark theme</param>
/// <param name="Light">Light theme</param>
public record CheermoteImages(CheermoteImagesTheme Dark, CheermoteImagesTheme Light);

/// <param name="Animated">Animated</param>
/// <param name="Static">Static</param>
public record CheermoteImagesTheme(Dictionary<string, string> Animated, Dictionary<string, string> Static);
#endregion
