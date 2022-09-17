namespace Twitcher.API.Enumerators;

internal static class IEnumerableExtensions
{
    internal static IEnumerable<IEnumerable<T>> UnsafeBatch<T>(this IEnumerable<T> source, int size)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

        switch (source)
        {
            case ICollection<T> { Count: 0 }:
                yield break;

            case ICollection<T> collection when collection.Count <= size:
                yield return source;
                yield break;

            case IReadOnlyCollection<T> { Count: 0 }:
                yield break;

            case IReadOnlyCollection<T> collection when collection.Count <= size:
                yield return source;
                yield break;
        }

        var count = 0;
        T[] bucket = new T[size];

        foreach (var item in source)
        {
            bucket[count++] = item;

            if (count == size)
            {
                yield return bucket;
                count = 0;
            }
        }

        if (count > 0)
            yield return bucket.Take(count);
    }
}
