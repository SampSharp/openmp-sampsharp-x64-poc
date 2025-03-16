using System.Net;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PeerNetworkData
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct NetworkID
    {
        public readonly PeerAddress address;
        public readonly ushort port;

        public IPEndPoint ToEndpoint()
        {
            return new IPEndPoint(address.ToAddress(), port);
        }
    }

    public readonly INetwork network;
    public readonly NetworkID networkID;
}