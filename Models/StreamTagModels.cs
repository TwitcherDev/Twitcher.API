namespace Twitcher.API.Models;

/// <param name="TagId">An ID that identifies the tag</param>
/// <param name="IsAuto">A Boolean value that determines whether the tag is an automatic tag. An automatic tag is one that Twitch adds to the stream. You cannot add or remove automatic tags. The value is <see langword="true"/> if the tag is an automatic tag; otherwise, <see langword="false"/></param>
/// <param name="LocalizationNames">A dictionary that contains the localized names of the tag. The key is in the form, {locale}-{coutry/region}. For example, us-en. The value is the localized name</param>
/// <param name="LocalizationDescriptions">A dictionary that contains the localized descriptions of the tag. The key is in the form, {locale}-{coutry/region}. For example, us-en. The value is the localized description</param>
public record StreamTagResponseBody(string TagId, bool IsAuto, Dictionary<string, string> LocalizationNames, Dictionary<string, string> LocalizationDescriptions);

/// <param name="TagIds">A list of IDs that identify the tags to apply to the channel. You may specify a maximum of five tags. To remove all tags from the channel, set <paramref name="TagIds"/> to an empty array</param>
public record ReplaceStreamTagsRequestBody(IEnumerable<string>? TagIds);
