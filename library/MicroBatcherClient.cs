﻿using System.Collections.Concurrent;
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
    private readonly Timer _flushTimer;
    private Boolean _isDisposed;
    private Int64 _batchesProcessed;
    private Int64 _jobsProcessed;

    /// <summary>
    /// Number of batches that have been processed since creation.
    /// </summary>
    public Int64 BatchesProcessed => _batchesProcessed;

    /// <summary>
    /// Number of jobs that have been processed since creation.
    /// </summary>
    public Int64 JobsProcessed => _jobsProcessed;

    /// <summary>
    /// Normal constructor for batcher
    /// </summary>
    /// <param name="processor">Processor to handle batches once ready</param>
    /// <param name="optionBuilder">Fluent option builder</param>
    public MicroBatcherClient(IBatchProcessor<TJob, TJobResult> processor, Func<Options, Options>? optionBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(processor, nameof(processor));
        ArgumentNullException.ThrowIfNull(optionBuilder, nameof(optionBuilder));

        _processor = processor;
        _options = optionBuilder?.Invoke(new()) ?? new();
        _flushTimer = new(
            _ => ProcessNow(),
            null,
            TimeSpan.Zero,
            _options.MaxDelayPerJob == TimeSpan.Zero ? Timeout.InfiniteTimeSpan : _options.MaxDelayPerJob);
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
            if (_queue.Count >= _options.MaxJobsPerBatch) ProcessNow();
        }

        return await tcs.Task.ConfigureAwait(false);
    }

    private void ProcessNow()
    {
        var records = _queue.DequeueAll();
        if (records.Count == 0) return;
        _processor.Process(records);
        Interlocked.Increment(ref _batchesProcessed);
        Interlocked.Add(ref _jobsProcessed, records.Count);
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
            _flushTimer.Dispose();
            ProcessNow();
        }
    }
}