namespace Twitcher.API.Models.Events;

public class TokenDeadArgs : EventArgs
{
    public string Group { get; set; }
    public string? UserId { get; set; }

    public TokenDeadArgs(string group, string? userId)
    {
        Group = group;
        UserId = userId;
    }
}