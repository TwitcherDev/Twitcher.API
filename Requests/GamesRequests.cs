namespace Twitcher.API.Requests;

/// <summary>Extension methods with games requests</summary>
public static class GamesRequests
{
    /// <summary>Gets games sorted by number of current viewers on Twitch, most popular first</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="first">Maximum number of objects to return. Maximum: 100</param>
    /// <param name="before">Cursor for backward pagination</param>
    /// <param name="after">Cursor for forward pagination</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<DataPaginationResponse<GameMetadata[]?>> GetTopGames(this TwitcherAPI api, int first = 20, string? before = null, string? after = null) =>
        api.APIRequest<DataPaginationResponse<GameMetadata[]?>>("helix/games/top", RequestMethod.Get, r => r
            .AddQueryParameterOrDefault("first", first, 20)
            .AddQueryParameterOrDefault("before", before)
            .AddQueryParameterOrDefault("after", after));

    /// <summary>Gets game information by game ID or name</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="ids">Game IDs. Maximum: 100</param>
    /// <param name="names">Game names. Maximum: 100</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static Task<DataResponse<GameMetadata[]?>> GetGames(this TwitcherAPI api, IEnumerable<string>? ids = null, IEnumerable<string>? names = null) =>
        api.APIRequest<DataResponse<GameMetadata[]?>>("helix/games", RequestMethod.Get, r => r
            .AddQueryParameters("id", ids)
            .AddQueryParameters("name", names));
}
