using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

public readonly struct NativeEventHandler<TEventHandler> where TEventHandler : class
{
    private readonly NativeEventHandlerManager<TEventHandler> _manager;
    private readonly TEventHandler _handler;

    internal NativeEventHandler(NativeEventHandlerManager<TEventHandler> manager, TEventHandler handler)
    {
        _manager = manager;
        _handler = handler;
    }

    public nint? Handle => _manager.GetReference(_handler);

    public nint Create()
    {
        return _manager.IncreaseReferenceCount(_handler);
    }

    public void Free()
    {
        _manager.DecreaseReferenceCount(_handler);
    }
}