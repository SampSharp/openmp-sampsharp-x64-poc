using System.Runtime.InteropServices;
using System.Text;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Explicit)]
public readonly unsafe struct HybridString25
{
    // First bit is 1 if dynamic and 0 if static; the rest are the length
    [FieldOffset(0)]
    private readonly Size _lenDynamic;

    //[FieldOffset(Size.Length)]
    //private readonly byte* _ptr;

    [FieldOffset(Size.Length)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst=25)]
    private readonly byte[]? _static;

    public HybridString25(string inp)
    {
        var q = Encoding.UTF8.GetBytes(inp);
        if (q.Length < 25)
        {
            _static = new byte[25];

            q.CopyTo(_static, 0);
            _lenDynamic = new Size(new IntPtr(((long)inp.Length << 1)));
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
}