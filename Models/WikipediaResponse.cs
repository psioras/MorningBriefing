//Wikipedia Response
using System.Text.Json.Serialization;

namespace MorningBriefing.Models;

public class WikipediaOnThisDayResponse
{
  [JsonPropertyName("selected")]
  public List<WikiEvent> Selected { get; set; } = [];
}

public class WikiEvent
{
  [JsonPropertyName("year")]
  public int Year { get; set; }

  [JsonPropertyName("text")]
  public string Text { get; set; } = string.Empty;
}
