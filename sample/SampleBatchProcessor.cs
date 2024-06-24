namespace InvertedTomato.MicroBatcher.Sample;

public sealed class SampleBatchProcessor : IBatchProcessor<Job, JobResult>
{
    public IEnumerable<JobResult> Process(IEnumerable<Job> jobs) =>
        jobs.Select(job => new JobResult(job.Argument1));
}