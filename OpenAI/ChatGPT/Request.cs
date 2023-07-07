using System.Text.Json.Serialization;

namespace OpenCLAI.OpenAI.ChatGPT
{
    public class Request
    {
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("messages")]
        public List<Message>? Messages { get; set; }
    }
}
