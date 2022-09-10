using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Twitcher.API.Requests;

/// <summary>A class with extension methods for requesting soundtracks</summary>
public static class SoundtrackRequests
{
    /// <summary>Gets the Soundtrack track that the broadcaster is playing</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="broadcasterId">The ID of the broadcaster that’s playing a Soundtrack track</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<SoundtrackCurrentTrack> GetSoundtrackCurrentTrack(this TwitcherAPI api, string broadcasterId)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(broadcasterId);

        var request = new RestRequest("helix/soundtrack/current_track", Method.Get)
            .AddQueryParameter("broadcaster_id", broadcasterId);

        var response = await api.APIRequest<DataResponse<SoundtrackCurrentTrack[]>>(request);
        return response.Data!.Data.Single();
    }

    /// <summary>Gets the tracks of a Soundtrack playlist</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">The ID of the Soundtrack playlist to get</param>
    /// <param name="first">The maximum number of tracks to return for this playlist in the response. The minimum number of tracks is 1 and the maximum is 100</param>
    /// <param name="after">The cursor used to get the next page of tracks for this playlist</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<DataPaginationResponse<SoundtrackTrack[]>> GetSoundtrackPlaylist(this TwitcherAPI api, string id, int first = 20, string? after = null)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(id);

        var request = new RestRequest("helix/soundtrack/playlist", Method.Get)
            .AddQueryParameter("id", id);

        if (first != 20)
            request.AddQueryParameter("first", first);

        if (after != null)
            request.AddQueryParameter("after", after);

        var response = await api.APIRequest<DataPaginationResponse<SoundtrackTrack[]>>(request);
        return response.Data!;
    }

    /// <summary>Gets a list of Soundtrack playlists</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<SoundtrackPlaylistMetadata[]> GetSoundtrackPlaylists(this TwitcherAPI api)
    {
        ArgumentNullException.ThrowIfNull(api);

        var request = new RestRequest("helix/soundtrack/playlists", Method.Get);

        var response = await api.APIRequest<DataResponse<SoundtrackPlaylistMetadata[]>>(request);
        return response.Data!.Data;
    }

    /// <summary>Gets a Soundtrack playlist by id</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="id">The ID of the Soundtrack playlist to get</param>
    /// <returns>Response</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task<SoundtrackPlaylistMetadata?> GetSoundtrackPlaylists(this TwitcherAPI api, string id)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(id);

        var request = new RestRequest("helix/soundtrack/playlists", Method.Get)
        .AddQueryParameter("id", id);

        var response = await api.APIRequest<DataResponse<SoundtrackPlaylistMetadata[]>>(request);
        return response.Data!.Data.SingleOrDefault();
    }
}