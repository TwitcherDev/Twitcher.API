namespace Twitcher.API.Exceptions;

/// <summary>Something bad happened on twitch side</summary>
public class InternalServerException : Exception
{
    public InternalServerException() : base("Something bad happened on twitch side") { }
}
