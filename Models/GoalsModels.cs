namespace Twitcher.API.Models;

/// <param name="Id">An ID that uniquely identifies this goal</param>
/// <param name="BroadcasterId">An ID that uniquely identifies the broadcaster</param>
/// <param name="BroadcasterLogin">The broadcaster's user handle</param>
/// <param name="BroadcasterName">The broadcaster's display name</param>
/// <param name="Type">The type of goal</param>
/// <param name="Description">A description of the goal, if specified. The description may contain a maximum of 40 characters</param>
/// <param name="CurrentAmount">The goal's current value</param>
/// <param name="TargetAmount">The goal's target value</param>
/// <param name="CreatedAt">The UTC timestamp, which indicates when the broadcaster created the goal</param>
public record GoalResponseBody(string Id, string BroadcasterId, string BroadcasterLogin, string BroadcasterName, string Type, string Description, int CurrentAmount, int TargetAmount, DateTime CreatedAt);
