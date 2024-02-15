using System.Runtime.InteropServices;
using System.Text;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Explicit)]
public readonly unsafe struct HybridString46
{
    // First bit is 1 if dynamic and 0 if static; the rest are the length
    [FieldOffset(0)]
    private readonly Size _lenDynamic;

    //[FieldOffset(Size.Length)]
    //private readonly byte* _ptr;

    [FieldOffset(Size.Length)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 46)]
    private readonly byte[]? _static;

    public HybridString46(string inp)
    {
        var q = Encoding.UTF8.GetBytes(inp);
        if (q.Length < 46)
        {
            _static = new byte[46];

            q.CopyTo(_static, 0);
            _lenDynamic = new Size(new nint((long)inp.Length << 1));
        }
        else
        {
            _static = null;
        }
    }

    public Span<byte> AsSpan()
    {
        var value = _lenDynamic.Value.ToInt64();
        var flag = (value & 1) != 0;
        var length = value >> 1;

        if (flag)
        {
            //return new Span<byte>(_ptr, (int)length);
        }

        return new Span<byte>(_static, 0, (int)length);
    }

    public override string ToString()
    {
        return Encoding.UTF8.GetString(AsSpan());
    }
}
[StructLayout(LayoutKind.Explicit)]
public readonly unsafe struct HybridString16
{
    // First bit is 1 if dynamic and 0 if static; the rest are the length
    [FieldOffset(0)]
    private readonly Size _lenDynamic;

    //[FieldOffset(Size.Length)]
    //private readonly byte* _ptr;

    [FieldOffset(Size.Length)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    private readonly byte[]? _static;

    public HybridString16(string inp)
    {
        var q = Encoding.UTF8.GetBytes(inp);
        if (q.Length < 16)
        {
            _static = new byte[16];

            q.CopyTo(_static, 0);
            _lenDynamic = new Size(new nint((long)inp.Length << 1));
        }
        else
        {
            _static = null;
        }
    }

    public Span<byte> AsSpan()
    {
        var value = _lenDynamic.Value.ToInt64();
        var flag = (value & 1) != 0;
        var length = value >> 1;

        if (flag)
        {
            //return new Span<byte>(_ptr, (int)length);
        }

        return new Span<byte>(_static, 0, (int)length);
    }

    public override string ToString()
    {
        return Encoding.UTF8.GetString(AsSpan());
    }
}
[StructLayout(LayoutKind.Explicit)]
public readonly unsafe struct HybridString24
{
    // First bit is 1 if dynamic and 0 if static; the rest are the length
    [FieldOffset(0)]
    private readonly Size _lenDynamic;

    //[FieldOffset(Size.Length)]
    //private readonly byte* _ptr;

    [FieldOffset(Size.Length)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
    private readonly byte[]? _static;

    public HybridString24(string inp)
    {
        var q = Encoding.UTF8.GetBytes(inp);
        if (q.Length < 24)
        {
            _static = new byte[24];

            q.CopyTo(_static, 0);
            _lenDynamic = new Size(new nint((long)inp.Length << 1));
        }
        else
        {
            _static = null;
        }
    }

    public Span<byte> AsSpan()
    {
        var value = _lenDynamic.Value.ToInt64();
        var flag = (value & 1) != 0;
        var length = value >> 1;

        if (flag)
        {
            //return new Span<byte>(_ptr, (int)length);
        }

        return new Span<byte>(_static, 0, (int)length);
    }

    public override string ToString()
    {
        return Encoding.UTF8.GetString(AsSpan());
    }
}