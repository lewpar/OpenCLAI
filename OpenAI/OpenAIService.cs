using OpenCLAI.OpenAI.ChatGPT;
using OpenCLAI.OpenAI.DALLE;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

        public async Task<ChatGPTResult> SendPromptAsync(string prompt, List<OpenAI.ChatGPT.Message> history)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                return new ChatGPTResult(OpenAIResultStatus.Failure, "Message was null.");
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

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(gptJson, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ChatGPTResult(OpenAIResultStatus.Failure, $"There was an issue with the response. Status: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var gptResponse = JsonSerializer.Deserialize<OpenAI.ChatGPT.Response>(content);

            if (gptResponse == null)
            {
                return new ChatGPTResult(OpenAIResultStatus.Failure, "There was an issue with the web request.");
            }

            if (gptResponse.Choices == null || gptResponse.Choices[0] == null)
            {
                return new ChatGPTResult(OpenAIResultStatus.Failure, "Failed to get GPT response choices.");
            }

            if (gptResponse.Choices[0].Message == null)
            {
                return new ChatGPTResult(OpenAIResultStatus.Failure, "GPT response Message was null.");
            }

            if (string.IsNullOrEmpty(gptResponse.Choices[0]?.Message?.Content))
            {
                return new ChatGPTResult(OpenAIResultStatus.Failure, "GPT response Message content was null.");
            }

            return new ChatGPTResult(OpenAIResultStatus.Success, gptResponse.Choices[0].Message!.Content!, gptResponse);
        }

        public async Task<DALLEResult> GetImageAsync(string prompt)
        {
            var request = new DALLE.Request()
            {
                Prompt = prompt,
                Format = "url",
                Count = 1,
                Size = "256x256"
            };

            var json = JsonSerializer.Serialize(request);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/images/generations")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                return new DALLEResult(OpenAIResultStatus.Failure, $"There was an issue with the response. Status: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var imageResponse = JsonSerializer.Deserialize<DALLE.Response>(content);

            if (imageResponse == null || imageResponse.Data == null)
            {
                return new DALLEResult(OpenAIResultStatus.Failure, "There was no image data.");
            }

            var sb = new StringBuilder();
            foreach (var image in imageResponse.Data)
            {
                sb.Append(image.Url);
                sb.Append(';');
            }

            return new DALLEResult(OpenAIResultStatus.Success, sb.ToString(), imageResponse);
        }
    }
}
