using OpenCLAI.Configuration;
using OpenCLAI.Models;

namespace OpenCLAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = await Config.TryLoadConfigAsync();
            if (config.Result == Result.Failed)
            {
                Console.WriteLine($"An error occured while trying to retrieve the config: {config.Message}");
                return;
            }

            if(config.Config is null)
            {
                Console.WriteLine($"An error occured while tryuing to retrieve the config: Config was null.");
                return;
            }

            if(string.IsNullOrEmpty(config.Config.OpenAIKey))
            {
                Console.WriteLine("An error occured while trying to get OpenAIKey: OpenAIKey was not set.");
                Console.WriteLine("Ensure you set the OpenAIKey in the config.");
                return;
            }
        }
    }
}