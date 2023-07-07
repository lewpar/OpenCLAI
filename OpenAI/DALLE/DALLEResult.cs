using OpenCLAI.OpenAI.ChatGPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLAI.OpenAI.DALLE
{
    public class DALLEResult : OpenAIResult
    {
        public DALLE.Response? Response { get; set; }

        public DALLEResult(OpenAIResultStatus status, string message, Response? response = null) : base(status, message)
        {
            Response = response;
        }
    }
}
