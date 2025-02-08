using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IIndexedEventDispatcher<T> where T : class, IEventHandler<T>
{
    private readonly nint _handle;

    public nint Handle => _handle;

    public int Count()
    {
        return (int)IndexedEventDispatcherInterop.Count(_handle);
    }

    public int Count(int index)
    {
        return (int)IndexedEventDispatcherInterop.Count(_handle, index);
    }

    public bool AddEventHandler(T handler, int index, EventPriority priority = EventPriority.Default)
    {
        var handlerHandle = T.Manager.Get(handler).Create();

        return IndexedEventDispatcherInterop.AddEventHandler(_handle, handlerHandle, index, priority);
    }

    public bool RemoveEventHandler(T handler, int index)
    {
        var reference = T.Manager.Get(handler);
        var handlerHandle = reference.Handle;

        if (!handlerHandle.HasValue)
        {
            return false;
        }

        if (IndexedEventDispatcherInterop.RemoveEventHandler(_handle, handlerHandle.Value, index))
        {
            reference.Free();
            return true;
        }

        return false;
    }

    public bool HasEventHandler(T handler, int index, out EventPriority priority)
    {
        var handlerHandle = T.Manager.Get(handler).Handle;
        priority = default;

        return handlerHandle.HasValue && IndexedEventDispatcherInterop.HasEventHandler(_handle, handlerHandle.Value, index, out priority);
    }
}