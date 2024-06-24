namespace InvertedTomato.MicroBatcher.Sample;

public readonly record struct SampleJob(
    Int32 Id
)
{
    public override String ToString() => $"Job {Id}";
};