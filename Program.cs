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
  Console.WriteLine($"Wikipedia 'On This Day' fetched");

  string rawData = string.Join("\n\n", dataChunks);

  var LLMResponse = await LLMService.GetResponseAsync(rawData);
  Console.WriteLine($"D E B U G : LLM response generated: {LLMResponse}");
  await NtfyService.SendAsync(LLMResponse, AppEnv.Require("NTFY_TOPIC"));

}
catch (Exception ex)
{
  Console.Error.WriteLine($"Error: {ex.Message}");
  await NtfyService.SendAsync($"Application crashed!\n " +
      $"Error: {ex.Message} .", AppEnv.Require("NTFY_TOPIC_DEV"));
  Environment.Exit(1);
}

