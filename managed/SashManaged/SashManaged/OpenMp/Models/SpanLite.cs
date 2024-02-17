using System.Runtime.InteropServices;

namespace SashManaged.OpenMp.Models;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct SpanLite<T> where T : unmanaged
{
    private readonly T* _data;
    private readonly Size _size;

    public Span<T> AsSpan()
    {
        return new Span<T>(_data, _size.Value.ToInt32());
    }
}