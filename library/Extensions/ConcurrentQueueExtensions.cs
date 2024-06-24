using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace InvertedTomato.MicroBatcher.Extensions;

/// <summary>
/// Helper extensions for ConcurrentQueue.
/// </summary>
public static class ConcurrentQueueExtensions
{
    /// <summary>
    /// Dequeue all records currently on the queue.
    /// </summary>
    public static Collection<T> DequeueAll<T>(this ConcurrentQueue<T> target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        var output = new Collection<T>();
        while(target.TryDequeue(out var record)) output.Add(record);
        return output;
    }
}