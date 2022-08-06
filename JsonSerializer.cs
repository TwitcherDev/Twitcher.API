using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp.Serializers;
using System.Text;

namespace Twitcher.API;

internal class TwitcherJsonSerializer : IRestSerializer
{
    private static JsonSerializerSettings CreateSettings()
    {
        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeStrategy() }
        };
        settings.Converters.Add(new DateTimeConverter());
        settings.Converters.Add(new StringEnumConverter<BroadcasterType>(BroadcasterType.None, false));
        settings.Converters.Add(new StringEnumConverter<CheermoteType>(CheermoteType.None, false));
        settings.Converters.Add(new StringEnumConverter<ExtensionType>(ExtensionType.Component, false));
        settings.Converters.Add(new StringEnumConverter<LeaderboardTimePeriod>(LeaderboardTimePeriod.All, false));
        settings.Converters.Add(new StringEnumConverter<ReasonType>(ReasonType.Other, false));
        settings.Converters.Add(new StringEnumConverter<RedemptionStatus>(RedemptionStatus.Unfulfilled, true));
        settings.Converters.Add(new StringEnumConverter<SortOrder>(SortOrder.Oldest, true));
        settings.Converters.Add(new StringEnumConverter<SourceContextType>(SourceContextType.Chat, false));
        settings.Converters.Add(new StringEnumConverter<UserType>(UserType.None, false));
        settings.Converters.Add(new StringEnumConverter<VideoSortOrder>(VideoSortOrder.Time, false));
        settings.Converters.Add(new StringEnumConverter<VideoTimePeriod>(VideoTimePeriod.All, false));
        settings.Converters.Add(new StringEnumConverter<VideoType>(VideoType.Upload, false));
        settings.Converters.Add(new StringEnumConverter<ViewableType>(ViewableType.Public, false));
        //settings.Converters.Add(new StringEnumConverter(new SnakeStrategy()));
        return settings;
    }

    private static readonly JsonSerializerSettings _settings = CreateSettings();
    private static readonly string[] _acceptedContentTypes = { "application/json", "text/json", "text/x-json", "text/javascript", "*+json" };

    internal static JsonSerializerSettings Settings => _settings;

    public ISerializer Serializer { get; } = new TwitcherSerializer();

    public IDeserializer Deserializer { get; } = new TwitcherDeserializer();

    public string[] AcceptedContentTypes => _acceptedContentTypes;

    public SupportsContentType SupportsContentType { get; } = (type) => _acceptedContentTypes.Contains(type);

    public DataFormat DataFormat { get; } = DataFormat.Json;

    public string? Serialize(Parameter parameter)
    {
        if (parameter.Value == null)
            return null;
        return Serializer.Serialize(parameter.Value);
    }
}

internal class TwitcherSerializer : ISerializer
{
    public string ContentType { get; set; } = "application/json";

    public string? Serialize(object obj) => JsonConvert.SerializeObject(obj, TwitcherJsonSerializer.Settings);
}

internal class TwitcherDeserializer : IDeserializer
{
    public T? Deserialize<T>(RestResponse response) => string.IsNullOrEmpty(response.Content) ? default : JsonConvert.DeserializeObject<T>(response.Content, TwitcherJsonSerializer.Settings);
}

internal class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.ValueType == typeof(DateTime))
            return (DateTime)reader.Value!;
        var str = (string?)reader.Value;
        if (string.IsNullOrEmpty(str))
            return default;
        return DateTime.Parse(str);
    }

    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}

internal class SnakeStrategy : NamingStrategy
{
    protected override string ResolvePropertyName(string name) => CamelToSnake(name);

    public static string CamelToSnake(string name)
    {
        var sb = new StringBuilder();
        bool isPrevious = true;
        foreach (var c in name)
        {
            if (!isPrevious & (isPrevious = (char.IsUpper(c) || char.IsDigit(c))))
                sb.Append('_');
            sb.Append(char.ToLower(c));
        }
        return sb.ToString();
    }

    public static string SnakeToCamel(string name)
    {
        var sb = new StringBuilder();
        bool isUpper = true;
        foreach (var c in name)
        {
            if (c == '_')
                isUpper = true;
            else if (isUpper)
            {
                sb.Append(char.ToUpper(c));
                isUpper = false;
            }
            else
                sb.Append(char.ToLower(c));
        }
        return sb.ToString();
    }
}

internal class StringEnumConverter<T> : JsonConverter<T> where T : struct
{
    private readonly T _defaultValue;
    private readonly bool _isUpper;

    public StringEnumConverter(T defaultValue, bool isUpper)
    {
        _defaultValue = defaultValue;
        _isUpper = isUpper;
    }

    public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var str = (string?)reader.Value;
        if (string.IsNullOrEmpty(str))
            return _defaultValue;
        return Enum.Parse<T>(SnakeStrategy.SnakeToCamel(str), true);
    }

    public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
    {
        var str = value.ToString();
        if (string.IsNullOrEmpty(str))
            writer.WriteNull();
        else
        {
            var val = SnakeStrategy.CamelToSnake(str);
            if (_isUpper)
                val = val.ToUpper();
            writer.WriteValue(val);
        }
    }
}
