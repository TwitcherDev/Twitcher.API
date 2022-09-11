namespace Twitcher.API.Models;

/// <param name="Id">ID of the poll</param>
/// <param name="BroadcasterId">ID of the broadcaster</param>
/// <param name="BroadcasterLogin">Login of the broadcaster</param>
/// <param name="BroadcasterName">Name of the broadcaster</param>
/// <param name="Title">Question displayed for the poll</param>
/// <param name="Choices">Array of the poll choices</param>
/// <param name="BitsVotingEnabled">Indicates if Bits can be used for voting</param>
/// <param name="BitsPerVote">Number of Bits required to vote once with Bits</param>
/// <param name="ChannelPointsVotingEnabled">Indicates if Channel Points can be used for voting</param>
/// <param name="ChannelPointsPerVote">Number of Channel Points required to vote once with Channel Points</param>
/// <param name="Status">Poll status</param>
/// <param name="Duration">Total duration for the poll (in seconds)</param>
/// <param name="StartedAt">UTC timestamp for the poll's start time</param>
/// <param name="EndedAt">UTC timestamp for the poll's end time. Set to <see langword="null"/> if the poll is <see cref="PollStatus.Active"/></param>
public record PollMetadata(Guid Id, string BroadcasterId, string BroadcasterLogin, string BroadcasterName, string Title, PollChoice[] Choices, bool BitsVotingEnabled, int BitsPerVote, bool ChannelPointsVotingEnabled, int ChannelPointsPerVote, PollStatus Status, int Duration, DateTime StartedAt, DateTime? EndedAt);

/// <param name="Id">ID for the choice</param>
/// <param name="Title">Text displayed for the choice</param>
/// <param name="Votes">Total number of votes received for the choice across all methods of voting</param>
/// <param name="ChannelPointsVotes">Number of votes received via Channel Points</param>
/// <param name="BitsVotes">Number of votes received via Bits</param>
public record PollChoice(Guid Id, string Title, int Votes, int ChannelPointsVotes, int BitsVotes);

/// <param name="BroadcasterId">The broadcaster running poll</param>
/// <param name="Title">Question displayed for the poll. Maximum: 60 characters</param>
/// <param name="Choices">Array of the poll choices. Minimum: 2 choices. Maximum: 5 choices</param>
/// <param name="Duration">Total duration for the poll (in seconds). Minimum: 15. Maximum: 1800</param>
/// <param name="ChannelPointsVotingEnabled">Indicates if Channel Points can be used for voting</param>
/// <param name="ChannelPointsPerVote">Number of Channel Points required to vote once with Channel Points. Minimum: 0. Maximum: 1000000</param>
public record CreatePollBody(string BroadcasterId, string Title, CreatePollChoice[] Choices, int Duration, bool? ChannelPointsVotingEnabled = null, int? ChannelPointsPerVote = null);

/// <param name="Title">Text displayed for the choice</param>
public record CreatePollChoice(string Title);

/// <param name="BroadcasterId">The broadcaster running polls</param>
/// <param name="Id">ID of the poll</param>
/// <param name="Status">The poll status to be set. Valid values: <see cref="PollStatus.Terminated"/>, <see cref="PollStatus.Archived"/></param>
public record EndPollBody(string BroadcasterId, Guid Id, PollStatus Status);
