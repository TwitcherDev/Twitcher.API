namespace Twitcher.API.Models;

/// <param name="Message">The whisper message to send. The message must not be empty</param>
public record SendWhisperRequestBody(string Message);
