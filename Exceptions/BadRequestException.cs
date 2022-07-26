namespace Twitcher.API.Exceptions;

/// <summary>Query Parameter missing or invalid</summary>
public class BadRequestException : Exception
{
    public BadRequestException() : base("Query Parameter missing or invalid") { }
}
