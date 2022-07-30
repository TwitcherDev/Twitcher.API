namespace Twitcher.API.Events;

public class TokenRefreshedArgs : EventArgs
{
    public string? Tag { get; set; }
    public string UserId { get; set; }
    public string Tokens { get; set; }

    public TokenRefreshedArgs(string? tag, string userId, string tokens)
    {
        Tag = tag;
        UserId = userId;
        Tokens = tokens;
    }
}
