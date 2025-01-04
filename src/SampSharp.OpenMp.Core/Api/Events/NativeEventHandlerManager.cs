namespace SampSharp.OpenMp.Core.Api;

public abstract class NativeEventHandlerManager<TEventHandler> : INativeEventHandlerManager<TEventHandler> where TEventHandler : class
{
    private readonly Dictionary<TEventHandler, HandlerData> _handlers = [];

    public NativeEventHandler<TEventHandler> Get(TEventHandler handler)
    {
        return new NativeEventHandler<TEventHandler>(this, handler);
    }

    internal nint IncreaseReferenceCount(TEventHandler handler)
    {
        if (_handlers.TryGetValue(handler, out var reference))
        {
            _handlers[handler] = reference with
            {
                RefCount = reference.RefCount + 1
            };

            return reference.Handle;
        }

        var (newHandle, data) = Create(handler);

        _handlers[handler] = new HandlerData(newHandle, 1, data);

        return newHandle;
    }

    internal void DecreaseReferenceCount(TEventHandler handler)
    {
        if (!_handlers.TryGetValue(handler, out var reference))
        {
            return;
        }

        if (reference.RefCount == 1)
        {
            _handlers.Remove(handler);
            Free(reference.Handle);
        }

        _handlers[handler] = reference with
        {
            RefCount = reference.RefCount - 1
        };
    }
    
    internal nint? GetReference(TEventHandler handler)
    {
        return _handlers.TryGetValue(handler, out var reference) 
            ? reference.Handle 
            : null;
    }

    protected abstract (nint, object) Create(TEventHandler handler);

    protected abstract void Free(nint handle);

    private readonly record struct HandlerData(nint Handle, int RefCount, object Data);

}