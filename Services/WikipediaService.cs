using System.Text.Json;

namespace MorningBriefing.Services;

public static class WikipediaService
{
    public static async Task<string> GetOnThisDayAsync()
    {
        var today = DateTime.UtcNow;
        string url = $"https://api.wikimedia.org/feed/v1/wikipedia/en/onthisday/selected/{today.Month:D2}/{today.Day:D2}";

        using var http = new HttpClient();
        // Wikipedia asks for a User-Agent header
        http.DefaultRequestHeaders.UserAgent.ParseAdd("MorningBriefing/1.0 (personal automation)");

        var response = await http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var events = json.RootElement.GetProperty("selected");

        // Pick a random event from the list
        var list = events.EnumerateArray().ToList();
        if (list.Count == 0)
            return "[Wikipedia] No 'On This Day' event found.";

        var pick = list[Random.Shared.Next(list.Count)];
        int year = pick.GetProperty("year").GetInt32();
        string text = pick.GetProperty("text").GetString() ?? "N/A";

        return $"[On This Day in {year}] {text}";
    }
}