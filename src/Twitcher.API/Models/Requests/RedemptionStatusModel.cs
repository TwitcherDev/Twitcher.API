using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Requests
{
    public class RedemptionStatusModel
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
