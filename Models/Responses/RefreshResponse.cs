﻿using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses;

public class RefreshResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("scope")]
    public string[]? Scopes { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}