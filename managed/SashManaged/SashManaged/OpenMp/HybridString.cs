using System.Runtime.InteropServices;
using System.Text;

namespace SashManaged.OpenMp;

[NumberedTypeGenerator(nameof(Size), 24)]
[NumberedTypeGenerator(nameof(Size), 25)]
[NumberedTypeGenerator(nameof(Size), 32)]
[NumberedTypeGenerator(nameof(Size), 46)]
[StructLayout(LayoutKind.Explicit)]
public readonly struct HybridString16
{
    private const int Size = 16;
        
    // First bit is 1 if dynamic and 0 if static; the rest are the length
    [FieldOffset(0)] private readonly Size _lenDynamic;
        
    [FieldOffset(OpenMp.Size.Length)] [MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
    private readonly byte[]? _static;
        
    public HybridString16(string inp)
    {
        var requiredSize = Encoding.GetByteCount(inp);
        if (requiredSize < Size) // last byte is for null terminator
        {
            _static = new byte[Size];
            Encoding.GetBytes(inp, 0, inp.Length, _static, 0);

            _lenDynamic = new Size(new nint((long)inp.Length << 1));
        }
        else
        {
            throw new NotImplementedException("dynamic string size not implemented");
        }
    }

    public bool IsDynamic => (_lenDynamic.Value.ToInt64() & 1) != 0;

    public int Length => (int)(_lenDynamic.Value.ToInt64() >> 1);

    public unsafe Span<byte> AsSpan()
    {
        return IsDynamic 
            ? new Span<byte>(GetDynamicStorage().Data, Length) 
            : new Span<byte>(_static, 0, Length);
    }

    private unsafe HybridStringDynamicStorage GetDynamicStorage()
    {
        fixed (byte* ptr = _static)
        {
            return *(HybridStringDynamicStorage*)ptr;
        }
    }

    public override string ToString()
    {
        return Encoding.GetString(AsSpan());
    }

    private static Encoding Encoding => Encoding.UTF8;
}