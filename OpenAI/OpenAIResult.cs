namespace OpenCLAI.OpenAI
{
    public enum OpenAIResultStatus
    {
        Success,
        Failure
    }

    public class OpenAIResult
    {
        public OpenAIResultStatus Status { get; set; }
        public string Message { get; set; }
        public ChatGPT.Response? Response { get; set; }

        public OpenAIResult(OpenAIResultStatus status, string message, ChatGPT.Response? response = null)
        {
            Status = status;
            Message = message;
            Response = response;
        }
    }
}
