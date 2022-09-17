namespace Twitcher.API.Linq;

/// <inheritdoc/>
public class TwitchRequest : RestRequest
{
    internal TwitchRequest(string resource, RequestMethod method) : base(resource, (Method)method) { }
}
