namespace Twitcher.API.Exceptions;

/// <summary>State was created outside of aplication</summary>
public class WrongStateException : Exception
{
    public WrongStateException() : base("State was created outside of aplication") { }
}
