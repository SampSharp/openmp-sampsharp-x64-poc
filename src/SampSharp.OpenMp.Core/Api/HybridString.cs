﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SampSharp.OpenMp.Core.Api;

[NumberedTypeGenerator(nameof(Size), 24)]
[NumberedTypeGenerator(nameof(Size), 25)]
[NumberedTypeGenerator(nameof(Size), 32)]
[NumberedTypeGenerator(nameof(Size), 46)]
[StructLayout(LayoutKind.Explicit)]
[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public readonly struct HybridString16
{
    private const int Size = 16;
        
    // First bit is 1 if dynamic and 0 if static; the rest are the length
    [FieldOffset(0)] private readonly Size _lenDynamic;
        
    [FieldOffset(Core.Size.Length), MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
    private readonly byte[]? _static;
        
    public HybridString16(string? inp)
    {
        if (inp == null)
        {
            _static = new byte[Size];
        }
        else
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
    }

    public void CopyTo(Span<byte> dest)
    {
        MemoryMarshal.Cast<byte, Size>(dest)[0] = Length;
        AsSpan().CopyTo(dest[Core.Size.Length..]);
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

    internal string DebuggerDisplay()
    {
        string value;

        try
        {
            value = ToString();
        }
        catch
        {
            value = "<error>";
        }
        return $"HybridString<{Size}> {{Length = {Length}, IsDynamic = {IsDynamic}, Value = {value}}}";
    }

    private static Encoding Encoding => Encoding.UTF8;
}