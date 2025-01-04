using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PeerAddress
{
    public readonly bool Ipv6;
    public readonly uint V4;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly byte[] Bytes;
}