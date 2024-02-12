using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IPlayer
{
    private readonly nint _data;

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern StringView IPlayer_getName(IPlayer player);

    public StringView GetName()
    {
        return IPlayer_getName(this);
    }
}