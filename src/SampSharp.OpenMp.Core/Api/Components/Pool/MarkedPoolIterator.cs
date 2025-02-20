using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public struct MarkedPoolIterator<T> : IDisposable, IEquatable<MarkedPoolIterator<T>> where T : unmanaged, IIDProviderInterface
{
    private IPool<T> _pool;
    private int _lockedId;
    private FlatPtrHashSet<T> _entries;
    private FlatPtrHashSetIterator _iter;

    public MarkedPoolIterator()
    {
        // the unmanaged MarkedPoolIterator constructor already initializes the iterator by locking
    }

    public void Dispose()
    {
        Unlock();
    }

    private void Lock()
    {
        Debug.Assert(_lockedId == -1);
        if (_iter != _entries.End())
        {
            _lockedId = Pointer.ToStruct<T>(_iter.Value).GetID();
            _pool.Lock(_lockedId);
        }
    }

    private void Unlock()
    {
        if (_lockedId != -1)
        {
            _pool.Unlock(_lockedId);
            _lockedId = -1;
        }
    }

    public T Current => Pointer.ToStruct<T>(_iter.Value);

    public bool Equals(MarkedPoolIterator<T> other)
    {
        return _iter.Equals(other._iter);
    }

    public override bool Equals(object? obj)
    {
        return obj is MarkedPoolIterator<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _iter.GetHashCode();
    }
    
    public static MarkedPoolIterator<T> operator ++(MarkedPoolIterator<T> self)
    {
        self._iter++;
        self.Unlock();
        self.Lock();
        return self;
    }

    public static bool operator ==(MarkedPoolIterator<T> a, MarkedPoolIterator<T> b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(MarkedPoolIterator<T> a, MarkedPoolIterator<T> b)
    {
        return !a.Equals(b);
    }
}