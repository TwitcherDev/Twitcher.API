using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Twitcher.API;

internal static class RestRequestExtensions
{
    internal static RestRequest AddQueryParameterDefault(this RestRequest request, string name, string? value, string? defaultValue = null)
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        if (value != defaultValue)
            request.AddQueryParameter(name, value);

        return request;
    }

    internal static RestRequest AddQueryParameterDefault<T>(this RestRequest request, string name, T value, T defaultValue)
        where T : struct, IEquatable<T>
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        if (!value.Equals(defaultValue))
            request.AddQueryParameter(name, value);

        return request;
    }

    internal static RestRequest AddQueryParameterNotNull(this RestRequest request, string name, string value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        ArgumentNullException.ThrowIfNull(value, paramName);

        request.AddQueryParameter(name, value);

        return request;
    }

    internal static RestRequest AddQueryParametersNotEmpty(this RestRequest request, string name, IEnumerable<string> values, [CallerArgumentExpression(nameof(values))] string? paramName = null)
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        ArgumentNullException.ThrowIfNull(values, paramName);

        var isAny = false;
        foreach (var value in values)
        {
            request.AddQueryParameter(name, value);
            isAny = true;
        }
        if (!isAny)
            throw new ArgumentException("Cannot be empty", paramName);

        return request;
    }

    internal static RestRequest AddQueryParametersNotEmpty<T>(this RestRequest request, string name, IEnumerable<T> values, [CallerArgumentExpression(nameof(values))] string? paramName = null)
        where T : struct
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        ArgumentNullException.ThrowIfNull(values, paramName);

        var isAny = false;
        foreach (var value in values)
        {
            request.AddQueryParameter(name, value);
            isAny = true;
        }
        if (!isAny)
            throw new ArgumentException("Cannot be empty", paramName);

        return request;
    }

    internal static RestRequest AddBodyNotNull(this RestRequest request, object body, [CallerArgumentExpression(nameof(body))] string? paramName = null)
    {
        Debug.Assert(request != null);

        ArgumentNullException.ThrowIfNull(body, paramName);

        request.AddBody(body);

        return request;
    }

}
