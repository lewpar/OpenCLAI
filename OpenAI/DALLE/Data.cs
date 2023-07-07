using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCLAI.OpenAI.DALLE
{
    public class Data
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
