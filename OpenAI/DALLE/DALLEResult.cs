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
