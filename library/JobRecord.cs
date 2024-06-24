namespace InvertedTomato.MicroBatcher;

/// <summary>
/// Represents a pending job, with a completion source to trigger the result of the job.
/// </summary>
public readonly record struct JobRecord<TJob, TJobResult>(
    TJob Job,
    TaskCompletionSource<TJobResult> TaskCompletionSource
);