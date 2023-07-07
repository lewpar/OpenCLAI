using OpenCLAI.CommandLine;
using OpenCLAI.Configuration;
using OpenCLAI.Models;
using OpenCLAI.OpenAI;

namespace OpenCLAI
{
    internal class Program
    {
        static OpenAIService openAIService;
        static Dictionary<string, Func<string[], Task>> aiOptions = new Dictionary<string, Func<string[], Task>>()
        {
            { "chatgpt",  HandleChatGPT }
        };

        static bool TryGetOption(string[] args, out string? option)
        {
            foreach(var aiOption in aiOptions)
            {
                if(ArgumentParser.TryFind(args, $"-{aiOption.Key}", out var argument))
                {
                    // Remove the leading dash.
                    option = argument.Name?.Substring(1);
                    return true;
                }
            }
            
            option = null;
            return false;
        }
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

            openAIService = new OpenAIService(config.Config.OpenAIKey);

            string? option = null;
            if(!TryGetOption(args, out option))
            {
                Console.WriteLine("You must supply an OpenAI option as a parameter. Type 'openclai --options' to view them all.");
                Console.WriteLine("Example: openclai -chatgpt -prompt \"How many planets are in our solar system?\"");
                return;
            }

            if(option is null)
            {
                Console.WriteLine("An error occured while getting OpenAI option: Options was null.");
                return;
            }

            await HandleOption(args, option);
        }

        static async Task HandleOption(string[] args, string? option)
        {
            await aiOptions[option!](args);
        }

        static async Task HandleChatGPT(string[] args)
        {
            Argument? prompt = null;
            if(!ArgumentParser.TryFind(args, "-prompt", out prompt) && 
                !ArgumentParser.TryFind(args, "-p", out prompt))
            {
                Console.WriteLine("Prompt expected when using ChatGPT option.");
                Console.WriteLine("Example: openclai -chatgpt -prompt \"What color is a banana?\"");
                return;
            }

            if (prompt is null || 
                string.IsNullOrEmpty(prompt.Value))
            {
                Console.WriteLine("An error occured while getting ChatGPT prompt: Prompt was null.");
                return;
            }

            var result = await openAIService.SendPromptAsync(prompt.Value, new List<OpenAI.ChatGPT.Message>());

            if(result is null)
            {
                Console.WriteLine("An error occured while getting the ChatGPT result: Result was null.");
                return;
            }

            if(result.Status == OpenAIResultStatus.Failure)
            {
                Console.WriteLine($"An error occured during the ChatGPT request: {result.Message}");
                return;
            }

            Console.WriteLine(result.Message);
        }
    }
}