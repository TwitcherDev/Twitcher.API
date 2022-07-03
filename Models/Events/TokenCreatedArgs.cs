namespace Twitcher.API;

public class TokenCreatedArgs : EventArgs
{
    public string Group { get; set; }
    public string UserId { get; set; }
    public string Tokens { get; set; }

    public TokenCreatedArgs(string group, string userId, string tokens)
    {
        Group = group;
        UserId = userId;
        Tokens = tokens;
    }
}