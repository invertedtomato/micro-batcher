// See https://aka.ms/new-console-template for more information

using InvertedTomato.MicroBatcher;
using InvertedTomato.MicroBatcher.Sample;

using var batcher = new MicroBatcherClient<Job, JobResult>(new SampleBatchProcessor(), opts => opts
    .WithMaxBatchSize(10)
    .WithMinDelay(TimeSpan.FromSeconds(1))
);

// Start 100 jobs
var jobResults = Enumerable
    .Range(0, 100)
    .Select(i => new Job(i))
    .Select(batcher.Submit);

// Wait for jobs to complete
batcher.Dispose(); //  `await Task.WhenAll(jobResults)` would have achieved the same net result, but there is a desire for an explicit "shutdown method"