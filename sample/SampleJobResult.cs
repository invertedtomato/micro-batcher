namespace InvertedTomato.MicroBatcher.Sample;

public readonly record struct SampleJobResult(
    Int32 Id
)
{
    public override String ToString() => $"Job result {Id}";
};