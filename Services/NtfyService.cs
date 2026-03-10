using System.Text;
using MorningBriefing;

namespace MorningBriefing.Services;

public static class NtfyService
{

  public static async Task SendAsync(string message, string topic)
  {
    string baseUrl = AppEnv.Get("NTFY_URL", "https://ntfy.sh");

    using var http = new HttpClient();


    //Create the message
    http.DefaultRequestHeaders.Add("Title", "Morning Briefing");
    http.DefaultRequestHeaders.Add("Priority", "default");
    http.DefaultRequestHeaders.Add("Tags", "sun_with_face");

    var content = new StringContent(message, Encoding.UTF8, "text/plain");
    var response = await http.PostAsync($"{baseUrl}/{topic}", content);
    response.EnsureSuccessStatusCode();
  }
}
