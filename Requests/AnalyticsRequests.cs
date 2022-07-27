using Twitcher.API.Exceptions;
using Twitcher.API.Models;

namespace Twitcher.API.Requests;

public static class AnalyticsRequests
{
    #region GetExtensionAnalytics
    /// <summary>Gets a URL that Extension developers can use to download analytics reports (CSV files) for their Extensions. The URL is valid for 5 minutes.
    /// Required scope: '<inheritdoc cref="Scopes.AnalyticsReadExtensions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100. Default: 20</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <param name="endedAt">Ending date/time for returned reports. If this is provided, started_at also must be specified</param>
    /// <param name="startedAt">Starting date/time for returned reports. This must be on or after January 31, 2018. If this is provided, ended_at also must be specified</param>
    /// <returns>Response. If you leave both <paramref name="startedAt" /> and <paramref name="endedAt" /> blank, the API returns the most recent date of data</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<DataPaginationResponse<ExtensionAnalyticsResponse[]>?> GetExtensionAnalytics(this TwitcherAPI api, int first = 20, string? after = null, DateTime startedAt = default, DateTime endedAt = default)
    {
        var request = new RestRequest("helix/analytics/extensions", Method.Get)
            .AddQueryParameter("type", "overview_v2");

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        if (startedAt != default)
            request.AddQueryParameter("started_at", startedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        if (endedAt != default)
            request.AddQueryParameter("ended_at", endedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        var response = await api.APIRequest<DataPaginationResponse<ExtensionAnalyticsResponse[]>>(request);

        return response.Data;
    }

    /// <summary>Gets a URL that Extension developers can use to download analytics reports (CSV files) for their Extensions. The URL is valid for 5 minutes.
    /// Required scope: '<inheritdoc cref="Scopes.AnalyticsReadExtensions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="extensionId">Client ID value assigned to the extension when it is created</param>
    /// <param name="endedAt">Ending date/time for returned reports. If this is provided, started_at also must be specified</param>
    /// <param name="startedAt">Starting date/time for returned reports. This must be on or after January 31, 2018. If this is provided, ended_at also must be specified</param>
    /// <returns>Response for specified extension. If you leave both <paramref name="startedAt" /> and <paramref name="endedAt" /> blank, the API returns the most recent date of data</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<ExtensionAnalyticsResponse?> GetExtensionAnalytics(this TwitcherAPI api, string extensionId, DateTime startedAt = default, DateTime endedAt = default)
    {
        var request = new RestRequest("helix/analytics/extensions", Method.Get)
            .AddQueryParameter("extension_id", extensionId)
            .AddQueryParameter("type", "overview_v2");

        if (startedAt != default)
            request.AddQueryParameter("started_at", startedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        if (endedAt != default)
            request.AddQueryParameter("ended_at", endedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        var response = await api.APIRequest<DataResponse<ExtensionAnalyticsResponse[]>>(request);

        return response.Data?.Data?.FirstOrDefault();
    }
    #endregion

    #region GetGameAnalytics
    /// <summary>Gets a URL that game developers can use to download analytics reports (CSV files) for their games. The URL is valid for 5 minutes.
    /// Required scope: '<inheritdoc cref="Scopes.AnalyticsReadGames"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100. Default: 20</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <param name="endedAt">Ending date/time for returned reports. If this is provided, started_at also must be specified</param>
    /// <param name="startedAt">Starting date/time for returned reports. If this is provided, ended_at also must be specified</param>
    /// <returns>Response for specified game. If you leave both <paramref name="startedAt" /> and <paramref name="endedAt" /> blank, the API returns the most recent date of data</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<DataPaginationResponse<GameAnalyticsResponse[]>?> GetGameAnalytics(this TwitcherAPI api, int first = 20, string? after = null, DateTime startedAt = default, DateTime endedAt = default)
    {
        var request = new RestRequest("helix/analytics/games", Method.Get)
            .AddQueryParameter("type", "overview_v2");

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        if (startedAt != default)
            request.AddQueryParameter("started_at", startedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        if (endedAt != default)
            request.AddQueryParameter("ended_at", endedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        var response = await api.APIRequest<DataPaginationResponse<GameAnalyticsResponse[]>>(request);

        return response.Data;
    }

    /// <summary>Gets a URL that game developers can use to download analytics reports (CSV files) for their games. The URL is valid for 5 minutes.
    /// Required scope: '<inheritdoc cref="Scopes.AnalyticsReadGames"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="gameId">Game ID</param>
    /// <param name="endedAt">Ending date/time for returned reports. If this is provided, started_at also must be specified</param>
    /// <param name="startedAt">Starting date/time for returned reports. If this is provided, ended_at also must be specified</param>
    /// <returns>Response for specified game. If you leave both <paramref name="startedAt" /> and <paramref name="endedAt" /> blank, the API returns the most recent date of data</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<GameAnalyticsResponse?> GetGameAnalytics(this TwitcherAPI api, string gameId, DateTime startedAt = default, DateTime endedAt = default)
    {
        var request = new RestRequest("helix/analytics/games", Method.Get)
            .AddQueryParameter("game_id", gameId)
            .AddQueryParameter("type", "overview_v2");

        if (startedAt != default)
            request.AddQueryParameter("started_at", startedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        if (endedAt != default)
            request.AddQueryParameter("ended_at", endedAt.ToString("yyyy-MM-ddT00:00:00Z"));

        var response = await api.APIRequest<DataResponse<GameAnalyticsResponse[]>>(request);

        return response.Data?.Data?.FirstOrDefault();
    }
    #endregion
}
