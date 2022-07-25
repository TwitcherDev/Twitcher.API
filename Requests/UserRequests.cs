using RestSharp;
using Twitcher.API.Exceptions;
using Twitcher.API.Models.Responses;

namespace Twitcher.API.Requests;

public static class UserRequests
{
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="DeadTokenException"></exception>
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
}
