using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLAI.OpenAI.ChatGPT
{
    public class ChatGPTResult : OpenAIResult
    {
        public ChatGPT.Response? Response { get; set; }

        public ChatGPTResult(OpenAIResultStatus status, string message, Response? response = null) : base(status, message)
        {
            Response = response;
        }
    }
}
