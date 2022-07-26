namespace Twitcher.API.Exceptions;

/// <summary>Special scope require</summary>
public class ScopeRequireException : Exception
{
    public string Scope { get; set; }

    public ScopeRequireException(string scope) : base($"Request require '{scope}' scope")
    {
        Scope = scope;
    }
}
