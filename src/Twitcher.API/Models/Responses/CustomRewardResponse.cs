using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses
{
    public class CustomRewardResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("cost")]
        public int Cost { get; set; }
    }
}
