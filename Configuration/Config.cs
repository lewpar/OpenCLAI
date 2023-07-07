using OpenCLAI.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCLAI.Configuration
{
    public class Config
    {
        const string PATH_CONFIG = "./config.json";

        public static bool TryGetConfigPath(out string? path)
        {
            var processPath = Path.GetDirectoryName(Environment.ProcessPath);

            if (processPath is null)
            {
                path = null;
                return false;
            }

            path = Path.GetFullPath(processPath);
            path = Path.Combine(path, PATH_CONFIG);

            return true;
        }

        public static async Task<ConfigResult> TryLoadConfigAsync()
        {
            string? path;

            if (!TryGetConfigPath(out path))
            {
                return new ConfigResult(null, Result.Failed, "Failed to retrieve config path.");
            }

            if (path is null)
            {
                return new ConfigResult(null, Result.Failed, "Config path was null.");
            }

            if (!File.Exists(path))
            {
                using FileStream fsCreate = File.Create(path);
                await JsonSerializer.SerializeAsync<Config>(fsCreate, new Config());
                await fsCreate.DisposeAsync();
            }

            if(!File.Exists(path))
            {
                return new ConfigResult(null, Result.Failed, $"Config does not exist at path '{path}'.");
            }

            using FileStream fsRead = File.OpenRead(path);
            var config = await JsonSerializer.DeserializeAsync<Config>(fsRead);
            await fsRead.DisposeAsync();

            if(config is null)
            {
                return new ConfigResult(null, Result.Failed, "Config was null.");
            }

            return new ConfigResult(config, Result.Success, default);
        }

        [JsonPropertyName("IsDebug")]
        public bool IsDebug { get; set; } = false;

        [JsonPropertyName("OpenAIKey")]
        public string? OpenAIKey { get; set; } = string.Empty;
    }
}
