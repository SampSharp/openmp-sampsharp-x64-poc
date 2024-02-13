namespace SashManaged;

public abstract class BaseEventHandler<T> : IDisposable where T : class
{
    private readonly T _handler;
    private bool _disposed;
    private static BaseEventHandler<T>? _active;

    protected BaseEventHandler(T handler, nint handle)
    {
        _handler = handler;
        Handle = handle;
        _active = this;
    }

    public nint Handle { get; }

    public static T? Active => _active?._handler;

    public static nint? ActiveHandle => _active?.Handle;

    protected static BaseEventHandler<T>? ActiveHandler => _active;

    protected static bool IsActive => _active != null;

    public event EventHandler? Disposing;

    ~BaseEventHandler()
    {
        Dispose(false);
    }

    protected static void ThrowIfActive()
    {
        if (IsActive)
        {
            throw new InvalidOperationException("Already active");
        }
    }

    protected abstract void Delete();

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        Disposing?.Invoke(this, EventArgs.Empty);

        Delete();
        _disposed = true;

        if (_active == this)
        {
            _active = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}