namespace Twitcher.API.Requests;

public static class WhisperRequests
{
    /// <summary>Sends a whisper message to the specified user.
    /// Required scope: '<inheritdoc cref="Scopes.UserManageWhispers"/>'</summary>
    /// <param name="api">The instance of the api that should request</param>
    /// <param name="fromUserId">The ID of the user sending the whisper. This user must have a verified phone number</param>
    /// <param name="toUserId">The ID of the user to receive the whisper</param>
    /// <param name="message">The whisper message to send. The message must not be empty</param>
    /// <exception cref="NotValidatedException"></exception>
    /// <exception cref="TwitchErrorException"></exception>
    public static async Task SendWhisper(this TwitcherAPI api, string fromUserId, string toUserId, string message)
    {
        var request = new RestRequest("helix/whispers", Method.Post)
            .AddQueryParameter("from_user_id", fromUserId)
            .AddQueryParameter("to_user_id", toUserId)
            .AddBody(new SendWhisperRequestBody(message));

        _ = await api.APIRequest(request);
    }
}
