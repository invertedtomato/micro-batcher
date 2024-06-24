// See https://aka.ms/new-console-template for more information

using InvertedTomato.MicroBatcher;
using InvertedTomato.MicroBatcher.Extensions;
using InvertedTomato.MicroBatcher.Sample;

var batcher = new MicroBatcherClient<SampleJob, SampleJobResult>(new SampleBatchProcessor(), opts => opts
        .WithJobCountTarget(10)
    // .WithMinDelay(TimeSpan.FromSeconds(1))
);

// Run 100 jobs in parallel
var results = await Task.WhenAll(Enumerable
    .Range(0, 100)
    .Select(i => new SampleJob(i))
    .Select(batcher.Submit));

// Wait for jobs to complete
batcher.Dispose(); // The following `WhenAll` effectively does the same thing, however there is a desire for a "shutdown method".

// Print results
Console.WriteLine("Job Results:");
results.ForEach(x => Console.WriteLine($"  {x}"));