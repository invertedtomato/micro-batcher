namespace InvertedTomato.MicroBatcher;

/// <summary>
/// Configure the parameters for a MicroBatcherClient.
/// </summary>
public readonly record struct Options()
{
    /// <summary>
    /// Target number of jobs to include in each batch. More or less may occur in edge scenarios.
    /// </summary>
    public Int32 JobCountTarget { get; private init; } = Int32.MaxValue;

    /// <summary>
    /// Set the target number of jobs to include in each batch. More or less may occur in edge scenarios.
    /// </summary>
    public Options WithJobCountTarget(Int32 value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(value));
        return this with
        {
            JobCountTarget = value
        };
    }

    // public TimeSpan MinDelay { get; private init; } = TimeSpan.MaxValue;
    //
    // public Options WithMinDelay(TimeSpan value)
    // {
    //     ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value.Ticks, nameof(value));
    //     return this with
    //     {
    //         MinDelay = value
    //     };
    // }
}