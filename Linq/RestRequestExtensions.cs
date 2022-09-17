using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Twitcher.API.Linq;

/// <summary>More <see cref="RestRequest"/> add parameter methods</summary>
public static class RestRequestExtensions
{
    /// <summary>Adds a query string to the request if it is not equal to the <paramref name="defaultValue"/></summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="value">Parameter value</param>
    /// <param name="defaultValue">Default value, which will not be added as a parameter</param>
    public static RestRequest AddQueryParameterOrDefault(this RestRequest request, string name, string? value, string? defaultValue = null)
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        if (value != defaultValue)
            request.AddQueryParameter(name, value);

        return request;
    }

    /// <summary>Adds a query string to the request if <paramref name="value"/> has value</summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="value">Parameter value</param>
    public static RestRequest AddQueryParameterOrDefault<T>(this RestRequest request, string name, T? value)
        where T : struct
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        if (value.HasValue)
            request.AddQueryParameter(name, value.Value);

        return request;
    }

    /// <summary>Adds a query string to the request if it is not equal to the <paramref name="defaultValue"/></summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="value">Parameter value</param>
    /// <param name="defaultValue">Default value, which will not be added as a parameter</param>
    public static RestRequest AddQueryParameterOrDefault<T>(this RestRequest request, string name, T value, T defaultValue)
        where T : struct, IEquatable<T>
    {
        Debug.Assert(request != null);
        Debug.Assert(!string.IsNullOrEmpty(name));

        if (!value.Equals(defaultValue))
            request.AddQueryParameter(name, value);

        return request;
    }

    /// <summary>Adds a query string to the request. Throws an exception if <paramref name="value"/> is <see langword="null"/></summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="value">Parameter value</param>
    /// <param name="paramName">The name of <paramref name="value"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static RestRequest AddQueryParameterNotNull(this RestRequest request, string name, string value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value, paramName);

        request.AddQueryParameter(name, value);

        return request;
    }

    /// <summary>Adds each element of the sequence as a query string to the request or any element is <see langword="null"/></summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="values">Parameter values</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static RestRequest AddQueryParameters(this RestRequest request, string name, IEnumerable<string>? values)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(name);

        if (values is not null)
            foreach (var value in values)
                request.AddQueryParameterNotNull(name, value);

        return request;
    }

    /// <summary>Adds each element of the sequence as a query string to the request</summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="values">Parameter values</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static RestRequest AddQueryParameters<T>(this RestRequest request, string name, IEnumerable<T>? values)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(name);

        if (values is not null)
            foreach (var value in values)
                request.AddQueryParameter(name, value);

        return request;
    }

    /// <summary>Adds each element of the sequence as a query string to the request. Throws an exception if the sequence does not contain elements or any element is <see langword="null"/></summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="values">Parameter values</param>
    /// <param name="paramName">The name of <paramref name="values"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static RestRequest AddQueryParametersNotEmpty(this RestRequest request, string name, IEnumerable<string> values, [CallerArgumentExpression(nameof(values))] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(values, paramName);

        var isAny = false;
        foreach (var value in values)
        {
            request.AddQueryParameterNotNull(name, value);
            isAny = true;
        }
        if (!isAny)
            throw new ArgumentException("Cannot be empty", paramName);

        return request;
    }

    /// <summary>Adds each element of the sequence as a query string to the request. Throws an exception if the sequence does not contain elements</summary>
    /// <param name="request">Request instance</param>
    /// <param name="name">Parameter name</param>
    /// <param name="values">Parameter values</param>
    /// <param name="paramName">The name of <paramref name="values"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static RestRequest AddQueryParametersNotEmpty<T>(this RestRequest request, string name, IEnumerable<T> values, [CallerArgumentExpression(nameof(values))] string? paramName = null)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(name);
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

    /// <summary>Adds a body parameter to the request. Throws an exception if <paramref name="body"/> is <see langword="null"/></summary>
    /// <param name="request">Request instance</param>
    /// <param name="body">Object to be used as the request body, or string for plain content</param>
    /// <param name="paramName">The name of <paramref name="body"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static RestRequest AddBodyNotNull(this RestRequest request, object body, [CallerArgumentExpression(nameof(body))] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(body, paramName);

        request.AddBody(body);

        return request;
    }

}
