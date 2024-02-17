using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PeerNetworkData
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct NetworkID
    {
        public readonly PeerAddress address;
        public readonly ushort port;
    }

    public readonly INetwork network;
    public readonly NetworkID networkID;
}