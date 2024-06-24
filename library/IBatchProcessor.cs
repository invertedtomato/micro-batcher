using System.Collections.ObjectModel;

namespace InvertedTomato.MicroBatcher;

/// <summary>
/// Interface to implement a batch processor, to batch-process jobs.
/// </summary>
/// <typeparam name="TJob">Definition of a job</typeparam>
/// <typeparam name="TJobResult">Definition of the result of a job</typeparam>
public interface IBatchProcessor<TJob, TJobResult>
{
    /// <summary>
    /// Process a batch of jobs
    /// </summary>
    void Process(Collection<JobRecord<TJob, TJobResult>> records);
}