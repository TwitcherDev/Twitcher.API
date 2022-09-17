namespace Twitcher.API.Models;

/// <param name="Code">Code</param>
/// <param name="Status">Code status</param>
public record CodeData(string Code, CodeStatus Status);

/// <param name="Id">Unique identifier of the entitlement</param>
/// <param name="BenefitId">Identifier of the benefit</param>
/// <param name="Timestamp">UTC timestamp when this entitlement was granted on Twitch</param>
/// <param name="UserId">Twitch user ID of the user who was granted the entitlement</param>
/// <param name="GameId">Twitch game ID of the game that was being played when this benefit was entitled</param>
/// <param name="FulfillmentStatus">The fulfillment status of the entitlement as determined by the game developer</param>
/// <param name="UpdatedAt">UTC timestamp when this entitlement was last updated</param>
public record EntitlementDropData(string Id, string BenefitId, DateTime Timestamp, string UserId, string GameId, FulfillmentStatus FulfillmentStatus, DateTime UpdatedAt);

/// <param name="EntitlementIds">An array of unique identifiers of the entitlements to update. Maximum: 100</param>
/// <param name="FulfillmentStatus">A fulfillment status</param>
public record UpdateDropsEntitlementsRequestBody(IEnumerable<string> EntitlementIds, FulfillmentStatus FulfillmentStatus);

/// <param name="Status">Status code applied to a set of entitlements for the update operation that can be used to indicate partial success</param>
/// <param name="Ids">Array of unique identifiers of the entitlements for the specified status</param>
public record UpdateDropsResponse(UpdateDropsEntitlementsStatus Status, string[] Ids);

/// <param name="Code">The redemption code</param>
/// <param name="Status">The redemption code’s status</param>
public record RedeemCodeData(string Code, RedeemCodeStatus Status);
