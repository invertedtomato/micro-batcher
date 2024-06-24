using System.Collections.ObjectModel;
using InvertedTomato.MicroBatcher.Extensions;

namespace InvertedTomato.MicroBatcher.Sample;

public sealed class SampleBatchProcessor : IBatchProcessor<SampleJob, SampleJobResult>
{
    public void Process(Collection<JobRecord<SampleJob, SampleJobResult>> records)
    {
        Console.WriteLine($"Processed {records.Count} jobs");
        records.ForEach(x => x.TaskCompletionSource.SetResult(new(x.Job.Id)));
    }
}