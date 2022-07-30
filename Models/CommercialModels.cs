namespace Twitcher.API.Models;

/// <param name="BroadcasterId">ID of the channel requesting a commercial</param>
/// <param name="Length">Desired length of the commercial in seconds. Valid options are 30, 60, 90, 120, 150, 180</param>
public record StartCommercialRequestBody(string BroadcasterId, int Length);

/// <param name="Length">Length of the triggered commercial</param>
/// <param name="Message">Provides contextual information on why the request failed</param>
/// <param name="RetryAfter">Seconds until the next commercial can be served on this channel</param>
public record StartCommercialResponseBody(int Length, string? Message, int RetryAfter);
