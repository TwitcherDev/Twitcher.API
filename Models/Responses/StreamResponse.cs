using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses
{
    public class StreamResponse
    {
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("user_login")]
        public string? UserLogin { get; set; }
    }
}
