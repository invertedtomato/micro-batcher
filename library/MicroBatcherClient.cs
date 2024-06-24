using System.Collections.Concurrent;
using InvertedTomato.MicroBatcher.Extensions;

namespace InvertedTomato.MicroBatcher;

/// <summary>
/// Tool to batch many jobs to provide down-stream efficentcy.
/// </summary>
/// <typeparam name="TJob">Definition of a job</typeparam>
/// <typeparam name="TJobResult">Definition of the result of a job</typeparam>
public sealed class MicroBatcherClient<TJob, TJobResult> : IDisposable
    where TJob : notnull
    where TJobResult : notnull
{
    private readonly IBatchProcessor<TJob, TJobResult> _processor;
    private readonly Options _options;
    private readonly ConcurrentQueue<JobRecord<TJob, TJobResult>> _queue = new();
    private readonly Object _processLock = new();
    private Boolean _isDisposed;

    /// <summary>
    /// Normal constructor for batcher
    /// </summary>
    /// <param name="processor">Processor to handle batches once ready</param>
    /// <param name="optionBuilder">Fluent option builder</param>
    public MicroBatcherClient(IBatchProcessor<TJob, TJobResult> processor, Func<Options, Options> optionBuilder)
    {
        ArgumentNullException.ThrowIfNull(processor, nameof(processor));
        ArgumentNullException.ThrowIfNull(optionBuilder, nameof(optionBuilder));

        _processor = processor;
        _options = optionBuilder(new());
    }

    /// <summary>
    /// Submit a job to be batched.
    /// </summary>
    public async Task<TJobResult> Submit(TJob job)
    {
        var tcs = new TaskCompletionSource<TJobResult>();
        lock (_processLock)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            _queue.Enqueue(new(job, tcs));
            if (_queue.Count >= _options.JobCountTarget) ProcessNow();
        }

        return await tcs.Task.ConfigureAwait(false);
    }

    // TODO: Max delay

    private void ProcessNow()
    {
        var records = _queue.DequeueAll();
        if (records.Count == 0) return;
        _processor.Process(records);
    }

    /// <summary>
    /// Cease accepting new jobs and process all remaining jobs.
    /// </summary>
    public void Dispose()
    {
        lock (_processLock)
        {
            if (_isDisposed) return;
            _isDisposed = true;
            ProcessNow();
        }
    }
}