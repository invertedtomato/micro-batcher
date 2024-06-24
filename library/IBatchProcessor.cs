namespace InvertedTomato.MicroBatcher;

public interface IBatchProcessor<TJob, TJobResult>
{
    IEnumerable<TJobResult> Process(IEnumerable<TJob> jobs);
}