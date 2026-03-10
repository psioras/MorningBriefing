using DotNetEnv;
using MorningBriefing.Services;
using MorningBriefing;

// Load .env file when running locally (ignored if vars already set, e.g. GitHub Actions)
DotNetEnv.Env.TraversePath().Load();

Console.WriteLine($"Morning Briefing starting...");

try
{
  var dataChunks = new List<string>();


  var weather = await OpenWeatherService.GetSummaryAsync();
  dataChunks.Add(weather);
  Console.WriteLine($"✅ Weather fetched");

  Console.WriteLine($"D E B U G : Debbugging weather data and OpenWeather response:");
  Console.WriteLine($"Collected data: {weather}");
  Console.WriteLine($"dataChunks: {dataChunks}");

  // The idea is, I can pass the raw data to an LLM and come back with a briefing.
  // Currently, I will just join the strings that are coming back from each Service and will send those.
  string rawData = string.Join("\n\n", dataChunks);

  await NtfyService.SendAsync(rawData, AppEnv.Require("NTFY_TOPIC"));

}
catch (Exception ex)
{
  Console.Error.WriteLine($"Error: {ex.Message}");
  Environment.Exit(1);
}

