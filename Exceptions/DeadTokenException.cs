namespace Twitcher.API.Exceptions;

/// <summary>Refresh token are dead or withdrawn by user</summary>
public class DeadTokenException : Exception
{
    public DeadTokenException() : base("Refresh token are dead or withdrawn by user") { }
}
