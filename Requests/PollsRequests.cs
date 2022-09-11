namespace Twitcher.API.Requests;

/// <summary>Extension methods with polls requests</summary>
public static class PollsRequests
{
    /// <summary>Get information about all polls or specific polls for a Twitch channel. Poll information is available for 90 days.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadPolls"/>' or '<inheritdoc cref="Scopes.ChannelManagePolls"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running polls. Must match the User ID in <paramref name="api"/></param>
    /// <param name="first">Maximum number of objects to return. Maximum: 20</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<PollMetadata[]?>> GetPolls(this TwitcherAPI api, string broadcasterId, int first = 20, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/polls", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterOrDefault("first", first, 20)
            .AddQueryParameterOrDefault("after", after);

        var response = await api.APIRequest<DataPaginationResponse<PollMetadata[]?>>(request);
        return response.Data!;
    }

    /// <summary>Get information about poll by <paramref name="id"/>. Poll information is available for 90 days.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadPolls"/>' or '<inheritdoc cref="Scopes.ChannelManagePolls"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running polls. Must match the User ID in <paramref name="api"/></param>
    /// <param name="id">ID of a poll</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PollMetadata?> GetPolls(this TwitcherAPI api, string broadcasterId, Guid id)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/polls", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameter("id", id);

        var response = await api.APIRequest<DataResponse<PollMetadata[]?>>(request);
        return response.Data!.Data?.SingleOrDefault();
    }

    /// <summary>Get information about polls by <paramref name="ids"/>. Poll information is available for 90 days.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadPolls"/>' or '<inheritdoc cref="Scopes.ChannelManagePolls"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running polls. Must match the User ID in <paramref name="api"/></param>
    /// <param name="ids">IDs of polls. Maximum: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PollMetadata[]> GetPolls(this TwitcherAPI api, string broadcasterId, IEnumerable<Guid> ids)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/polls", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParametersNotEmpty("id", ids);

        var response = await api.APIRequest<DataResponse<PollMetadata[]?>>(request);
        return response.Data!.Data ?? Array.Empty<PollMetadata>();
    }

    /// <summary>Create a poll for a specific Twitch channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManagePolls"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running poll. Must match the User ID in <paramref name="api"/></param>
    /// <param name="title">Question displayed for the poll. Maximum: 60 characters</param>
    /// <param name="choices">Array of the poll choices. Minimum: 2 choices. Maximum: 5 choices. Maximum 25 characters per choice</param>
    /// <param name="duration">Total duration for the poll (in seconds). Minimum: 15. Maximum: 1800</param>
    /// <param name="channelPointsPerVote">Number of Channel Points required to vote once with Channel Points. Minimum: 0. Maximum: 1000000. If <see langword="null"/>, then off</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<PollMetadata> CreatePoll(this TwitcherAPI api, string broadcasterId, string title, IEnumerable<string> choices, int duration, int? channelPointsPerVote = null)
    {
        ArgumentNullException.ThrowIfNull(broadcasterId);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(choices);

        var body = new CreatePollBody(broadcasterId, title, choices.Select(t => new CreatePollChoice(t)).ToArray(), duration,
            channelPointsPerVote != null,
            channelPointsPerVote);

        return CreatePoll(api, body);
    }

    /// <summary>Create a poll for a specific Twitch channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManagePolls"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="body">Request body</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PollMetadata> CreatePoll(this TwitcherAPI api, CreatePollBody body)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/polls", Method.Post)
            .AddBodyNotNull(body);

        var response = await api.APIRequest<DataResponse<PollMetadata[]>>(request);
        return response.Data!.Data.Single();
    }

    /// <summary>End a poll that is currently active.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManagePolls"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running polls. Must match the User ID in <paramref name="api"/></param>
    /// <param name="id">ID of the poll</param>
    /// <param name="status">The poll status to be set. Valid values: <see cref="PollStatus.Terminated"/>, <see cref="PollStatus.Archived"/></param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PollMetadata> EndPoll(this TwitcherAPI api, string broadcasterId, Guid id, PollStatus status)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);

        var request = new RestRequest("helix/polls", Method.Patch)
            .AddBodyNotNull(new EndPollBody(broadcasterId, id, status));

        var response = await api.APIRequest<DataResponse<PollMetadata[]>>(request);
        return response.Data!.Data.Single();
    }
}
