using System.Collections;
using System.Runtime.InteropServices;

namespace SashManaged.RobinHood;

[StructLayout(LayoutKind.Sequential)]
public readonly struct FlatPtrHashSet<T> : IEnumerable<T> where T : unmanaged
{
    private readonly nint _data;

    private static unsafe T Dereference(ref FlatPtrHashSetIterator iterator)
    {
        return *(T*)iterator._keyVals;
    }

    public int Count => FlatPtrHashSetInterop.FlatPtrHashSet_size(_data).Value.ToInt32();

    public IEnumerator<T> GetEnumerator()
    {
        var iter = FlatPtrHashSetInterop.FlatPtrHashSet_begin(_data);

        while (iter != FlatPtrHashSetInterop.FlatPtrHashSet_end(_data))
        {
            yield return Dereference(ref iter);
            iter = FlatPtrHashSetInterop.FlatPtrHashSet_inc(iter);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}