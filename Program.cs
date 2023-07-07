﻿using OpenCLAI.CommandLine;
using OpenCLAI.Configuration;
using OpenCLAI.Models;

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
                    option = argument.Name;
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

            Console.WriteLine($"Found option {option}");
        }

        static void HandleChatGPT(string[] args)
        {

        }
    }
}