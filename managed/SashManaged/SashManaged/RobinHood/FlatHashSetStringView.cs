using System.Collections;
using System.Runtime.InteropServices;
using SashManaged.OpenMp;

namespace SashManaged;

[StructLayout(LayoutKind.Sequential)]
public readonly struct FlatHashSetStringView : IReadOnlyCollection<StringView>
{
    private readonly nint _data;

    private static unsafe StringView Dereference(ref FlatPtrHashSetIterator iterator)
    {
        return *(StringView*)iterator.Value;
    }

    public int Count => RobinHood.FlatHashSetStringView_size(_data).Value.ToInt32();

    public IEnumerator<StringView> GetEnumerator()
    {
        var iter = RobinHood.FlatHashSetStringView_begin(_data);

        while (iter != RobinHood.FlatHashSetStringView_end(_data))
        {
            yield return Dereference(ref iter);
            iter = RobinHood.FlatHashSetStringView_inc(iter);
        }
    }

    public void Emplace(StringView value)
    {
        RobinHood.FlatHashSetStringView_emplace(_data, value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}