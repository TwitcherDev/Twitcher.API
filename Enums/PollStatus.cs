namespace Twitcher.API.Enums;

/// <summary>Poll status</summary>
public enum PollStatus
{
    /// <summary>Something went wrong determining the state</summary>
    Invalid,
    /// <summary>Poll is currently in progress</summary>
    Active,
    /// <summary>Poll has reached its ended_at time</summary>
    Completed,
    /// <summary>Poll has been manually terminated before its ended_at time</summary>
    Terminated,
    /// <summary>Poll is no longer visible on the channel</summary>
    Archived,
    /// <summary>Poll is no longer visible to any user on Twitch</summary>
    Moderated
}
