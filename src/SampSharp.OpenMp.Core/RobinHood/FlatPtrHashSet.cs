using System.Collections;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct FlatPtrHashSet<T> : IReadOnlyCollection<T> where T : unmanaged
{
    private readonly nint _data;

    internal FlatPtrHashSet(IntPtr data)
    {
        _data = data;
    }

    private static unsafe T Dereference(ref FlatPtrHashSetIterator iterator)
    {
        return *(T*)iterator.Value;
    }

    public int Count => RobinHood.FlatPtrHashSet_size(_data).ToInt32();

    public IEnumerator<T> GetEnumerator()
    {
        // TODO: non alloc
        var iter = Begin();
        while (iter != End())
        {
            yield return Dereference(ref iter);
            iter++;
        }
    }

    internal FlatPtrHashSetIterator Begin()
    {
        return RobinHood.FlatPtrHashSet_begin(_data);
    }

    internal FlatPtrHashSetIterator End()
    {
        return RobinHood.FlatPtrHashSet_end(_data);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}