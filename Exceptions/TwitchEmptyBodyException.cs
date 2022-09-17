namespace Twitcher.API.Exceptions;

/// <summary>Twitch returned a successful response without body</summary>
public class TwitchEmptyBodyException : Exception
{
    internal TwitchEmptyBodyException() : base("Twitch returned a successful response without body") { }
}
