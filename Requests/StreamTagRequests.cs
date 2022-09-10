namespace Twitcher.API.Requests;

/// <summary>A class with extension methods for requesting streams tags</summary>
public static class StreamTagRequests
{
    /// <summary>Gets the list of all stream tags that Twitch defines</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="first">The maximum number of tags to return per page. Maximum: 100</param>
    /// <param name="after">The cursor used to get the next page of results</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<StreamTagResponseBody[]>> GetAllStreamTags(this TwitcherAPI api, int first = 20, string? after = null)
    {
        var request = new RestRequest("helix/tags/streams", Method.Get);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<StreamTagResponseBody[]>>(request);
        return response.Data!;
    }

    /// <summary>Gets the tag of the stream, by the specified id</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">An ID that identifies a specific tag to return</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<StreamTagResponseBody?> GetAllStreamTags(this TwitcherAPI api, string id)
    {
        var request = new RestRequest("helix/tags/streams", Method.Get)
            .AddQueryParameter("tag_id", id);

        var response = await api.APIRequest<DataResponse<StreamTagResponseBody[]>>(request);
        return response.Data!.Data.SingleOrDefault();
    }

    /// <summary>Gets the tags of the stream, by the specified ids</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">An ID that identifies a specific tag to return. You may specify a maximum of 100 IDs</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<StreamTagResponseBody[]> GetAllStreamTags(this TwitcherAPI api, IEnumerable<string> ids)
    {
        var request = new RestRequest("helix/tags/streams", Method.Get);

        var isAny = false;
        foreach (var id in ids)
        {
            request.AddQueryParameter("tag_id", id);
            isAny = true;
        }
        if (!isAny)
            throw new ArgumentException("Cannot be empty", nameof(ids));

        var response = await api.APIRequest<DataResponse<StreamTagResponseBody[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Gets the list of stream tags that are set on the specified channel</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The user ID of the channel to get the tags from</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<StreamTagResponseBody[]> GetStreamTags(this TwitcherAPI api, string broadcasterId)
    {
        var request = new RestRequest("helix/streams/tags", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        var response = await api.APIRequest<DataResponse<StreamTagResponseBody[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Applies one or more tags to the specified channel, overwriting any existing tags. If the request does not specify tags, all existing tags are removed from the channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManageBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The user ID of the channel to apply the tags to</param>
    /// <param name="ids">A list of IDs that identify the tags to apply to the channel. You may specify a maximum of five tags. To remove all tags from the channel, set <paramref name="ids"/> to an empty array</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task ReplaceStreamTags(this TwitcherAPI api, string broadcasterId, IEnumerable<string>? ids)
    {
        var request = new RestRequest("helix/streams/tags", Method.Put)
            .AddQueryParameter("broadcaster_id", broadcasterId)
            .AddBody(new ReplaceStreamTagsRequestBody(ids));

        _ = await api.APIRequest(request);
    }
}
