namespace Twitcher.API.Exceptions;

/// <summary>Any not success status in response</summary>
public class TwitchErrorException : Exception
{
    /// <summary>HTTP Status code</summary>
    public int Status { get; }
    /// <summary>Error message from twitch</summary>
    public string TwitchMessage{ get; }

    internal TwitchErrorException(int status, string? error, string? message) :
        base("Error from twitch " + status.ToString() + (error != null ? $" ({error})" : "") + ": " + message ?? "No message")
    {
        Status = status;
        TwitchMessage = message ?? string.Empty;
    }
}

/// <summary>400 Bad Request status in response</summary>
public class BadRequestException : TwitchErrorException
{
    internal BadRequestException(string? message) : base(400, "Bad Request", message) { }
}

/// <summary>401 Unauthorized status in response: token withdrawn by user</summary>
public class UnauthorizedException : TwitchErrorException
{
    internal UnauthorizedException(string? message) : base(401, "Unauthorized", message) { }
}

/// <summary>403 Forbidden status in response</summary>
public class ForbiddenException : TwitchErrorException
{
    internal ForbiddenException(string? message) : base(403, "Forbidden", message) { }
}

/// <summary>404 Not Found status in response</summary>
public class NotFoundException : TwitchErrorException
{
    internal NotFoundException(string? message) : base(404, "Not Found", message) { }
}

/// <summary>429 Too Many Requests status in response</summary>
public class TooManyRequestsException : TwitchErrorException
{
    /// <summary>Timestamp that identifies when your bucket is reset to full</summary>
    public DateTime? RateLimitReset { get; }

    internal TooManyRequestsException(string? message, DateTime? rateLimitReset) : base(429, "Too Many Requests", message) 
    {
        RateLimitReset = rateLimitReset;
    }
}

/// <summary>500-599 status in response: something bad happened on twitch side</summary>
public class ServerErrorException : TwitchErrorException
{
    internal ServerErrorException(int status, string? error, string? message) : base(status, error, message) { }
}
