namespace Twitcher.API.Models;

#region Rewards
/// <param name="Title">The title of the reward</param>
/// <param name="Cost">The cost of the reward</param>
/// <param name="Prompt">The prompt for the viewer when redeeming the reward</param>
/// <param name="IsEnabled">Is the reward currently enabled, if false the reward won’t show up to viewers</param>
/// <param name="BackgroundColor">Custom background color for the reward. Format: Hex with # prefix</param>
/// <param name="IsUserInputRequired">Does the user need to enter information when redeeming the reward</param>
/// <param name="IsMaxPerStreamEnabled">Whether a maximum per stream is enabled</param>
/// <param name="MaxPerStream">The maximum number per stream if enabled</param>
/// <param name="IsMaxPerUserPerStreamEnabled">Whether a maximum per user per stream is enabled</param>
/// <param name="MaxPerUserPerStream">The maximum number per user per stream if enabled</param>
/// <param name="IsGlobalCooldownEnabled">Whether a cooldown is enabled</param>
/// <param name="GlobalCooldownSeconds">The cooldown in seconds if enabled</param>
/// <param name="ShouldRedemptionsSkipRequestQueue">Should redemptions be set to <see cref="RedemptionStatus.FULFILLED" /> status immediately when redeemed and skip the request queue instead of the normal <see cref="RedemptionStatus.UNFULFILLED" /> status</param>
public record CustomRewardRequestBody(string? Title = null, int Cost = default, string? Prompt = null, bool? IsEnabled = null, string? BackgroundColor = null, bool? IsUserInputRequired = null,
    bool? IsMaxPerStreamEnabled = null, int MaxPerStream = default,
    bool? IsMaxPerUserPerStreamEnabled = null, int MaxPerUserPerStream = default,
    bool? IsGlobalCooldownEnabled = null, int GlobalCooldownSeconds = default,
    bool? ShouldRedemptionsSkipRequestQueue = null);

/// <param name="BroadcasterId">ID of the channel the reward is for</param>
/// <param name="BroadcasterLogin">Broadcaster user login name</param>
/// <param name="BroadcasterName">Display name of the channel the reward is for</param>
/// <param name="Id">ID of the reward</param>
/// <param name="Title">The title of the reward</param>
/// <param name="Prompt">The prompt for the viewer when they are redeeming the reward</param>
/// <param name="Cost">The cost of the reward</param>
/// <param name="Image">Set of custom images of 1x, 2x and 4x sizes for the reward, can be null if no images have been uploaded</param>
/// <param name="DefaultImage">Set of default images of 1x, 2x and 4x sizes for the reward</param>
/// <param name="BackgroundColor">Custom background color for the reward. Format: Hex with # prefix</param>
/// <param name="IsEnabled">Is the reward currently enabled, if false the reward won’t show up to viewers</param>
/// <param name="IsUserInputRequired">Does the user need to enter information when redeeming the reward</param>
/// <param name="MaxPerStreamSetting">Whether a maximum per stream is enabled and what the maximum is</param>
/// <param name="MaxPerUserPerStreamSetting">Whether a maximum per user per stream is enabled and what the maximum is</param>
/// <param name="GlobalCooldownSetting">Whether a cooldown is enabled and what the cooldown is</param>
/// <param name="IsPaused">Is the reward currently paused, if true viewers can’t redeem</param>
/// <param name="IsInStock">Is the reward currently in stock, if false viewers can’t redeem</param>
/// <param name="ShouldRedemptionsSkipRequestQueue">Should redemptions be set to <see cref="RedemptionStatus.FULFILLED" /> status immediately when redeemed and skip the request queue instead of the normal <see cref="RedemptionStatus.UNFULFILLED" /> status</param>
/// <param name="RedemptionsRedeemedCurrentStream">The number of redemptions redeemed during the current live stream. 0 if the broadcasters stream isn’t live or max_per_stream_setting isn’t enabled</param>
/// <param name="CooldownExpiresAt">Time of the cooldown expiration. Null if the reward isn’t on cooldown</param>
public record CustomRewardResponseBody(string BroadcasterId, string BroadcasterLogin, string BroadcasterName, Guid Id, string Title, string? Prompt, int Cost,
    CustomRewardImages Image, CustomRewardImages DefaultImage, string BackgroundColor, bool IsEnabled, bool IsUserInputRequired,
    CustomRewardMaxPerStreamSettings MaxPerStreamSetting, CustomRewardMaxPerUserPerStreamSettings MaxPerUserPerStreamSetting, CustomRewardGlobalCooldownSettings GlobalCooldownSetting,
    bool IsPaused, bool IsInStock, bool ShouldRedemptionsSkipRequestQueue, int? RedemptionsRedeemedCurrentStream, DateTime? CooldownExpiresAt);

/// <param name="Url_1x">1x size image</param>
/// <param name="Url_2x">2x size image</param>
/// <param name="Url_4x">4x size image</param>
public record CustomRewardImages(string Url_1x, string Url_2x, string Url_4x);

/// <param name="IsEnabled">Whether a maximum per stream is enabled</param>
/// <param name="MaxPerStream">The maximum number per stream if enabled</param>
public record CustomRewardMaxPerStreamSettings(bool IsEnabled, int MaxPerStream);

/// <param name="IsEnabled">Whether a maximum per user per stream is enabled</param>
/// <param name="MaxPerUserPerStream">The maximum number per user per stream if enabled</param>
public record CustomRewardMaxPerUserPerStreamSettings(bool IsEnabled, int MaxPerUserPerStream);

/// <param name="IsEnabled">Whether a cooldown is enabled</param>
/// <param name="GlobalCooldownSeconds">The cooldown in seconds if enabled</param>
public record CustomRewardGlobalCooldownSettings(bool IsEnabled, int GlobalCooldownSeconds);
#endregion

#region Redemptions
/// <param name="BroadcasterId">The id of the broadcaster that the reward belongs to</param>
/// <param name="BroadcasterLogin">Broadcaster user login name</param>
/// <param name="BroadcasterName">The display name of the broadcaster that the reward belongs to</param>
/// <param name="Id">The ID of the redemption</param>
/// <param name="UserId">The ID of the user that redeemed the reward</param>
/// <param name="UserLogin">The login of the user who redeemed the reward</param>
/// <param name="UserName">The display name of the user that redeemed the reward</param>
/// <param name="Reward">Basic information about the Custom Reward that was redeemed at the time it was redeemed</param>
/// <param name="UserInput">The user input provided. Empty string if not provided</param>
/// <param name="Status">Redemption status</param>
/// <param name="RedeemedAt">Time of when the reward was redeemed</param>
public record CustomRewardRedemptionResponseBody(string BroadcasterId, string BroadcasterLogin, string BroadcasterName, Guid Id,
    string UserId, string UserLogin, string UserName, CustomRewardShort Reward, string UserInput, RedemptionStatus Status, DateTime? RedeemedAt);

/// <param name="Id">ID of the reward</param>
/// <param name="Title">The title of the reward</param>
/// <param name="Prompt">The prompt for the viewer when they are redeeming the reward</param>
/// <param name="Cost">The cost of the reward</param>
public record CustomRewardShort(Guid Id, string Title, string? Prompt, int Cost);

/// <param name="Status">The new status to set redemptions to. Updating to <see cref="RedemptionStatus.CANCELED"/> will refund the user their Channel Points</param>
public record CustomRewardRedemptionStatusRequestBody(RedemptionStatus Status);
#endregion
