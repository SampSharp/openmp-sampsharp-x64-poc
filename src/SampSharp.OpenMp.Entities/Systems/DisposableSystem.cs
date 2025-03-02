namespace SampSharp.Entities;

public abstract class DisposableSystem : ISystem, IDisposable
{
    private readonly List<IDisposable> _disposables = [];
    private bool _disposed;

    protected void AddDisposable(IDisposable disposable)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _disposables.Add(disposable);
    }

    protected virtual void OnDispose()
    {
        var errors = new List<Exception>();
        foreach (var disposable in _disposables)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }

        _disposables.Clear();
        
        if (errors.Count > 0)
        {
            throw new AggregateException(errors);
        }
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        try
        {
            OnDispose();
        }
        finally
        {
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}