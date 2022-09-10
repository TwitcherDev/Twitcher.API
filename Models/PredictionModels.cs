namespace Twitcher.API.Models;

/// <param name="Id">ID of the Prediction</param>
/// <param name="BroadcasterId">ID of the broadcaster</param>
/// <param name="BroadcasterLogin">Login of the broadcaster</param>
/// <param name="BroadcasterName">Name of the broadcaster</param>
/// <param name="Title">Title for the Prediction</param>
/// <param name="WinningOutcomeId">ID of the winning outcome. If the status is <see cref="PredictionStatus.Active"/>, this is set to <see langword="null"/></param>
/// <param name="Outcomes">Array of possible outcomes for the Prediction</param>
/// <param name="PredictionWindow">Total duration for the Prediction (in seconds)</param>
/// <param name="Status">Status of the Prediction</param>
/// <param name="CreatedAt">UTC timestamp for the Prediction's start time</param>
/// <param name="EndedAt">UTC timestamp for when the Prediction ended. If the status is <see cref="PredictionStatus.Active"/>, this is set to <see langword="null"/></param>
/// <param name="LockedAt">UTC timestamp for when the Prediction was locked. If the status is not <see cref="PredictionStatus.Locked"/>, this is set to <see langword="null"/></param>
public record PointsPrediction(Guid Id, string BroadcasterId, string BroadcasterLogin, string BroadcasterName, string Title, Guid? WinningOutcomeId, PredictionOutcome[] Outcomes, int PredictionWindow, PredictionStatus Status, DateTime CreatedAt, DateTime? EndedAt, DateTime? LockedAt);

/// <param name="Id">ID for the outcome</param>
/// <param name="Title">Text displayed for outcome</param>
/// <param name="Users">Number of unique uesrs that chose the outcome</param>
/// <param name="ChannelPoints">Number of Channel Points used for the outcome</param>
/// <param name="TopPredictors">Array of users who were the top predictors. <see langword="null"/> if none</param>
/// <param name="Color">Color for the outcome. If the number of outcomes is two, the color is BLUE for the first one and PINK for the second one. If there are more than two outcomes, the color is BLUE for all of them</param>
public record PredictionOutcome(Guid Id, string Title, int Users, int ChannelPoints, PredictionTopPredictor[]? TopPredictors, string Color);

/// <param name="UserId">ID of the user</param>
/// <param name="UserLogin">Login of the user</param>
/// <param name="UserName">Display name of the user</param>
/// <param name="ChannelPointsUsed">Number of Channel Points used by the user</param>
/// <param name="ChannelPointsWon">Number of Channel Points won by the user</param>
public record PredictionTopPredictor(string UserId, string UserLogin, string UserName, int ChannelPointsUsed, int ChannelPointsWon);

/// <param name="BroadcasterId">The broadcaster running Predictions</param>
/// <param name="Title">Title for the Prediction. Maximum: 45 characters</param>
/// <param name="Outcomes">The list of possible outcomes for the Prediction. The minimum number of outcomes that you may specify is 2 and the maximum is 10</param>
/// <param name="PredictionWindow">Total duration for the Prediction (in seconds). Minimum: 1. Maximum: 1800</param>
public record CreatePredictionBody(string BroadcasterId, string Title, CreatePredictionOutcome[] Outcomes, int PredictionWindow);

/// <param name="Title">Text displayed for the outcome choice. Maximum: 25 characters</param>
public record CreatePredictionOutcome(string Title);

/// <param name="BroadcasterId">The broadcaster running prediction events</param>
/// <param name="Id">ID of the Prediction</param>
/// <param name="Status">The Prediction status to be set. Valid values: <see cref="PredictionStatus.Resolved"/>, <see cref="PredictionStatus.Canceled"/>, <see cref="PredictionStatus.Locked"/></param>]
/// <param name="WinningOutcomeId">ID of the winning outcome for the Prediction. This parameter is required if status is being set to <see cref="PredictionStatus.Resolved"/></param>
public record EndPredictionBody(string BroadcasterId, Guid Id, PredictionStatus Status, Guid? WinningOutcomeId);
