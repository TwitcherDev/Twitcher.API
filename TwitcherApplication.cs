using RestSharp;

namespace Twitcher.API
{
    public class TwitcherApplication
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly RestClient _idClient;
        private readonly RestClient _apiClient;

        public string ClientId => _clientId;

        public TwitcherApplication(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _idClient = new RestClient("https://id.twitch.tv");
            _apiClient = new RestClient("https://api.twitch.tv");
        }

        public TwitcherAPI CreateAPI(string tokens) => new(tokens, _clientId, _clientSecret, _idClient, _apiClient);
    }
}
