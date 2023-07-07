using System.Text.Json.Serialization;

namespace OpenCLAI.OpenAI.DALLE
{
    public class Request
    {
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        [JsonPropertyName("n")]
        public int Count { get; set; }

        [JsonPropertyName("size")]
        public string? Size { get; set; }

        [JsonPropertyName("response_format")]
        public string? Format { get; set; }
    }
}
