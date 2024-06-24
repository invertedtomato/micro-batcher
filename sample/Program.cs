// See https://aka.ms/new-console-template for more information

using InvertedTomato.MicroBatcher;
using InvertedTomato.MicroBatcher.Sample;

var batcher = new MicroBatcherClient<SampleJob, SampleJobResult>(new SampleBatchProcessor(), opts => opts
        .WithMaxJobsPerBatch(2)
    // .WithMinDelay(TimeSpan.FromSeconds(1))
);

// Async submit jobs jobs
var job1 =  batcher.Submit(new SampleJob(1));
var job2 =  batcher.Submit(new SampleJob(2));
var job3 =  batcher.Submit(new SampleJob(3));

// Wait for jobs to complete
batcher.Dispose();

// Print results
Console.WriteLine(await job1);
Console.WriteLine(await job2);
Console.WriteLine(await job3);