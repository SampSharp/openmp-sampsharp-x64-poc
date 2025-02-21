using System.Collections;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.RobinHood;

[StructLayout(LayoutKind.Sequential)]
public readonly struct FlatPtrHashSet<T> : IReadOnlyCollection<T> where T : unmanaged
{
    private readonly nint _data;

    internal FlatPtrHashSet(IntPtr data)
    {
        _data = data;
    }

    public int Count => RobinHoodInterop.FlatPtrHashSet_size(_data).ToInt32();

    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    internal FlatPtrHashSetIterator Begin()
    {
        return RobinHoodInterop.FlatPtrHashSet_begin(_data);
    }

    internal FlatPtrHashSetIterator End()
    {
        return RobinHoodInterop.FlatPtrHashSet_end(_data);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public struct Enumerator : IEnumerator<T>
    {
        private readonly FlatPtrHashSet<T> _set;
        private FlatPtrHashSetIterator? _iterator;

        internal Enumerator(FlatPtrHashSet<T> set)
        {
            _set = set;
        }

        public bool MoveNext()
        {
            if (!_iterator.HasValue)
            {
                _iterator = _set.Begin();
                return _iterator != _set.End();
            }

            if (_iterator == _set.End())
            {
                return false;
            }

            var iter = _iterator.Value;
            iter.Advance();
            _iterator = iter;
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public T Current => _iterator?.Get<T>() ?? throw new InvalidOperationException();

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _iterator = null;
        }
    }
}