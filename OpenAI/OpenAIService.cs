using OpenCLAI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCLAI.OpenAI
{
    public class OpenAIService
    {
        private HttpClient _httpClient = new HttpClient();

        public OpenAIService(string key)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        }

        public async Task<OpenAIResult> SendPromptAsync(string prompt, List<OpenAI.ChatGPT.Message> history)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                return new OpenAIResult(OpenAIResultStatus.Failure, "Message was null.");
            }

            var gptRequest = new OpenAI.ChatGPT.Request()
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<OpenAI.ChatGPT.Message>()
                {
                    new OpenAI.ChatGPT.Message() { Role = "system", Content = "Use short and concise answers." }
                }
            };

            foreach (var message in history)
            {
                gptRequest.Messages.Add(message);
            }

            gptRequest.Messages.Add(new OpenAI.ChatGPT.Message() { Role = "user", Content = prompt });

            var gptJson = JsonSerializer.Serialize(gptRequest);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Content = new StringContent(gptJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new OpenAIResult(OpenAIResultStatus.Failure, $"There was an issue with the response. Status: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var gptResponse = JsonSerializer.Deserialize<OpenAI.ChatGPT.Response>(content);

            if (gptResponse == null)
            {
                return new OpenAIResult(OpenAIResultStatus.Failure, "There was an issue with the web request.");
            }

            if (gptResponse.Choices == null || gptResponse.Choices[0] == null)
            {
                return new OpenAIResult(OpenAIResultStatus.Failure, "Failed to get GPT response choices.");
            }

            if (gptResponse.Choices[0].Message == null)
            {
                return new OpenAIResult(OpenAIResultStatus.Failure, "GPT response Message was null.");
            }

            if (string.IsNullOrEmpty(gptResponse.Choices[0]?.Message?.Content))
            {
                return new OpenAIResult(OpenAIResultStatus.Failure, "GPT response Message content was null.");
            }

            var resultMessage = gptResponse.Choices[0].Message!.Content!;

            return new OpenAIResult(OpenAIResultStatus.Success, gptResponse.Choices[0].Message!.Content!, gptResponse);
        }
    }
}
