namespace Twitcher.API.Exceptions;

/// <summary>Not success status in response</summary>
public class TwitchErrorException : Exception
{
    /// <summary>HTTP Status code</summary>
    public int Status { get; set; }

    public TwitchErrorException(int status, string? error, string? message) :
        base("Error from twitch " + status.ToString() + (error != null ? $" ({error})" : "") + ": " + message ?? "No message")
    {
        Status = status;
    }
}

/// <summary>400 Bad Request status in response</summary>
public class BadRequestException : TwitchErrorException
{
    public BadRequestException(string? message) : base(400, "Bad Request", message) { }
}

/// <summary>401 Unauthorized status in response: token withdrawn by user</summary>
public class UnauthorizedException : TwitchErrorException
{
    public UnauthorizedException(string? message) : base(401, "Unauthorized", message) { }
}

/// <summary>403 Forbidden status in response</summary>
public class ForbiddenException : TwitchErrorException
{
    public ForbiddenException(string? message) : base(403, "Forbidden", message) { }
}

/// <summary>404 Not Found status in response</summary>
public class NotFoundException : TwitchErrorException
{
    public NotFoundException(string? message) : base(404, "Not Found", message) { }
}

/// <summary>500-599 status in response: something bad happened on twitch side</summary>
public class ServerErrorException : TwitchErrorException
{
    public ServerErrorException(int status, string? error, string? message) : base(status, error, message) { }
}
