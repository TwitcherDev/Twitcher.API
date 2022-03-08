using RestSharp;
using System.Net;
using Twitcher.API.Exceptions;
using Twitcher.API.Models.Responses;

namespace Twitcher.API
{
    public class TwitcherAPI
    {
        private bool _changed;
        private string _accessToken;
        private string _refreshToken;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly RestClient _idClient;
        private readonly RestClient _apiClient;

        public bool IsChanged => _changed;
        /// <summary>Get tokens and set IsChanged false</summary>
        /// <returns>access:refresh</returns>
        public string SaveTokens()
        {
            _changed = false;
            return _accessToken + ':' + _refreshToken;
        }
        public string AccessToken => _accessToken;

        public TwitcherAPI(string tokens, string clientId, string clientSecret, RestClient idClient, RestClient apiClient)
        {
            if (string.IsNullOrEmpty(tokens))
                throw new ArgumentNullException(nameof(tokens));
            
            var s = tokens.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2)
                throw new ArgumentException("Tokens format: <access>:<refresh>", nameof(tokens));

            _changed = false;
            _accessToken = s[0];
            _refreshToken = s[1];
            _clientId = clientId;
            _clientSecret = clientSecret;
            _idClient = idClient;
            _apiClient = apiClient;
        }

        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        public async Task Refresh()
        {
            var request = new RestRequest("oauth2/token", Method.Post)
                .AddQueryParameter("grant_type", "refresh_token")
                .AddQueryParameter("client_id", _clientId)
                .AddQueryParameter("client_secret", _clientSecret)
                .AddQueryParameter("refresh_token", _refreshToken);

            var response = await _idClient.ExecuteAsync<RefreshResponse>(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new DeadTokenException();

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new InternalServerException();

            if (response.Data != default)
            {
                _changed = true;
                _accessToken = response.Data.AccessToken;
                _refreshToken = response.Data.RefreshToken;
            }
        }

        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        public async Task<RestResponse<TResult>> APIRequest<TResult>(RestRequest request)
        {
            request.AddHeader("Client-Id", _clientId);
            request.AddHeader("Authorization", "Bearer " + _accessToken);

            var response = await _apiClient.ExecuteAsync<TResult>(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Refresh();
                request.AddOrUpdateHeader("Authorization", "Bearer " + _accessToken);
                response = await _apiClient.ExecuteAsync<TResult>(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new DeadTokenException();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new BadRequestException();

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new InternalServerException();

            return response;
        }
    }
}