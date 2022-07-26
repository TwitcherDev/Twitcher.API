namespace Twitcher.API.Models;

/// <param name="ExtensionId">ID of the extension whose analytics data is being provided</param>
/// <param name="StartedAt">Report start date/time</param>
/// <param name="EndedAt">Report end date/time</param>
/// <param name="Type">Type of report</param>
/// <param name="URL">URL to the downloadable CSV file containing analytics data. Valid for 5 minutes</param>
public record ExtensionAnalyticsResponse(string ExtensionId, DateTime EndedAt, DateTime StartedAt, string Type, [property: JsonProperty("URL")] string URL);

/// <param name="GameId">ID of the game whose analytics data is being provided</param>
/// <param name="StartedAt">Report start date/time</param>
/// <param name="EndedAt">Report end date/time</param>
/// <param name="Type">Type of report</param>
/// <param name="URL">URL to the downloadable CSV file containing analytics data. Valid for 5 minutes</param>
public record GameAnalyticsResponse(string GameId, DateTime EndedAt, DateTime StartedAt, string Type, [property: JsonProperty("URL")] string URL);
