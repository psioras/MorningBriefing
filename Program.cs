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
  Console.WriteLine($"Weather fetched");

  var wiki = await WikipediaService.GetOnThisDayAsync();
  dataChunks.Add(wiki);
  Console.WriteLine($"Wiki fetched");
  Console.WriteLine($"D E B U G : Wikipedia debug, data coming back:\n"
      + $"{wiki}");

  // The idea is, I can pass the raw data to an LLM and come back with a briefing.
  // Currently, I will just join the strings that are coming back from each Service and will send those to Ntfy for testing.
  // Will look bad for sure though...
  string rawData = string.Join("\n\n", dataChunks);

  await NtfyService.SendAsync(rawData, AppEnv.Require("NTFY_TOPIC"));

}
catch (Exception ex)
{
  Console.Error.WriteLine($"Error: {ex.Message}");
  await NtfyService.SendAsync($"Application crashed!\n " +
      $"Error: {ex.Message} .", AppEnv.Require("NTFY_TOPIC_DEV"));
  Environment.Exit(1);
}

