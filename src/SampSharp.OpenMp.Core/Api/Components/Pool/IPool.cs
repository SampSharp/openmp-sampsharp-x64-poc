using System.Collections;

namespace SampSharp.OpenMp.Core.Api;

public readonly struct IPool<T> : IEnumerable<T> where T : unmanaged, IIDProviderInterface
{
    private readonly nint _handle;

    public IPool(nint handle)
    {
        _handle = handle;
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

    public Size Count()
    {
        return IPoolInterop.IPool_count(_handle);
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

    private MarkedPoolIterator<T> Begin()
    {
        var entries = Entries();
        return new MarkedPoolIterator<T>(this, entries, entries.Begin());
    }

    private MarkedPoolIterator<T> End()
    {
        var entries = Entries();
        return new MarkedPoolIterator<T>(this, entries, entries.End());
    }

    public override int GetHashCode()
    {
        return _handle.GetHashCode();
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static explicit operator IReadOnlyPool<T>(IPool<T> value)
    {
        return new IReadOnlyPool<T>(value._handle);
    }

    public struct Enumerator : IEnumerator<T>
    {
        private readonly IPool<T> _pool;
        private MarkedPoolIterator<T>? _iterator;

        internal Enumerator(IPool<T> pool)
        {
            _pool = pool;
        }

        public bool MoveNext()
        {
            if (!_iterator.HasValue)
            {
                _iterator = _pool.Begin();
                return _iterator != _pool.End();
            }

            if (_iterator == _pool.End())
            {
                return false;
            }

            _iterator++;
            return true;
        }

        public void Reset()
        {
            throw new InvalidOperationException();
        }

        public T Current => _iterator?.Current ?? throw new InvalidOperationException();

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _iterator?.Dispose();
            _iterator = null;
        }
    }
}