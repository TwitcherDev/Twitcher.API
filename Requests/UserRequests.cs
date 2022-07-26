namespace Twitcher.API.Requests;

public static class UserRequests
{
    /// <summary>Gets information about one or more specified Twitch users. Users are identified by optional user IDs and/or login name. If neither a user ID nor a login name is specified, the user is looked up by Bearer token</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">User ID. Multiple user IDs can be specified. Limit: 100</param>
    /// <param name="logins">User login name. Multiple login names can be specified. Limit: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<UserResponse[]?> GetUsers(this TwitcherAPI api, IEnumerable<string>? ids = null, IEnumerable<string>? logins = null)
    {
        var request = new RestRequest("helix/users", Method.Get);

        if (ids != null)
            foreach (var id in ids)
                request.AddQueryParameter("id", id);

        if (logins != null)
            foreach (var login in logins)
                request.AddQueryParameter("login", login);

        var response = await api.APIRequest<DataResponse<UserResponse[]>>(request);

        return response.Data?.Data;
    }

    /// <summary>Updates the description of a user specified by the bearer token.
    /// Required scope: '<inheritdoc cref="Scopes.UserEdit"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="description">User account description</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<UserResponse?> UpdateUser(this TwitcherAPI api, string? description = null)
    {
        if (api.Scopes == default || !api.Scopes.Contains(Scopes.UserEdit))
            throw new ScopeRequireException(Scopes.UserEdit);

        var request = new RestRequest("helix/users", Method.Put);

        if (description != null)
            request.AddQueryParameter("description", description);

        var response = await api.APIRequest<DataResponse<UserResponse[]>>(request);

        return response.Data?.Data?.FirstOrDefault();
    }

    /// <summary>Gets information on follow relationships between two Twitch users. At minimum, <paramref name="fromId"/> or <paramref name="toId"/> must be provided for a query to be valid</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="description">User account description</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<UserFollowsResponse?> GetUsersFollows(this TwitcherAPI api, string? fromId = null, string? toId = null, int first = 20, string? after = null)
    {
        var request = new RestRequest("helix/users/follows", Method.Get);

        if (fromId != null)
            request.AddQueryParameter("from_id", fromId);

        if (toId != null)
            request.AddQueryParameter("to_id", toId);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<UserFollowsResponse>(request);

        return response.Data;
    }

    /// <summary>Updates the description of a user specified by the bearer token.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadBlockedUsers"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="description">User account description</param>
    /// <returns>Response</returns>
    /// <exception cref="ScopeRequireException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="DeadTokenException"></exception>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="InternalServerException"></exception>
    public static async Task<UserResponse?> GetUserBlockList(this TwitcherAPI api, string? description = null)
    {
        throw new NotImplementedException();
        if (api.Scopes == default || !api.Scopes.Contains(Scopes.UserEdit))
            throw new ScopeRequireException(Scopes.UserEdit);

        var request = new RestRequest("helix/users", Method.Put);

        if (description != null)
            request.AddQueryParameter("description", description);

        var response = await api.APIRequest<DataResponse<UserResponse[]>>(request);

        return response.Data?.Data?.FirstOrDefault();
    }
}
