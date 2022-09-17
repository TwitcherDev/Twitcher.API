namespace Twitcher.API.Requests;

/// <summary>Extension methods with entitlements requests</summary>
public static class EntitlementsRequests
{
    /// <summary>Gets the status of provided code</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="code">The code to get the status of</param>
    /// <param name="userId">Represents a numeric Twitch user ID</param>
    /// <returns>Code status</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<CodeData[]> GetCodeStatus(this TwitcherAPI api, string code, string userId) =>
        (await api.APIRequest<DataResponse<CodeData[]>>("helix/entitlements/codes", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("code", code)
            .AddQueryParameterNotNull("user_id", userId))).Data;

    /// <summary>Gets the status of provided codes</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="codes">The codes to get the status of</param>
    /// <param name="userId">Represents a numeric Twitch user ID</param>
    /// <returns>Array of payloads</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<CodeData[]> GetCodeStatus(this TwitcherAPI api, IEnumerable<string> codes, string userId) =>
        (await api.APIRequest<DataResponse<CodeData[]>>("helix/entitlements/codes", RequestMethod.Get, r => r
            .AddQueryParametersNotEmpty("code", codes)
            .AddQueryParameterNotNull("user_id", userId))).Data;

    /// <summary>Gets a list of entitlements for a given organization that have been granted to a game, user, or both</summary>
    /// <remarks>The client ID associated with the access token must have ownership of the game: Client ID > Organization ID > Game ID</remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">Unique identifier of the entitlement</param>
    /// <param name="userId">A Twitch user ID</param>
    /// <param name="gameId">A Twitch game ID</param>
    /// <param name="status">Fulfillment status used to filter entitlements</param>
    /// <param name="first">Maximum number of entitlements to return. Max: 1000</param>
    /// <param name="after">The cursor used to fetch the next page of data</param>
    /// <returns>Array of entitlements</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<EntitlementDropData[]?>> GetDropsEntitlements(this TwitcherAPI api, string id, string? userId = null, string? gameId = null, FulfillmentStatus? status = null, int first = 20, string? after = null) =>
        await api.APIRequest<DataPaginationResponse<EntitlementDropData[]?>>("helix/entitlements/drops", RequestMethod.Get, r => r
            .AddQueryParameterOrDefault("id", id)
            .AddQueryParameterOrDefault("user_id", userId)
            .AddQueryParameterOrDefault("game_id", gameId)
            .AddQueryParameterOrDefault("fulfillment_status", status)
            .AddQueryParameterOrDefault("first", first, 20)
            .AddQueryParameterOrDefault("after", after));

    /// <summary>Updates the fulfillment status on a set of Drops entitlements, specified by their entitlement IDs</summary>
    /// <remarks>The client ID associated with the access token must have ownership of the game: Client ID > Organization ID > Game ID</remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="entitlementIds">An array of unique identifiers of the entitlements to update. Maximum: 100</param>
    /// <param name="status">A fulfillment status</param>
    /// <returns>Array of entitlement update statuses</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UpdateDropsResponse[]> UpdateDropsEntitlements(this TwitcherAPI api, IEnumerable<string> entitlementIds, FulfillmentStatus status) =>
        (await api.APIRequest<DataResponse<UpdateDropsResponse[]>>("helix/entitlements/drops", RequestMethod.Patch, r => r
            .AddBodyNotNull(new UpdateDropsEntitlementsRequestBody(entitlementIds, status))))
            .Data;


    /// <summary>Redeems redemption codes</summary>
    /// <remarks>Requires an App access token. Only client IDs approved by Twitch may redeem codes on behalf of any Twitch user account</remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="codes">The redemption codes to redeem. Maximum: 20</param>
    /// <param name="userId">The ID of the user that owns the redemption code to redeem</param>
    /// <returns>The list of redeemed codes</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<RedeemCodeData[]> RedeemCode(this TwitcherAPI api, IEnumerable<string> codes, string userId) =>
        (await api.APIRequest<DataResponse<RedeemCodeData[]>>("helix/entitlements/codes", RequestMethod.Post, r => r
            .AddQueryParametersNotEmpty("code", codes)
            .AddQueryParameterOrDefault("user_id", userId)))
            .Data;

}
