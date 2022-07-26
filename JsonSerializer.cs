using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp.Serializers;
using System.Globalization;

namespace Twitcher.API;

internal class JsonSnakeSerializer : IRestSerializer
{
    private static JsonSerializerSettings CreateSettings()
    {
        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
        };
        settings.Converters.Add(new DateTimeConverter());
        //settings.Converters.Add(new SnakeEnumConverter<LeaderboardTimePeriod>(LeaderboardTimePeriod.All));
        //settings.Converters.Add(new SnakeEnumConverter<CheermoteType>(CheermoteType.None));
        //settings.Converters.Add(new SnakeEnumConverter<UserType>(UserType.None));
        //settings.Converters.Add(new SnakeEnumConverter<BroadcasterType>(BroadcasterType.None));
        return settings;
    }

    private static readonly JsonSerializerSettings _settings = CreateSettings();
    private static readonly string[] _acceptedContentTypes = { "application/json", "text/json", "text/x-json", "text/javascript", "*+json" };

    internal static JsonSerializerSettings Settings => _settings;

    public ISerializer Serializer { get; } = new Serializer();

    public IDeserializer Deserializer { get; } = new Deserializer();

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

internal class Serializer : ISerializer
{
    public string ContentType { get; set; } = "application/json";

    public string? Serialize(object obj) => JsonConvert.SerializeObject(obj, JsonSnakeSerializer.Settings);
}

internal class Deserializer : IDeserializer
{
    public T? Deserialize<T>(RestResponse response) => string.IsNullOrEmpty(response.Content) ? default : JsonConvert.DeserializeObject<T>(response.Content, JsonSnakeSerializer.Settings);
}

//internal class SnakeConverter : JsonNamingPolicy
//{
//    public override string ConvertName(string name) => CamelToSnake(name);

//    public static string CamelToSnake(string name)
//    {
//        var sb = new StringBuilder();
//        bool isPrevious = true;
//        foreach (var c in name)
//        {
//            if (!isPrevious & (isPrevious = (char.IsUpper(c) || char.IsDigit(c))))
//                sb.Append('_');
//            sb.Append(char.ToLower(c));
//        }
//        return sb.ToString();
//    }

//    public static string SnakeToCamel(string name)
//    {
//        var sb = new StringBuilder();
//        bool isUpper = true;
//        foreach (var c in name)
//        {
//            if (c == '_')
//                isUpper = true;
//            else if (isUpper)
//            {
//                sb.Append(char.ToUpper(c));
//                isUpper = false;
//            }
//            else
//                sb.Append(char.ToLower(c));
//        }
//        return sb.ToString();
//    }
//}

internal class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
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

//internal class SnakeEnumConverter<T> : JsonConverter<T> where T : struct, IConvertible
//{
//    internal T DefaultValue { get; }

//    public SnakeEnumConverter(T defaultValue)
//    {
//        DefaultValue = defaultValue;
//    }

//    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        var str = reader.GetString();
//        if (string.IsNullOrEmpty(str))
//            return DefaultValue;
//        return Enum.Parse<T>(SnakeConverter.SnakeToCamel(str));
//    }

//    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => writer.WriteStringValue(SnakeConverter.CamelToSnake(value.ToString()!));

//}
