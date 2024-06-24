namespace InvertedTomato.MicroBatcher.Test;

public readonly record struct JobResult(
    Int32 Id
)
{
    public override String ToString() => $"Job {Id}";
};