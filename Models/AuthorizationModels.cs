namespace Twitcher.API.Models;

internal record AuthorizationCodeResponseBody(string AccessToken, string RefreshToken);

internal record RefreshResponseBody(string AccessToken, string RefreshToken, string[] Scopes, int ExpiresIn);

internal record ValidateResponseBody(string[] Scopes, int ExpiresIn, string Login, string UserId);
