using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Requests
{
    public class CustomRewardModel
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("cost")]
        public int Cost { get; set; }
    }
}
