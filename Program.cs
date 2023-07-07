using OpenCLAI.CommandLine;
using OpenCLAI.Configuration;
using OpenCLAI.Models;
using System.Security.Cryptography;

namespace OpenCLAI
{
    internal class Program
    {
        static Dictionary<string, Action<string[]>> aiOptions = new Dictionary<string, Action<string[]>>()
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

            string? option = null;
            if(!TryGetOption(args, out option))
            {
                Console.WriteLine("You must supply an OpenAI option as a parameter. Type 'openclai --options' to view them all.");
                Console.WriteLine("Example: openclai -chatgpt -p \"How many planets are in our solar system?\"");
                return;
            }

            if(option is null)
            {
                Console.WriteLine("An error occured while getting OpenAI option: Options was null.");
                return;
            }

            HandleOption(args, option);
        }

        static void HandleOption(string[] args, string? option)
        {
            aiOptions[option!].Invoke(args);
        }

        static void HandleChatGPT(string[] args)
        {
            Argument? prompt = null;
            if(!ArgumentParser.TryFind(args, "-prompt", out prompt) && 
                !ArgumentParser.TryFind(args, "-p", out prompt))
            {
                Console.WriteLine("Prompt expected when using ChatGPT option.");
                Console.WriteLine("Example: openclai -chatgpt -prompt \"What color is a banana?\"");
                return;
            }

            if (prompt is null)
            {
                Console.WriteLine("An error occured while getting ChatGPT prompt: Prompt was null.");
                return;
            }
        }
    }
}