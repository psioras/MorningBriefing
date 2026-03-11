using System.Text.Json.Serialization;

namespace MorningBriefing.Models;

public class LLMResponse
{
  [JsonPropertyName("choices")]
  public List<LLMChoice> Choices { get; set; } = [];
}

public class LLMChoice
{
  [JsonPropertyName("message")]
  public LLMMessage Message { get; set; } = new();
}

public class LLMMessage
{
  [JsonPropertyName("content")]
  public string Content { get; set; } = string.Empty;
}
