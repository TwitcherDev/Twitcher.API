namespace Twitcher.API.Exceptions;

/// <summary>Token revoked by user</summary>
public class TokenRevokedException : Exception
{
    internal TokenRevokedException(Exception innerException) : base("Token revoked by user", innerException) { }
}
