using FluentAssertions;
using InvertedTomato.MicroBatcher.Test.Subs;

namespace InvertedTomato.MicroBatcher.Test;

public class ClientTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(1000)]
    public async Task CanProcessWhenMaxJobsReached(Int32 count)
    {
        var processor = new BatchProcessor();
        var sut = new MicroBatcherClient<Job, JobResult>(processor, opt => opt
            .WithMaxJobsPerBatch(count)
            .WithMaxDelayPerJob(TimeSpan.FromDays(1)));
        var jobs = SubmitJobs(sut, count);
        sut.Dispose();
        processor.JobsProcessed.Should().Be(count);
        processor.BatchesProcessed.Should().Be(1);
        var results = await jobs;
        results.Length.Should().Be(count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(1000)]
    public async Task CanProcessWhenMaxDelayReached(Int32 count)
    {
        var processor = new BatchProcessor();
        var sut = new MicroBatcherClient<Job, JobResult>(processor, opt => opt
            .WithMaxJobsPerBatch(count + 1)
            .WithMaxDelayPerJob(TimeSpan.FromSeconds(1)));
        var jobs = SubmitJobs(sut, count);

        await Task.Delay(TimeSpan.FromSeconds(2));
        processor.JobsProcessed.Should().Be(count);
        processor.BatchesProcessed.Should().BeGreaterOrEqualTo(1);

        var results = await jobs;
        results.Length.Should().Be(count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(1000)]
    public async Task CanProcessMultipleBatches(Int32 count)
    {
        const Int32 jobsPerBatch = 5;
        var processor = new BatchProcessor();
        var sut = new MicroBatcherClient<Job, JobResult>(processor, opt => opt
            .WithMaxJobsPerBatch(jobsPerBatch)
            .WithMaxDelayPerJob(TimeSpan.FromDays(1)));
        var jobs = SubmitJobs(sut, count);
        sut.Dispose();
        processor.BatchesProcessed.Should().Be((Int32)Math.Ceiling((Single)count / jobsPerBatch));
        processor.JobsProcessed.Should().Be(count);
        var results = await jobs;
        results.Length.Should().Be(count);
    }
    
    private static async Task<JobResult[]> SubmitJobs(MicroBatcherClient<Job, JobResult> sut, Int32 count) =>
        await Task.WhenAll(Enumerable
            .Range(0, count)
            .Select(i => new Job(i))
            .Select(sut.Submit));
}