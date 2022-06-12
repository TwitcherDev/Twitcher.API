using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses
{
    public class Pagination
    {

        [JsonPropertyName("cursor")]
        public string? Cursor { get; set; }
    }
}