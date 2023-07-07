using System.Text.Json.Serialization;

namespace OpenCLAI.OpenAI.DALLE
{
    public class Response
    {
        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("data")]
        public List<Data>? Data { get; set; }
    }
}
