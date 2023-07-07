using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
