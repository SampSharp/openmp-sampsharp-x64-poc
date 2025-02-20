using System.Collections;

namespace SampSharp.OpenMp.Core.Api;

public readonly struct IPool<T> : IEnumerable<T> where T : unmanaged, IIDProviderInterface
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

    public IEnumerator<T> GetEnumerator()
    {
        var iter = Begin();

        // TODO: non-alloc
        while (iter != End())
        {
            yield return iter.Current;
            iter++;
        }

        iter.Dispose();
    }

    public override int GetHashCode()
    {
        return _handle.GetHashCode();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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

    private FlatPtrHashSet<T> Entries()
    {
        return new FlatPtrHashSet<T>(IPoolInterop.IPool_entries(_handle));
    }

    public MarkedPoolIterator<T> Begin()
    {
        var entries = Entries();
        return new MarkedPoolIterator<T>(this, entries, entries.Begin());
    }

    public MarkedPoolIterator<T> End()
    {
        var entries = Entries();
        return new MarkedPoolIterator<T>(this, entries, entries.End());
    }

    public Size Count()
    {
        return IPoolInterop.IPool_count(_handle);
    }
}