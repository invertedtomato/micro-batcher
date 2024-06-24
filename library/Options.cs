namespace InvertedTomato.MicroBatcher;

/// <summary>
/// Configure the parameters for a MicroBatcherClient.
/// </summary>
public readonly record struct Options()
{
    /// <summary>
    /// Maximum number of jobs to include in each batch. More or less may occur in edge scenarios.
    /// </summary>
    public Int32 MaxJobsPerBatch { get; private init; } = 100;

    /// <summary>
    /// Set the maximum number of jobs to include in each batch. More or less may occur in edge scenarios.
    /// </summary>
    public Options WithMaxJobsPerBatch(Int32 value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(value));
        return this with
        {
            MaxJobsPerBatch = value
        };
    }

    /// <summary>
    /// Maximum delay before processing a job. Used when the target job count isn't reached before this time elapses.
    /// </summary>
    public TimeSpan MaxDelayPerJob { get; private init; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Set the maximum delay before processing a job. Used when the target job count isn't reached before this time elapses. Set to `TimeSpan.Zero` to disable.
    /// </summary>
    public Options WithMaxDelayPerJob(TimeSpan value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value.Ticks, nameof(value));
        return this with
        {
            MaxDelayPerJob = value
        };
    }
}