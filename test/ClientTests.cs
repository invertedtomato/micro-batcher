using FluentAssertions;

namespace InvertedTomato.MicroBatcher.Test;

public class ClientTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task CanProcessWhenMaxJobsReached(Int32 count)
    {
        var sut = GenerateSut(opt => opt
            .WithMaxJobsPerBatch(count));
        var jobs = SubmitJobs(sut, count);
        sut.Dispose();
        sut.JobsProcessed.Should().Be(count);
        sut.BatchesProcessed.Should().Be(1);
        var results = await jobs;
        results.Length.Should().Be(count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task CanProcessWhenMaxDelayReached(Int32 count)
    {
        using var sut = GenerateSut(opt => opt
            .WithMaxDelayPerJob(TimeSpan.FromSeconds(1)));
        var jobs = SubmitJobs(sut, count);

        await Task.Delay(TimeSpan.FromSeconds(2));
        sut.JobsProcessed.Should().Be(count);
        sut.BatchesProcessed.Should().Be(1);

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
    public async Task CanProcessMultipleBatches(Int32 count)
    {
        const Int32 jobsPerBatch = 5;
        var sut = GenerateSut(opt => opt
            .WithMaxJobsPerBatch(jobsPerBatch));
        var jobs = SubmitJobs(sut, count);
        sut.Dispose();
        sut.BatchesProcessed.Should().Be((Int32)Math.Ceiling((Single)count / jobsPerBatch));
        sut.JobsProcessed.Should().Be(count);
        var results = await jobs;
        results.Length.Should().Be(count);
    }

    private static MicroBatcherClient<Job, JobResult> GenerateSut(Func<Options, Options>? optionBuilder = null) =>
        new(new BatchProcessor(), optionBuilder);

    private static async Task<JobResult[]> SubmitJobs(MicroBatcherClient<Job, JobResult> sut, Int32 count) =>
        await Task.WhenAll(Enumerable
            .Range(0, count)
            .Select(i => new Job(i))
            .Select(sut.Submit));
}