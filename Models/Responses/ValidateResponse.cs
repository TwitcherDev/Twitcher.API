using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses;

public class ValidateResponse
{
    [JsonPropertyName("scopes")]
    public string[]? Scopes { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}
