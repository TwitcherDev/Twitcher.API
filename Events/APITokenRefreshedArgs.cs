namespace Twitcher.API.Events;

/// <summary>Contains new tokens</summary>
public class APITokenRefreshedArgs : EventArgs
{
    /// <summary>New tokens</summary>
    public string Tokens { get; set; }

    internal APITokenRefreshedArgs(string tokens)
    {
        Tokens = tokens;
    }
}
