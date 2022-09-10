namespace Twitcher.API.Requests;

/// <summary>A class with extension methods for requesting users</summary>
public static class UserRequests
{
    /// <summary>Gets information about one or more specified Twitch users. Users are identified by optional user IDs and/or login name. If neither a user ID nor a login name is specified, the user is looked up by Bearer token</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">User ID. Multiple user IDs can be specified. Limit: 100</param>
    /// <param name="logins">User login name. Multiple login names can be specified. Limit: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserResponseBody[]> GetUsers(this TwitcherAPI api, IEnumerable<string>? ids = null, IEnumerable<string>? logins = null)
    {
        var request = new RestRequest("helix/users", Method.Get);

        if (ids != null)
            foreach (var id in ids)
                request.AddQueryParameter("id", id);

        if (logins != null)
            foreach (var login in logins)
                request.AddQueryParameter("login", login);

        var response = await api.APIRequest<DataResponse<UserResponseBody[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Updates the description of a user specified by the bearer token.
    /// Required scope: '<inheritdoc cref="Scopes.UserEdit"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="description">User account description</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserResponseBody> UpdateUser(this TwitcherAPI api, string? description = null)
    {
        var request = new RestRequest("helix/users", Method.Put);

        if (description != null)
            request.AddQueryParameter("description", description);

        var response = await api.APIRequest<DataResponse<UserResponseBody[]>>(request);
        return response.Data!.Data.Single();
    }

    /// <summary>Gets information on follow relationships between two Twitch users. At minimum, <paramref name="fromId"/> or <paramref name="toId"/> must be provided for a query to be valid</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="fromId">User ID. The request returns information about users who are being followed by the <paramref name="fromId"/> user</param>
    /// <param name="toId">User ID. The request returns information about users who are following the <paramref name="toId"/> user</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100. Default: 20</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserFollowsResponseBody> GetUsersFollows(this TwitcherAPI api, string? fromId = null, string? toId = null, int first = 20, string? after = null)
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

        var response = await api.APIRequest<UserFollowsResponseBody>(request);
        return response.Data!;
    }

    /// <summary>Gets a specified user’s block list. The list is sorted by when the block occurred in descending order (i.e. most recent block first).
    /// Required scope: '<inheritdoc cref="Scopes.UserReadBlockedUsers"/>' or '<inheritdoc cref="Scopes.UserManageBlockedUsers"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">User ID for a Twitch user</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100. Default: 20</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<UserBlockResponseBody[]>> GetUserBlockList(this TwitcherAPI api, string broadcasterId, int first = 20, string? after = null)
    {
        var request = new RestRequest("helix/users/blocks", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<UserBlockResponseBody[]>>(request);
        return response.Data!;
    }

    /// <summary>Blocks the specified user on behalf of the authenticated user.
    /// Required scope: '<inheritdoc cref="Scopes.UserManageBlockedUsers"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="targetUserId">User ID of the user to be blocked</param>
    /// <param name="sourceContext">Source context for blocking the user</param>
    /// <param name="reason">Reason for blocking the user</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task BlockUser(this TwitcherAPI api, string targetUserId, SourceContextType? sourceContext = null, ReasonType? reason = null)
    {
        var request = new RestRequest("helix/users/blocks", Method.Put)
            .AddQueryParameter("target_user_id", targetUserId);

        if (sourceContext.HasValue)
            request.AddQueryParameter("source_context", sourceContext.Value.ToString().ToLower());

        if (reason.HasValue)
            request.AddQueryParameter("reason", reason.Value.ToString().ToLower());

        _ = await api.APIRequest(request);
    }

    /// <summary>Unblocks the specified user on behalf of the authenticated user.
    /// Required scope: '<inheritdoc cref="Scopes.UserManageBlockedUsers"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="targetUserId">User ID of the user to be blocked</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task UnblockUser(this TwitcherAPI api, string targetUserId)
    {
        var request = new RestRequest("helix/users/blocks", Method.Delete)
            .AddQueryParameter("target_user_id", targetUserId);

        _ = await api.APIRequest(request);
    }

    /// <summary>Gets a list of all extensions (both active and inactive) for a specified user, identified by a Bearer token.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadBroadcast"/>' or '<inheritdoc cref="Scopes.UserEditBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserExtensionsResponseBody[]> GetUserExtensions(this TwitcherAPI api)
    {
        var request = new RestRequest("helix/users/extensions/list", Method.Get);

        var response = await api.APIRequest<DataResponse<UserExtensionsResponseBody[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Gets information about active extensions installed by a specified user, identified by a user ID or Bearer token.
    /// Required scope: '<inheritdoc cref="Scopes.UserReadBroadcast"/>' or '<inheritdoc cref="Scopes.UserEditBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="userId">ID of the user whose installed extensions will be returned</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserActiveExtensionsResponseBody> GetUserActiveExtensions(this TwitcherAPI api, string? userId = null)
    {
        var request = new RestRequest("helix/users/extensions", Method.Get);

        if (userId != null)
            request.AddQueryParameter("user_id", userId);

        var response = await api.APIRequest<DataResponse<UserActiveExtensionsResponseBody>>(request);
        return response.Data!.Data;
    }

    /// <summary>Updates the activation state, extension ID, and/or version number of installed extensions for a specified user, identified by a Bearer token. If you try to activate a given extension under multiple extension types, the last write wins (and there is no guarantee of write order).
    /// Required scope: '<inheritdoc cref="Scopes.UserEditBroadcast"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="data">Data for update</param>
    /// <returns>Response</returns>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<UserActiveExtensionsResponseBody> UpdateUserExtensions(this TwitcherAPI api, UserActiveExtensionsResponseBody data)
    {
        var request = new RestRequest("helix/users/extensions", Method.Put)
            .AddBody(new DataResponse<UserActiveExtensionsResponseBody>(data));

        var response = await api.APIRequest<DataResponse<UserActiveExtensionsResponseBody>>(request);
        return response.Data!.Data;
    }
}
