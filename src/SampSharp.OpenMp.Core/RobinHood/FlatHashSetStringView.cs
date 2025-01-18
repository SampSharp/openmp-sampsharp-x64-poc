using System.Collections;
using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct FlatHashSetStringView : IReadOnlyCollection<string?>
{
    private readonly nint _data;

    private static unsafe StringView Dereference(ref FlatPtrHashSetIterator iterator)
    {
        return *(StringView*)iterator.Value;
    }

    public int Count => RobinHood.FlatHashSetStringView_size(_data).Value.ToInt32();

    public IEnumerator<string?> GetEnumerator()
    {
        var iter = RobinHood.FlatHashSetStringView_begin(_data);

        while (iter != RobinHood.FlatHashSetStringView_end(_data))
        {
            yield return Dereference(ref iter).ToString();
            iter = RobinHood.FlatHashSetStringView_inc(iter);
        }
    }

    public unsafe void Emplace(string value)
    {
        scoped StringViewMarshaller.ManagedToNative valueMarshaller = new();
        Span<byte> buffer = stackalloc byte[StringViewMarshaller.ManagedToNative.BufferSize];
        valueMarshaller.FromManaged(value, buffer);

        var valueMarshalled = valueMarshaller.ToUnmanaged();

        RobinHood.FlatHashSetStringView_emplace(_data, valueMarshalled);

        valueMarshaller.Free();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}