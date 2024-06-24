using System.Collections.ObjectModel;
using InvertedTomato.MicroBatcher.Extensions;

namespace InvertedTomato.MicroBatcher.Test.Subs;

public sealed class BatchProcessor : IBatchProcessor<Job, JobResult>
{
    private Int64 _batchesProcessed;
    private Int64 _jobsProcessed;

    /// <summary>
    /// Number of batches that have been processed since creation.
    /// </summary>
    public Int64 BatchesProcessed => _batchesProcessed;

    /// <summary>
    /// Number of jobs that have been processed since creation.
    /// </summary>
    public Int64 JobsProcessed => _jobsProcessed;

    public void Process(Collection<JobRecord<Job, JobResult>> records)
    {
        records.ForEach(x => x.TaskCompletionSource.SetResult(new(x.Job.Id)));

        Interlocked.Increment(ref _batchesProcessed);
        Interlocked.Add(ref _jobsProcessed, records.Count);
    }
}