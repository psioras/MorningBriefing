using System.Text.Json;
using MorningBriefing.Models;

namespace MorningBriefing.Services;

public static class OpenWeatherService
{

  public static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  public static async Task<string> GetSummaryAsync()
  {
    string apiKey = AppEnv.Require("OPENWEATHER_API_KEY");
    string lat = AppEnv.Require("OPENWEATHER_LAT");
    string lon = AppEnv.Require("OPENWEATHER_LON");
    string units = AppEnv.Get("OPENWEATHER_UNITS", "metric"); // metric | imperial

    string url = $"https://api.openweathermap.org/data/2.5/weather" +
                 $"?lat={lat}&lon={lon}&appid={apiKey}&units={units}";

    using var http = new HttpClient();
    var response = await http.GetAsync(url);
    response.EnsureSuccessStatusCode();

    var data = JsonSerializer.Deserialize<OpenWeatherResponse>(await response.Content.ReadAsStringAsync(), JsonOptions)
    ?? throw new InvalidOperationException("Failed to deserialize OpenWeather response.");

    string description = data.Weather.FirstOrDefault()?.Description ?? "N/A";
    string unitSymbol = units == "metric" ? "°C" : "°F";

    return $"[Weather in {data.CityName}] {description}, " +
      $"{data.Main.Temp:F1}{unitSymbol}, feels like {data.Main.FeelsLike}{unitSymbol}, " +
      $"humidity {data.Main.Humidity:F1}%, wind {data.Wind.Speed:F1} m/s";
  }
}
