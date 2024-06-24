using FluentAssertions;

namespace InvertedTomato.MicroBatcher.Test;

public class ClientTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task CanProcessWhenBatchTargetReached(Int32 count)
    {
        var sut = GenerateSut(opt => opt
            .WithJobCountTarget(count));
        await SubmitJobs(sut, count);
        sut.Dispose();
        sut.JobsProcessed.Should().Be(count);
        sut.BatchesProcessed.Should().Be(1);
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
            .WithJobCountTarget(jobsPerBatch));
        await SubmitJobs(sut, count);
        sut.Dispose();
        sut.BatchesProcessed.Should().Be((Int32)Math.Ceiling((Single)count / jobsPerBatch));
        sut.JobsProcessed.Should().Be(count);
    }

    private static MicroBatcherClient<Job, JobResult> GenerateSut(Func<Options, Options>? optionBuilder = null) =>
        new(new BatchProcessor(), optionBuilder);

    private static async Task SubmitJobs(MicroBatcherClient<Job, JobResult> sut, Int32 count) =>
        await Task.WhenAll(Enumerable
            .Range(0, 100)
            .Select(i => new Job(i))
            .Select(sut.Submit));
}