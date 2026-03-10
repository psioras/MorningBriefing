namespace MorningBriefing;

/// <summary>
/// Thin wrapper around environment variables.
/// </summary>
internal static class AppEnv
{
  /// <summary>Returns the value or throws a clear error if missing.</summary>
  public static string Require(string key)
  {
    var value = Environment.GetEnvironmentVariable(key);
    if (string.IsNullOrWhiteSpace(value))
      throw new InvalidOperationException(
          $"Required environment variable '{key}' is not set. " +
          $"Add it to your .env file (local) or GitHub secrets.");
    return value;
  }

  /// <summary>Returns the value or a fallback default.</summary>
  public static string? Get(string key, string? defaultValue = null)
      => Environment.GetEnvironmentVariable(key) is { Length: > 0 } v ? v : defaultValue;
}
