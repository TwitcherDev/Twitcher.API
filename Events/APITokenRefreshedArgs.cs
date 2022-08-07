namespace Twitcher.API.Events;

/// <summary>Contains new tokens</summary>
public class TokenRefreshedArgs : EventArgs
{
    /// <summary>New access tokens</summary>
    public string AccessToken { get; set; }
    /// <summary>New refresh tokens</summary>
    public string RefreshToken { get; set; }
    /// <summary>Twitch id of the token owner</summary>
    public string UserId { get; set; }

    /// <summary>Access and refresh tokens in 'access:refresh' format</summary>
    public string Tokens => AccessToken + ':' + RefreshToken;

    internal TokenRefreshedArgs(string accessToken, string refreshToken, string userId)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        UserId = userId;
    }
}
