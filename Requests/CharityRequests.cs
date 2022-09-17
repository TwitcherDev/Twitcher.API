namespace Twitcher.API.Requests;

/// <summary>Extension methods with charity requests</summary>
public static class CharityRequests
{
    /// <summary>Gets information about the charity campaign that a broadcaster is running, such as their fundraising goal and the amount that’s been donated so far</summary>
    /// <remarks>Required scope: <see cref="Scopes.ChannelReadCharity"/></remarks>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster that’s actively running a charity campaign</param>
    /// <returns>Single charity campaign or <see langword="null"/> if the broadcaster is not running a charity campaign</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<CharityData?> GetCharityCampaign(this TwitcherAPI api, string broadcasterId) =>
        (await api.APIRequest<DataResponse<CharityData[]>>("helix/charity/campaigns", RequestMethod.Get, r => r
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)))
            .Data.SingleOrDefault();
}
