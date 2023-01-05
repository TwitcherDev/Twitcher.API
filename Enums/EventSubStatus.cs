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
    /// <summary>The moderator that authorized the subscription is no longer one of the broadcaster's moderators</summary>
    ModeratorRemoved,
    /// <summary>One of the users specified in the Condition object was removed</summary>
    UserRemoved,
    /// <summary>The subscribed to subscription type and version is no longer supported</summary>
    VersionRemoved,
    /// <summary>The client closed the connection</summary>
    WebsocketDisconnected,
    /// <summary>The client failed to respond to a ping message</summary>
    WebsocketFailedPingPong,
    /// <summary>The client sent a non-pong message. Clients may only send pong messages (and only in response to a ping message)</summary>
    WebsocketReceivedInboundTraffic,
    /// <summary>The client failed to subscribe to events within the required time</summary>
    WebsocketConnectionUnused,
    /// <summary>The Twitch WebSocket server experienced an unexpected error</summary>
    WebsocketInternalError,
    /// <summary>The Twitch WebSocket server timed out writing the message to the client</summary>
    WebsocketNetworkTimeout,
    /// <summary>The Twitch WebSocket server experienced a network error writing the message to the client</summary>
    WebsocketNetworkError

}
