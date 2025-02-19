using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

public readonly struct IPool<T> where T : unmanaged
{
    private readonly nint _handle;

    public IPool(nint handle)
    {
        _handle = handle;
    }

    public nint Handle => _handle;

    public static explicit operator IReadOnlyPool<T>(IPool<T> value)
    {
        return new IReadOnlyPool<T>(value.Handle);
    }

    public override int GetHashCode()
    {
        return _handle.GetHashCode();
    }

    public T Get(int index)
    {
        return ((IReadOnlyPool<T>)this).Get(index);
    }

    public void Bounds(out Pair<Size, Size> bounds)
    {
        ((IReadOnlyPool<T>)this).Bounds(out bounds);
    }

    public void Release(int index)
    {
        IPoolInterop.IPool_release(_handle, index);
    }

    public void Lock(int index)
    {
        IPoolInterop.IPool_lock(_handle, index);
    }

    public bool Unlock(int index)
    {
        return IPoolInterop.IPool_unlock(_handle, index);
    }

    public IEventDispatcher<IPoolEventHandler<T>> GetPoolEventDispatcher()
    {
        var data = IPoolInterop.IPool_getPoolEventDispatcher(_handle);
        return new IEventDispatcher<IPoolEventHandler<T>>(data);
    }

    public MarkedPoolIterator<T> Begin()
    {
        var data =  IPoolInterop.IPool_begin(_handle);
        Union un = default;
        un.Iter2 = data;
        return un.Iter1;
    }

    public MarkedPoolIterator<T> End()
    {
        var data = IPoolInterop.IPool_end(_handle);
        Union un = default;
        un.Iter2 = data;
        return un.Iter1;
    }

    public Size Count()
    {
        return IPoolInterop.IPool_count(_handle);
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct Union
    {
        [FieldOffset(0)] public MarkedPoolIterator<T> Iter1;
        [FieldOffset(0)] public MarkedPoolIteratorData Iter2;
    }
}