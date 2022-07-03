using RestSharp;
using System.Net;
using Twitcher.API.Enums;
using Twitcher.API.Exceptions;
using Twitcher.API.Models.Requests;
using Twitcher.API.Models.Responses;

namespace Twitcher.API.Requests
{
    public static class CustomRewardRequests
    {
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        /// <exception cref="NotAvailableException"></exception>
        public static async Task<CustomRewardResponse?> CreateCustomReward(this TwitcherAPI api, string broadcasterId, CustomRewardModel reward)
        {
            var request = new RestRequest("helix/channel_points/custom_rewards", Method.Post)
                .AddQueryParameter("broadcaster_id", broadcasterId)
                .AddBody(reward);

            var response = await api.APIRequest<DataResponse<CustomRewardResponse[]>>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAvailableException();

            return response.Data?.Data?.FirstOrDefault();
        }

        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        /// <exception cref="NotAvailableException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public static async Task DeleteCustomReward(this TwitcherAPI api, string broadcasterId, string id)
        {
            var request = new RestRequest("helix/channel_points/custom_rewards", Method.Delete)
                .AddQueryParameter("broadcaster_id", broadcasterId)
                .AddQueryParameter("id", id);

            var response = await api.APIRequest<NoContentResponse>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAvailableException();

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException();
        }

        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        /// <exception cref="NotAvailableException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<CustomRewardResponse[]?> GetCustomRewards(this TwitcherAPI api, string broadcasterId, string? id = null)
        {
            var request = new RestRequest("helix/channel_points/custom_rewards", Method.Get)
                .AddQueryParameter("broadcaster_id", broadcasterId);

            if (id != null)
                request.AddQueryParameter("id", id);

            var response = await api.APIRequest<DataResponse<CustomRewardResponse[]>>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAvailableException();

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException();

            return response.Data?.Data;
        }

        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        /// <exception cref="NotAvailableException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<DataPaginationResponse<RedemptionResponse[]>?> GetRedemptionStatus(this TwitcherAPI api, string broadcasterId, string rewardId, int first = 20, string? after = null, RedemptionStatus? status = null)
        {
            var request = new RestRequest("helix/channel_points/custom_rewards/redemptions", Method.Get)
                .AddQueryParameter("broadcaster_id", broadcasterId)
                .AddQueryParameter("reward_id", rewardId);

            if (first != 20)
                request.AddQueryParameter("first", first);

            if (after != null)
                request.AddQueryParameter("after", after);

            if (status != default)
                request.AddQueryParameter("status", status.ToString());

            var response = await api.APIRequest<DataPaginationResponse<RedemptionResponse[]>>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAvailableException();

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException();

            return response.Data;
        }

        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="DeadTokenException"></exception>
        /// <exception cref="InternalServerException"></exception>
        /// <exception cref="NotAvailableException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<RedemptionResponse[]?> UpdateRedemptionStatus(this TwitcherAPI api, string broadcasterId, string rewardId, IEnumerable<string> ids, RedemptionStatus status)
        {
            var request = new RestRequest("helix/channel_points/custom_rewards/redemptions", Method.Patch)
                .AddQueryParameter("broadcaster_id", broadcasterId)
                .AddQueryParameter("reward_id", rewardId)
                .AddBody(new RedemptionStatusModel { Status = status.ToString() });

            foreach (var id in ids)
                request.AddQueryParameter("id", id);

            var response = await api.APIRequest<DataResponse<RedemptionResponse[]>>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAvailableException();

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException();

            return response.Data?.Data;
        }
    }
}
