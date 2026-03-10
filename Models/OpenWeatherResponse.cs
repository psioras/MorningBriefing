// OpenWeatherResponse.cs 
using System.Text.Json.Serialization;

namespace MorningBriefing.Models;

public class OpenWeatherResponse
{
  [JsonPropertyName("main")]
  public MainData Main { get; set; } = new();

  [JsonPropertyName("weather")]
  public List<WeatherDescription> Weather { get; set; } = [];

  [JsonPropertyName("wind")]
  public WindData Wind { get; set; } = new();

  [JsonPropertyName("name")]
  public string CityName { get; set; } = string.Empty;
}

public class WeatherDescription
{
  [JsonPropertyName("main")]
  public string Main { get; set; } = string.Empty; // e.x. "Rain"

  [JsonPropertyName("description")]
  public string Description { get; set; } = string.Empty; // e.x. "light rain"
}

public class MainData
{
  [JsonPropertyName("temp")]
  public double Temp { get; set; }

  [JsonPropertyName("feels_like")]
  public double FeelsLike { get; set; }

  [JsonPropertyName("humidity")]
  public int Humidity { get; set; }
}

public class WindData
{
  [JsonPropertyName("speed")]
  public double Speed { get; set; }
}
