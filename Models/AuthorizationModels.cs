namespace Twitcher.API.Models;

/// <param name="AccessToken"></param>
/// <param name="RefreshToken"></param>
public record AuthorizationCodeResponseBody(string AccessToken, string RefreshToken);

/// <param name="AccessToken"></param>
/// <param name="RefreshToken"></param>
/// <param name="Scopes"></param>
/// <param name="ExpiresIn"></param>
public record RefreshResponseBody(string AccessToken, string RefreshToken, string[] Scopes, int ExpiresIn);

/// <param name="AccessToken"></param>
/// <param name="RefreshToken"></param>
/// <param name="Scopes"></param>
/// <param name="ExpiresIn"></param>
public record ValidateResponseBody(string[] Scopes, int ExpiresIn, string Login, string UserId);
