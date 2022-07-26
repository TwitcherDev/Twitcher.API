namespace Twitcher.API.Exceptions;

/// <summary>404 Not Found status in response</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string meaning) : base("404 Not Found status in response: " + meaning) { }
}
