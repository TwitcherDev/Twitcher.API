using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses
{
    public class RedemptionResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("reward")]
        public CustomRewardResponse? Reward { get; set; }
    }
}
