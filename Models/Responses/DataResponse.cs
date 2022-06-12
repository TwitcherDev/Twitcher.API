using System.Text.Json.Serialization;

namespace Twitcher.API.Models.Responses
{
    public class DataResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
