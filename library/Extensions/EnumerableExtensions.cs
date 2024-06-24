namespace InvertedTomato.MicroBatcher.Extensions;

/// <summary>
/// Helper extensions for ConcurrentQueue.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Run action on each element of enumerable.
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(enumeration, nameof(enumeration));
        ArgumentNullException.ThrowIfNull(action, nameof(action));
        foreach (var item in enumeration) action(item);
    }
}