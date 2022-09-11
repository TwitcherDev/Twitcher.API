namespace Twitcher.API.Models;

/// <param name="Id">The distinct ID of the event</param>
/// <param name="EventType">Displays hypetrain.{event_name}, currently only hypetrain.progression</param>
/// <param name="EventTimestamp">Timestamp of event</param>
/// <param name="Version">Returns the version of the endpoint</param>
/// <param name="EventData">(See below for the schema)</param>
public record HypeTrainResponseBody(string Id, string EventType, DateTime EventTimestamp, string Version, HypeTrainEventData EventData);

/// <param name="Id">The distinct ID of this Hype Train</param>
/// <param name="BroadcasterId">Channel ID of which Hype Train events the clients are interested in</param>
/// <param name="StartedAt">Timestamp of when this Hype Train started</param>
/// <param name="ExpiresAt">Timestamp of the expiration time of this Hype Train</param>
/// <param name="CooldownEndTime">Timestamp of when another Hype Train can be started again</param>
/// <param name="Level">The highest level (in the scale of 1-5) reached of the Hype Train</param>
/// <param name="Goal">The goal value of the level above</param>
/// <param name="Total">The total score so far towards completing the level goal above</param>
/// <param name="TopContributions">An array of top contribution objects, one object for each type. For example, one object would represent top contributor of BITS, by aggregate, and one would represent top contributor of SUBS by count</param>
/// <param name="LastContribution">An object that represents the most recent contribution</param>
public record HypeTrainEventData(string Id, string BroadcasterId, DateTime StartedAt, DateTime ExpiresAt, DateTime CooldownEndTime, int Level, int Goal, int Total, HypeTrainContributor[] TopContributions, HypeTrainContributor LastContribution);

/// <param name="Total">Total amount contributed. If type is BITS, total represents amounts of bits used. If type is SUBS, total is 500, 1000, or 2500 to represent tier 1, 2, or 3 subscriptions respectively</param>
/// <param name="Type">Identifies the contribution method, either BITS or SUBS</param>
/// <param name="User">ID of the contributing user</param>
public record HypeTrainContributor(int Total, string Type, string User);
