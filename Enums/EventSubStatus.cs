namespace Twitcher.API.Enums;

/// <summary>Event sub status</summary>
public enum EventSubStatus
{
    /// <summary>None</summary>
    None,
    /// <summary>The subscription is enabled</summary>
    Enabled,
    /// <summary>The subscription is pending verification of the specified callback URL</summary>
    WebhookCallbackVerificationPending,
    /// <summary>The specified callback URL failed verification</summary>
    WebhookCallbackVerificationFailed,
    /// <summary>The notification delivery failure rate was too high</summary>
    NotificationFailuresExceeded,
    /// <summary>The authorization was revoked for one or more users specified in the Condition object</summary>
    AuthorizationRevoked,
    /// <summary>One of the users specified in the Condition object was removed</summary>
    UserRemoved
}
