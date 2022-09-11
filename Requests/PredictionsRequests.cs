namespace Twitcher.API.Requests;

/// <summary>Extension methods with predictions requests</summary>
public static class PredictionsRequests
{
    /// <summary>Get information about all Channel Points Predictions or specific Channel Points Predictions for a Twitch channel. Results are ordered by most recent, so it can be assumed that the currently active or locked Prediction will be the first item.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadPredictions"/>' or '<inheritdoc cref="Scopes.ChannelManagePredictions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running Predictions. Must match the User ID in <paramref name="api"/></param>
    /// <param name="first">Maximum number of objects to return. Maximum: 20</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<PointsPrediction[]>> GetPredictions(this TwitcherAPI api, string broadcasterId, int first = 20, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/predictions", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameterDefault("first", first, 20)
            .AddQueryParameterDefault("after", after);

        var response = await api.APIRequest<DataPaginationResponse<PointsPrediction[]>>(request);
        return response.Data!;
    }

    /// <summary>Get information about Channel Points Prediction by <paramref name="id"/>.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadPredictions"/>' or '<inheritdoc cref="Scopes.ChannelManagePredictions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running Predictions. Must match the User ID in <paramref name="api"/></param>
    /// <param name="id">ID of a Prediction</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PointsPrediction?> GetPredictions(this TwitcherAPI api, string broadcasterId, Guid id)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/predictions", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParameter("id", id);

        var response = await api.APIRequest<DataResponse<PointsPrediction[]?>>(request);
        return response.Data!.Data?.SingleOrDefault();
    }

    /// <summary>Get information about Channel Points Predictions by <paramref name="ids"/>.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelReadPredictions"/>' or '<inheritdoc cref="Scopes.ChannelManagePredictions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running Predictions. Must match the User ID in <paramref name="api"/></param>
    /// <param name="ids">IDs of Predictions. Cannot be empty. Maximum: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PointsPrediction[]> GetPredictions(this TwitcherAPI api, string broadcasterId, IEnumerable<Guid> ids)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/predictions", Method.Get)
            .AddQueryParameterNotNull("broadcaster_id", broadcasterId)
            .AddQueryParametersNotEmpty("id", ids);

        var response = await api.APIRequest<DataResponse<PointsPrediction[]?>>(request);
        return response.Data!.Data ?? Array.Empty<PointsPrediction>();
    }

    /// <summary>Create a Channel Points Prediction for a specific Twitch channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManagePredictions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running Predictions</param>
    /// <param name="title">Title for the Prediction. Maximum: 45 characters</param>
    /// <param name="outcomes">The list of possible outcomes for the Prediction. The minimum number of outcomes that you may specify is 2 and the maximum is 10</param>
    /// <param name="predictionWindow">Total duration for the Prediction (in seconds). Minimum: 1. Maximum: 1800</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<PointsPrediction> CreatePrediction(this TwitcherAPI api, string broadcasterId, string title, IEnumerable<string> outcomes, int predictionWindow)
    {
        ArgumentNullException.ThrowIfNull(broadcasterId);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(outcomes);

        var body = new CreatePredictionBody(broadcasterId, title, outcomes.Select(t => new CreatePredictionOutcome(t)).ToArray(), predictionWindow);

        return CreatePrediction(api, body);
    }

    /// <summary>Create a Channel Points Prediction for a specific Twitch channel.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManagePredictions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="body">Request body</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PointsPrediction> CreatePrediction(this TwitcherAPI api, CreatePredictionBody body)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/predictions", Method.Post)
            .AddBodyNotNull(body);

        var response = await api.APIRequest<DataResponse<PointsPrediction[]>>(request);
        return response.Data!.Data.Single();
    }

    /// <summary>Lock, resolve, or cancel a Channel Points Prediction. Active Predictions can be updated to be <see cref="PredictionStatus.Locked"/>, <see cref="PredictionStatus.Resolved"/>, or <see cref="PredictionStatus.Canceled"/>. Locked Predictions can be updated to be <see cref="PredictionStatus.Resolved"/> or <see cref="PredictionStatus.Canceled"/>.
    /// Required scope: '<inheritdoc cref="Scopes.ChannelManagePredictions"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The broadcaster running prediction events</param>
    /// <param name="id">ID of the Prediction</param>
    /// <param name="status">The Prediction status to be set. Valid values: <see cref="PredictionStatus.Resolved"/>, <see cref="PredictionStatus.Canceled"/>, <see cref="PredictionStatus.Locked"/></param>
    /// <param name="winningOutcomeId">ID of the winning outcome for the Prediction. This parameter is required if status is being set to <see cref="PredictionStatus.Resolved"/></param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<PointsPrediction> EndPrediction(this TwitcherAPI api, string broadcasterId, Guid id, PredictionStatus status, Guid? winningOutcomeId = null)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);

        var request = new RestRequest("helix/predictions", Method.Patch)
            .AddBody(new EndPredictionBody(broadcasterId, id, status, winningOutcomeId));

        var response = await api.APIRequest<DataResponse<PointsPrediction[]>>(request);
        return response.Data!.Data.Single();
    }
}