using System.Collections;
using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core.RobinHood;

[StructLayout(LayoutKind.Sequential)]
public readonly struct FlatHashSetStringView : IReadOnlyCollection<string?>
{
    private readonly nint _data;

    public int Count => RobinHoodInterop.FlatHashSetStringView_size(_data).Value.ToInt32();

    public IEnumerator<string?> GetEnumerator()
    {
        var iter = RobinHoodInterop.FlatHashSetStringView_begin(_data);

        while (iter != RobinHoodInterop.FlatHashSetStringView_end(_data))
        {
            yield return iter.Get<StringView>().ToString();
            iter = RobinHoodInterop.FlatHashSetStringView_inc(iter);
        }
    }

    public void Emplace(string value)
    {
        scoped StringViewMarshaller.ManagedToNative valueMarshaller = new();

        try
        {
            Span<byte> buffer = stackalloc byte[StringViewMarshaller.ManagedToNative.BufferSize];
            valueMarshaller.FromManaged(value, buffer);
            var valueMarshalled = valueMarshaller.ToUnmanaged();

            RobinHoodInterop.FlatHashSetStringView_emplace(_data, valueMarshalled);
        }
        finally
        {
            valueMarshaller.Free();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}