using System.Text.Json;
using MorningBriefing.Models;

namespace MorningBriefing.Services;

public static class WikipediaService
{

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  public static async Task<string> GetOnThisDayAsync()
  {
    var today = DateTime.UtcNow;
    string url = $"https://api.wikimedia.org/feed/v1/wikipedia/en/onthisday/selected/{today.Month:D2}/{today.Day:D2}";

    using var http = new HttpClient();
    // Wikipedia asks for a User-Agent header
    http.DefaultRequestHeaders.UserAgent.ParseAdd("MorningBriefing (personal automation)");

    var response = await http.GetAsync(url);
    response.EnsureSuccessStatusCode();

    var data = JsonSerializer.Deserialize<WikipediaOnThisDayResponse>(await response.Content.ReadAsStringAsync(), JsonOptions)
      ?? throw new InvalidOperationException("Failed to Deserialize Wikipedia response");

    if (data.Selected.Count == 0)
      return $"[Wikipedia] No 'On This Day' events were found.";

    var pick = data.Selected[Random.Shared.Next(data.Selected.Count)];

    return $"[On This Day in {pick.Year}] {pick.Text}";
  }
}
