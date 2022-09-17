namespace Twitcher.API.Linq;

/// <summary>HTTP method to use when making requests</summary>
public enum RequestMethod
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Get = Method.Get,
    Post = Method.Post,
    Put = Method.Put,
    Delete = Method.Delete,
    Head = Method.Head,
    Options = Method.Options,
    Patch = Method.Patch,
    Merge = Method.Merge,
    Copy = Method.Copy,
    Search = Method.Search
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
