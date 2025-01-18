using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct SpanLite<T> where T : unmanaged
{
    private readonly T* _data;
    private readonly Size _size;

    public SpanLite(T* data, Size size)
    {
        _data = data;
        _size = size;
    }

    public Span<T> AsSpan()
    {
        return new Span<T>(_data, _size.Value.ToInt32());
    }
}