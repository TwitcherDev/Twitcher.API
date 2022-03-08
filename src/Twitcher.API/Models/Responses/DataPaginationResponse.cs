using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses
{
    public class DataPaginationResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("pagination")]
        public Pagination? Pagination { get; set; }
    }
}
