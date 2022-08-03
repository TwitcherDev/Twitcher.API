namespace Twitcher.API.Models;

internal record AuthorizationCodeResponseBody(string AccessToken, string RefreshToken);

/// <param name="AccessToken">New access token</param>
/// <param name="RefreshToken">New refresh token</param>
/// <param name="Scopes">Token scopes</param>
/// <param name="ExpiresIn">Token expiration time</param>
public record RefreshResponseBody(string AccessToken, string RefreshToken, string[] Scopes, int ExpiresIn);

/// <param name="Scopes">Token scopes</param>
/// <param name="ExpiresIn">Token expiration time</param>
/// <param name="Login">Token owner login</param>
/// <param name="UserId">Token owner user id</param>
public record ValidateResponseBody(string[] Scopes, int ExpiresIn, string Login, string UserId);
