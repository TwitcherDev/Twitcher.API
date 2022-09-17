namespace Twitcher.API.Models;

/// <param name="Type">The type of subscription to create</param>
/// <param name="Version">The version of the subscription type used in this request</param>
/// <param name="Condition">The parameter values that are specific to the specified subscription type</param>
/// <param name="Transport">The transport details, such as the transport method and callback URL, that you want Twitch to use when sending you notifications</param>
public record CreateEventSubRequestBody(string Type, string Version, Dictionary<string, string> Condition, EventSubTransport Transport);

/// <param name="Method">The transport method. Supported values: 'webhook'</param>
/// <param name="Callback">The callback URL where the notification should be sent</param>
/// <param name="Secret">The secret used for verifying a signature</param>
public record EventSubTransport(string Method, string Callback, string Secret);

/// <param name="Id">An ID that identifies the subscription</param>
/// <param name="Status">The status of the create subscription request</param>
/// <param name="Type">The type of subscription</param>
/// <param name="Version">The version of the subscription type</param>
/// <param name="Condition">The parameter values for the subscription type</param>
/// <param name="CreatedAt">Timestamp indicating when the subscription was created</param>
/// <param name="Transport">The transport details used to send you notifications</param>
/// <param name="Cost">The amount that the subscription counts against your limit</param>
public record EventSubData(Guid Id, EventSubStatus Status, string Type, string Version, Dictionary<string, string> Condition, DateTime CreatedAt, EventSubTransportMetadata Transport, int Cost);

/// <param name="Method">The transport method. Supported values: 'webhook'</param>
/// <param name="Callback">The callback URL where the notification should be sent</param>
public record EventSubTransportMetadata(string Method, string Callback);

/// <param name="Data">Response data</param>
/// <param name="Total">The total number of subscriptions you've created</param>
/// <param name="TotalCost">The sum of all of your subscription costs</param>
/// <param name="MaxTotalCost">The maximum total cost that you may incur for all subscriptions you create</param>
/// <param name="Pagination">A cursor value, to be used in a subsequent request to specify the starting point of the next set of results</param>
public record EventSubResponseBody(EventSubData[] Data, int Total, int TotalCost, int MaxTotalCost, Pagination Pagination) : DataPaginationResponse<EventSubData[]>(Data, Pagination);
