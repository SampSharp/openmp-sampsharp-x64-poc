using System.Net;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PeerAddress
{
    public readonly bool Ipv6;
    public readonly uint V4;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public readonly byte[] Bytes;

    public IPAddress ToIpAddress()
    {
        if (Ipv6)
        {
            Span<byte> buf = stackalloc byte[46]; // INET6_ADDRSTRLEN
            Bytes.CopyTo(buf);
            return new IPAddress(buf);
        }
        else
        {
            return new IPAddress(V4);
        }
    }
}