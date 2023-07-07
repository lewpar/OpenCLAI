using System.Text.Json.Serialization;

namespace OpenCLAI.OpenAI.DALLE
{
    public class Data
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
