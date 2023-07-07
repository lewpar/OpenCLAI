using System.Text.Json.Serialization;

namespace OpenCLAI.OpenAI.ChatGPT
{
    public class Response
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("usage")]
        public TokenUsage? Usage { get; set; }

        [JsonPropertyName("choices")]
        public List<Choice>? Choices { get; set; }
    }
}
