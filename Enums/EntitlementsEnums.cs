namespace Twitcher.API.Enums;


/// <summary>Twitch code status</summary>
public enum CodeStatus
{
    /// <summary>None</summary>
    None,
    /// <summary>Request successfully redeemed this code to the authenticated user's account</summary>
    SuccessfullyRedeemed,
    /// <summary>Code has already been claimed by a Twitch user</summary>
    AlreadyClaimed,
    /// <summary>Code has expired and can no longer be claimed</summary>
    Expired,
    /// <summary>User is not eligible to redeem this code</summary>
    UserNotEligible,
    /// <summary>Code is not valid and/or does not exist in our database</summary>
    NotFound,
    /// <summary>Code is not currently active</summary>
    Inactive,
    /// <summary>Code has not been claimed.This status will only ever be encountered when calling the GET API to get a keys status</summary>
    Unused,
    /// <summary>Code was not properly formatted</summary>
    IncorrectFormat,
    /// <summary>Indicates some internal and/or unknown failure handling this code</summary>
    InternalError
}

/// <summary>The fulfillment status of the entitlement as determined by the game developer</summary>
public enum FulfillmentStatus
{
    /// <summary>None</summary>
    None,
    /// <summary></summary>
    Claimed,
    /// <summary></summary>
    Fulfilled
}

/// <summary>Status code applied to a set of entitlements for the update operation that can be used to indicate partial success</summary>
public enum UpdateDropsEntitlementsStatus
{
    /// <summary>None</summary>
    None,
    /// <summary>Entitlement was successfully updated</summary>
    Success,
    /// <summary>Invalid format for entitlement ID</summary>
    InvalidId,
    /// <summary>Entitlement ID does not exist</summary>
    NotFound,
    /// <summary>Entitlement is not owned by the organization or the user when called with a user OAuth token</summary>
    Unauthorized,
    /// <summary>Indicates the entitlement update operation failed. Errors in the this state are expected to be be transient and should be retried later</summary>
    UpdateFailed
}

/// <summary>The redemption code’s status</summary>
public enum RedeemCodeStatus
{
    /// <summary>None</summary>
    None,
    /// <summary>The code has already been claimed. All codes are single-use</summary>
    AlreadyClaimed,
    /// <summary>The code has expired and can no longer be claimed</summary>
    Expired,
    /// <summary>The code has not been activated</summary>
    Inactive,
    /// <summary>The code is not properly formatted</summary>
    IncorrectFormat,
    /// <summary>An internal or unknown error occurred when accessing the code</summary>
    InternalError,
    /// <summary>The code was not found</summary>
    NotFound,
    /// <summary>Successfully redeemed the code and credited the user's account with the entitlement</summary>
    SuccessfullyRedeemed,
    /// <summary>The code has not been claimed</summary>
    Unused,
    /// <summary>The user is not eligible to redeem this code</summary>
    UserNotEligible
}
