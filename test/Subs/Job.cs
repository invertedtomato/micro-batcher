namespace InvertedTomato.MicroBatcher.Test;

public readonly record struct Job(
    Int32 Id
)
{
    public override String ToString() => $"Job {Id}";
};