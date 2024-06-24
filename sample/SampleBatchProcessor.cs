using System.Collections.ObjectModel;

namespace InvertedTomato.MicroBatcher.Sample;

public sealed class SampleBatchProcessor : IBatchProcessor<SampleJob, SampleJobResult>
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