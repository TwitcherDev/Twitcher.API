namespace Twitcher.API.Exceptions;

/// <summary>403 Forbidden status in response</summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string meaning) : base("403 Forbidden status in twitch response: " + meaning) { }
}
