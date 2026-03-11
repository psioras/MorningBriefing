using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using MorningBriefing.Models;

namespace MorningBriefing.Services
{
  public class LLMService
  {
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
      PropertyNameCaseInsensitive = true
    };

    public static async Task<string> GetResponseAsync(string prompt)
    {
      string apiKey = AppEnv.Require("LLM_API_KEY");
      string model = AppEnv.Get("LLM_MODEL", "meta-llama/Llama-3.2-1B-Instruct");
      string url = AppEnv.Require("LLM_ENDPOINT");

      using var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);

      var requestBody = new
      {
        model = model,
        messages = new[]
              {
            new { role = "system", content = "Use all provided data to produce a concise morning briefing. Output exactly 3 short plain-text lines (max 4 only if strictly needed). Do not invent facts or hallucinate; if data is missing say 'unknown'. Do not use emojis, markdown, bullets, labels, or extra commentary. Keep each line ≤120 characters."},
            new { role = "user", content = prompt }
        },
        max_tokens = 120
      };

      Console.WriteLine($"D E B U G : Sending request to LLM at {url} with model {model}");
      var response = await client.PostAsJsonAsync($"{url}", requestBody);
      response.EnsureSuccessStatusCode();

      var result = await response.Content.ReadFromJsonAsync<LLMResponse>(JsonOptions)
        ?? throw new InvalidOperationException("Failed to deserialize LLM response.");

      string generatedText = result.Choices.FirstOrDefault()?.Message.Content ?? "[LLM] No response generated.";
      return $"{generatedText}";

    }
  }
}
