using RestSharp;
using Twitcher.API.Exceptions;
using Twitcher.API.Models.Responses;

namespace Twitcher.API.Requests
{
    public static class StreamRequests
    {
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        public static async Task<StreamResponse[]?> GetStreams(this TwitcherAPI api, int first = 20, IEnumerable<string>? userIds = null, IEnumerable<string>? userLogins = null)
        {
            var request = new RestRequest("helix/users", Method.Get);

            if (first != 20)
                request.AddQueryParameter("first", first);

            if (userIds != null)
                foreach (var id in userIds)
                    request.AddQueryParameter("id", id);

            if (userLogins != null)
                foreach (var login in userLogins)
                    request.AddQueryParameter("login", login);

            var response = await api.APIRequest<DataResponse<StreamResponse[]>>(request);

            return response.Data?.Data;
        }
    }
}
