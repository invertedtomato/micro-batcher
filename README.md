# InvertedTomato.MicroBatcher
Micro-batching is a technique used in processing pipelines where individual tasks are grouped
together into small batches. This can improve throughput by reducing the number of requests made
to a downstream system.

## Overview
```c#
// Setup job models
public readonly record struct Job(Int32 Id);
public readonly record struct JobResult(Int32 Id);

// Setup how jobs are processed
public sealed class BatchProcessor : IBatchProcessor<Job, JobResult>
{
    public void Process(Collection<JobRecord<SampleJob, SampleJobResult>> records)
    {
        Console.WriteLine($"Processing batch of {records.Count} jobs:");
        foreach (var record in records)
        {
            var job = record.Job;
            var result = new SampleJobResult(job.Id);
            Console.WriteLine($"  * {job} => {result}");
        }
    }
}

// Create a Batcher, this could be handled using DI
var batcher = new MicroBatcherClient<Job, JobResult>(new BatchProcessor(), opts => opts
    .WithMaxJobsPerBatch(100)                       // <== Set max records per batch.
    .WithMaxDelayPerJob(TimeSpan.FromSeconds(1))    // <== Set max delay for processing job
);

// Submit jobs (NOTE: this will block until the whole batch is processed, so keep the configured maxs above in mind)
await batcher.Submit(new Job(1));
await batcher.Submit(new Job(2));
await batcher.Submit(new Job(3));

// Wait for jobs to complete (optional, but a "shutdown method" is desired)
batcher.Dispose();
```