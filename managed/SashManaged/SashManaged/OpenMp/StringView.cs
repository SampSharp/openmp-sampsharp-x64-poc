using System.Runtime.InteropServices;
using System.Text;
namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe ref struct StringView
{
    private readonly byte* _reference;
    private readonly Size _size;

    public StringView(byte* data, Size size)
    {
        _reference = data;
        _size = size;
    }

    public StringView(ReadOnlySpan<char> span) : this(MemoryMarshal.AsBytes(span))
    {
    }

    public StringView(ReadOnlySpan<byte> span)
    {
        fixed (byte* pin = &span[0])
        {
            // making the dangerous assumption that the span is pinnable
            _reference = pin;
            _size = span.Length;
        }
    }

    public static StringView Create(byte* data, Size length)
    {
        return new StringView(data, length);
    }

    public ReadOnlySpan<byte> AsSpan()
    {
        return new ReadOnlySpan<byte>(_reference, _size.Value.ToInt32());
    }

    public override string ToString()
    {
        return Encoding.UTF8.GetString(AsSpan());
    }
}