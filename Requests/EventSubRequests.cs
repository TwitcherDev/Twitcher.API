namespace Twitcher.API.Requests;

/// <summary>Extension methods with clips requests</summary>
public static class EventSubRequests
{
    /// <summary>Creates an EventSub subscription</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="type">The type of subscription to create</param>
    /// <param name="version">The version of the subscription type used in this request</param>
    /// <param name="condition">The parameter values that are specific to the specified subscription type</param>
    /// <param name="method">The transport method. Supported values: 'webhook'</param>
    /// <param name="callback">The callback URL where the notification should be sent</param>
    /// <param name="secret">The secret used for verifying a signature</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<EventSubResponseBody> CreateEventSubSubscription(this TwitcherAPI api, string type, string version, Dictionary<string, string> condition, string method, string callback, string secret) =>
        CreateEventSubSubscription(api, new CreateEventSubRequestBody(type, version, condition, new EventSubTransport(method, callback, secret)));

    /// <summary>Creates an EventSub subscription</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="body">Request body</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<EventSubResponseBody> CreateEventSubSubscription(this TwitcherAPI api, CreateEventSubRequestBody body) =>
        api.APIRequest<EventSubResponseBody>("helix/eventsub/subscriptions", RequestMethod.Post, r => r
            .AddBody(body));

    /// <summary>Deletes an EventSub subscription</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">The ID of the subscription to delete</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task DeleteEventSubSubscription(this TwitcherAPI api, Guid id) =>
        api.APIRequest("helix/eventsub/subscriptions", RequestMethod.Delete, r => r
            .AddQueryParameter("id", id));

    /// <summary>Gets a list of your EventSub subscriptions. The list is paginated and ordered by the oldest subscription first</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="after">The cursor used to get the next page of results</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<EventSubResponseBody> GetEventSubSubscriptions(this TwitcherAPI api, string? after = null) =>
        api.APIRequest<EventSubResponseBody>("helix/eventsub/subscriptions", RequestMethod.Get, r => r
            .AddQueryParameterOrDefault("after", after));

    /// <summary>Gets a list of your EventSub subscriptions. The list is paginated and ordered by the oldest subscription first</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="status">Filter subscriptions by its status</param>
    /// <param name="after">The cursor used to get the next page of results</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<EventSubResponseBody> GetEventSubSubscriptionsByStatus(this TwitcherAPI api, EventSubStatus status, string? after = null) =>
        api.APIRequest<EventSubResponseBody>("helix/eventsub/subscriptions", RequestMethod.Get, r => r
            .AddQueryParameter("status", status)
            .AddQueryParameterOrDefault("after", after));

    /// <summary>Gets a list of your EventSub subscriptions. The list is paginated and ordered by the oldest subscription first</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="type">Filter subscriptions by subscription type</param>
    /// <param name="after">The cursor used to get the next page of results</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<EventSubResponseBody> GetEventSubSubscriptionsByType(this TwitcherAPI api, string type, string? after = null) =>
        api.APIRequest<EventSubResponseBody>("helix/eventsub/subscriptions", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("type", type)
            .AddQueryParameterOrDefault("after", after));

    /// <summary>Gets a list of your EventSub subscriptions. The list is paginated and ordered by the oldest subscription first</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">Filter subscriptions by user ID</param>
    /// <param name="after">The cursor used to get the next page of results</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<EventSubResponseBody> GetEventSubSubscriptionsByUserId(this TwitcherAPI api, string userId, string? after = null) =>
        api.APIRequest<EventSubResponseBody>("helix/eventsub/subscriptions", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("user_id", userId)
            .AddQueryParameterOrDefault("after", after));

}
