namespace Twitcher.API.Exceptions;

/// <summary>State was created outside of aplication</summary>
public class WrongStateException : Exception
{
    internal WrongStateException() : base("State was created outside of aplication") { }
}
