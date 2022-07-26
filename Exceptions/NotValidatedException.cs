namespace Twitcher.API.Exceptions;

/// <summary>Validate needed before the first request</summary>
public class NotValidatedException : Exception
{
    public NotValidatedException() : base("Validate needed before the first request") { }
}
