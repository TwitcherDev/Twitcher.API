namespace Twitcher.API.Events;

/// <summary>Contains new tokens and owner</summary>
public class APITokenRefreshedArgs : EventArgs
{
    /// <summary>Api name</summary>
    public string Name { get; set; }
    /// <summary>New access tokens</summary>
    public string AccessToken { get; set; }
    /// <summary>New refresh tokens</summary>
    public string RefreshToken { get; set; }
    /// <summary>Twitch id of the token owner</summary>
    public string UserId { get; set; }

    /// <summary>Access and refresh tokens in 'access:refresh' format</summary>
    public string Tokens => AccessToken + ':' + RefreshToken;

    internal APITokenRefreshedArgs(string name, string accessToken, string refreshToken, string userId)
    {
        Name = name;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        UserId = userId;
    }
}
