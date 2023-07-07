namespace OpenCLAI.OpenAI
{
    public enum OpenAIResultStatus
    {
        Success,
        Failure
    }

    public abstract class OpenAIResult
    {
        public OpenAIResultStatus Status { get; set; }
        public string Message { get; set; }

        public OpenAIResult(OpenAIResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
