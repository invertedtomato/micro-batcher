namespace InvertedTomato.MicroBatcher;

public sealed class MicroBatcherClient<TJob, TJobResult> : IDisposable
{
    private readonly IBatchProcessor<TJob, TJobResult> _processor;
    private readonly Options _options;
    private Boolean _isDisposed;

    public MicroBatcherClient(IBatchProcessor<TJob, TJobResult> processor, Func<Options, Options> builder)
    {
        ArgumentNullException.ThrowIfNull(processor, nameof(processor));
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        _processor = processor;
        _options = builder(new());
    }

    public MicroBatcherClient(IBatchProcessor processor, Options? options = null)
    {
        ArgumentNullException.ThrowIfNull(processor, nameof(processor));
        _processor = processor;
        _options = options ?? new();
    }

    public async Task<TJobResult> Submit(TJob job)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _isDisposed = true;
        throw new NotImplementedException();
    }
}