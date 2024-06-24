namespace InvertedTomato.MicroBatcher;

public readonly record struct Options
{
    public Int32? MaxBatchSize { get; private init; }

    public Options WithMaxBatchSize(Int32 value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(value));
        return this with
        {
            MaxBatchSize = value
        };
    }
    
    public TimeSpan? MinDelay { get; private init; }

    public Options WithMinDelay(TimeSpan value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value.Ticks, nameof(value));
        return this with
        {
            MinDelay = value
        };
    }
}