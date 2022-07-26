namespace Twitcher.API.Events;

public class APITokenRefreshedArgs : EventArgs
{
    public string Tokens { get; set; }

    public APITokenRefreshedArgs(string tokens)
    {
        Tokens = tokens;
    }
}
