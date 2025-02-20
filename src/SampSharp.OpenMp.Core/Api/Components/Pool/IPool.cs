using System.Collections;
using System.Runtime.InteropServices;

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

    public MarkedPoolIterator<T> Begin()
    {
        //  TODO FIXME: This somehow (?) works? The C++ code handles `data` as a pointer to IPool<IDProvider> while in
        // reality it would be a pointer to IPool<IVehicle> or some other type. While this doesn't look wrong at first
        // glance, IVehicle has base types IExtensible, IEntity while IEntity has IIDProvider as a base type. This
        // means that the pointers contained in the pool are not actually pointers to IIDProvider. When begin() or end()
        // is called to get an iterator on the pool, a MarkedPoolIterator is created which will lock the pool at the
        // given entry, which will place a lock by calling getID on the current iterator entry. The entry is of type
        // `Type` which is a template parameter of the pool. In our case this is IIDProvider while in reality it should
        // be IVehicle. This means the wrong vtable is accessed, and we end up with UB.
        var data =  IPoolInterop.IPool_begin(_handle);
        return Pointer.TypeCast<MarkedPoolIteratorData, MarkedPoolIterator<T>>(data);
    }

    public MarkedPoolIterator<T> End()
    {
        // TODO: FIXME: See Begin()
        var data = IPoolInterop.IPool_end(_handle);
        return Pointer.TypeCast<MarkedPoolIteratorData, MarkedPoolIterator<T>>(data);
    }

    public Size Count()
    {
        return IPoolInterop.IPool_count(_handle);
    }
}