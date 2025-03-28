namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Represents the unmanaged counterpart of an <see cref="IEventHandler{TEventHandler}" />.
/// </summary>
/// <typeparam name="TEventHandler">The interface type of the event handler.</typeparam>
public readonly struct NativeEventHandler<TEventHandler> where TEventHandler : class
{
    private readonly NativeEventHandlerManager<TEventHandler> _manager;
    private readonly TEventHandler _handler;

    internal NativeEventHandler(NativeEventHandlerManager<TEventHandler> manager, TEventHandler handler)
    {
        _manager = manager;
        _handler = handler;
    }

    /// <summary>
    /// Gets the reference to the native event handler. Returns <see langword="null" /> if the native event handler has
    /// not been created or has been freed.
    /// </summary>
    public nint? Handle => _manager?.GetReference(_handler);

    /// <summary>
    /// Creates a new reference to the native event handler or increases its reference count.
    /// </summary>
    /// <returns>The created reference to the native event handler.</returns>
    public nint Create()
    {
        return _manager?.IncreaseReferenceCount(_handler) ?? throw new InvalidOperationException("Invalid state");
    }

    /// <summary>
    /// Decreases the reference count of the native event handler or frees its resources.
    /// </summary>
    public void Free()
    {
        if (_manager == null)
        {
            throw new InvalidOperationException("Invalid state");
        }

        _manager.DecreaseReferenceCount(_handler);
    }
}