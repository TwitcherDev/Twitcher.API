namespace Twitcher.API.Requests;
public static class VideoRequests
{
    /// <summary>Gets video information by one or more video <paramref name="ids"/></summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">ID of the video being queried. Limit: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<VideoResponseBody[]> GetVideos(this TwitcherAPI api, IEnumerable<string> ids)
    {
        var request = new RestRequest("helix/videos", Method.Get);

        foreach (var id in ids)
            request.AddQueryParameter("id", id);

        var response = await api.APIRequest<DataResponse<VideoResponseBody[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Gets video information by <paramref name="userId"/>, or <paramref name="gameId"/>. If a <paramref name="gameId"/> is specified, a maximum of 500 results are available</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">ID of the user who owns the video</param>
    /// <param name="gameId">ID of the game the video is of</param>
    /// <param name="language">Language of the video being queried. Limit: 1. A language value must be either the ISO 639-1 two-letter code</param>
    /// <param name="period">Period during which the video was created</param>
    /// <param name="sort">Sort order of the videos</param>
    /// <param name="type">Type of video</param>
    /// <param name="first">Number of values to be returned. Limit: 100</param>
    /// <param name="before">Cursor for backward pagination</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<VideoResponseBody[]>> GetVideos(this TwitcherAPI api, string? userId = null, string? gameId = null, string? language = null, VideoTimePeriod period = VideoTimePeriod.All, VideoSortOrder sort = VideoSortOrder.Time, VideoType? type = null, int first = 20, string? before = null, string? after = null)
    {
        var request = new RestRequest("helix/videos", Method.Get);

        if (userId != null)
            request.AddQueryParameter("user_id", userId);

        if (gameId != null)
            request.AddQueryParameter("game_id", gameId);

        if (language != null)
            request.AddQueryParameter("language", language);

        if (period != VideoTimePeriod.All)
            request.AddQueryParameter("period", period.ToString().ToLower());

        if (sort != VideoSortOrder.Time)
            request.AddQueryParameter("sort", sort.ToString().ToLower());

        if (type.HasValue)
            request.AddQueryParameter("type", type.Value.ToString().ToLower());

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (before != null)
            request.AddQueryParameter("before", before);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<VideoResponseBody[]>>(request);
        return response.Data!;
    }

    /// <summary>Deletes one or more videos. Videos are past broadcasts, Highlights, or uploads.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageVideos"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">ID of the video(s) to be deleted. Limit: 5</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task DeleteVideos(this TwitcherAPI api, IEnumerable<string> ids)
    {
        var request = new RestRequest("helix/videos", Method.Delete);

        foreach (var id in ids)
            request.AddQueryParameter("id", id);

        _ = await api.APIRequest(request);
    }
}
