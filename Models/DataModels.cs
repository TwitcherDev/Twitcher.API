namespace Twitcher.API.Models;

/// <typeparam name="T">Type of response data</typeparam>
/// <param name="Data">Response data</param>
public record DataResponse<T>(T Data);

/// <typeparam name="T">Type of response data</typeparam>
/// <param name="Data">Response data</param>
/// <param name="Pagination">A cursor value, to be used in a subsequent request to specify the starting point of the next set of results</param>
public record DataPaginationResponse<T>(T Data, Pagination? Pagination) : DataResponse<T>(Data);

/// <param name="Cursor">A cursor value, to be used in a subsequent request to specify the starting point of the next set of results</param>
public record Pagination(string? Cursor);
