namespace Twitcher.API.Events;

/// <summary>Contains new tokens and owner</summary>
public class TokenRefreshedArgs : EventArgs
{
    /// <summary>Owner tag</summary>
    public string? Tag { get; set; }
    /// <summary>Owner user id</summary>
    public string UserId { get; set; }
    /// <summary>New tokens</summary>
    public string Tokens { get; set; }

    internal TokenRefreshedArgs(string? tag, string userId, string tokens)
    {
        Tag = tag;
        UserId = userId;
        Tokens = tokens;
    }
}
