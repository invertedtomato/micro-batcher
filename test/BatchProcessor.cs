using System.Collections.ObjectModel;
using InvertedTomato.MicroBatcher.Extensions;

namespace InvertedTomato.MicroBatcher.Test;

public sealed class BatchProcessor : IBatchProcessor<Job, JobResult>
{
    public void Process(Collection<JobRecord<Job, JobResult>> records) => 
        records.ForEach(x => x.TaskCompletionSource.SetResult(new(x.Job.Id)));
}