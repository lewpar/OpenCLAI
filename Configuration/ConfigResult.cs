using OpenCLAI.Models;

namespace OpenCLAI.Configuration
{
    public class ConfigResult
    {
        public ConfigResult(Config? config, Result result, string? message)
        {
            Config = config;
            Result = result;
            Message = message;
        }

        public Config? Config { get; set; }
        public Result Result { get; set; }
        public string? Message { get; set; }
    }
}
