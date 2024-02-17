using System.Runtime.InteropServices;
using System.Text;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct StringView : ISpanFormattable
{
    internal readonly byte* _reference;
    private readonly Size _size;

    public StringView(byte* data, Size size)
    {
        _reference = data;
        _size = size;
    }

    public StringView(ReadOnlySpan<char> span) : this(MemoryMarshal.AsBytes(span))
    {
    }

    private StringView(ReadOnlySpan<byte> span)
    {
        fixed (byte* pin = &span[0])
        {
            // TODO: making the dangerous assumption that the span is permanently pinned (utf-8 literal)
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

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return Encoding.UTF8.TryGetChars(AsSpan(), destination, out charsWritten);
    }

    public static implicit operator string(StringView view)
    {
        return view.ToString();
    }

    public static implicit operator StringView(ReadOnlySpan<byte> span)
    {
        // TODO: this is terrible but will do for now.
        return new StringView(span);
    }
}