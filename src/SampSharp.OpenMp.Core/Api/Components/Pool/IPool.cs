using System.Collections;
using SampSharp.OpenMp.Core.RobinHood;
using SampSharp.OpenMp.Core.Std;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPool{T}" /> interface.
/// </summary>
[OpenMpApi]
public readonly partial struct IPool<T> : IEnumerable<T> where T : unmanaged, IIDProvider.IManagedInterface
{
    public T Get(int index)
    {
        return ((IReadOnlyPool<T>)this).Get(index);
    }

    public (Size, Size) Bounds()
    {
        ((IReadOnlyPool<T>)this).Bounds(out var bounds);
        return bounds;
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
            _iterator = null;
        }

        public bool MoveNext()
        {
            if (!_iterator.HasValue)
            {
                _iterator = _pool.Begin();
                return _iterator != _pool.End();
            }

            
            var iter = _iterator.Value;
            iter.Advance();

            if (iter == _pool.End())
            {
                return false;
            }

            _iterator = iter;
            return true;
        }

        public void Reset()
        {
            throw new InvalidOperationException();
        }

        public readonly T Current => _iterator?.Current ?? throw new InvalidOperationException();

        readonly object IEnumerator.Current => Current;

        public void Dispose()
        {
            _iterator?.Dispose();
            _iterator = null;
        }
    }
}