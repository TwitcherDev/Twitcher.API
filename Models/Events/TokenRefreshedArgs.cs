namespace Twitcher.API.Models.Events;

public class TokenRefreshedArgs : EventArgs
{
    public string Group { get; set; }
    public string UserId { get; set; }
    public string Tokens { get; set; }

    public TokenRefreshedArgs(string group, string userId, string tokens)
    {
        Group = group;
        UserId = userId;
        Tokens = tokens;
    }
}
