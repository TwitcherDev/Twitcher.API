namespace Twitcher.API.Requests;

/// <summary>Extension methods with search requests</summary>
public static class SearchRequests
{
    /// <summary>Returns a list of games or categories that match the query via name either entirely or partially</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="query">Search query</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<SearchCategoryMetadata[]>> SearchCategories(this TwitcherAPI api, string query, int first = 20, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(query);

        var request = new RestRequest("helix/search/categories", Method.Get)
            .AddQueryParameter("query", query);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<SearchCategoryMetadata[]>>(request);
        return response.Data!;
    }

    /// <summary>Returns a list of channels (users who have streamed within the past 6 months) that match the query via channel name or description either entirely or partially. Results include both live and offline channels. Online channels will have additional metadata</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="query">Search query</param>
    /// <param name="liveOnly">Filter results for live streams only</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<SearchChannelMetadata[]>> SearchChannels(this TwitcherAPI api, string query, bool liveOnly = false, int first = 20, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(query);

        var request = new RestRequest("helix/search/channels", Method.Get)
            .AddQueryParameter("query", query);

        if (liveOnly != false)
            request.AddQueryParameter("live_only", liveOnly);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<SearchChannelMetadata[]>>(request);
        return response.Data!;
    }
}
