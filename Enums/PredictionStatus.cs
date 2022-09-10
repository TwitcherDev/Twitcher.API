namespace Twitcher.API.Enums;

public enum PredictionStatus
{
    /// <summary>No value</summary>
    None,
    /// <summary>A winning outcome has been chosen and the Channel Points have been distributed to the users who guessed the correct outcome</summary>
    Resolved,
    /// <summary>The Prediction is active and viewers can make predictions</summary>
    Active,
    /// <summary>The Prediction has been canceled and the Channel Points have been refunded to participants</summary>
    Canceled,
    /// <summary>The Prediction has been locked and viewers can no longer make predictions</summary>
    Locked
}
