using System.Runtime.InteropServices.Marshalling;
using SampSharp.OpenMp.Core.Std;
using SampSharp.OpenMp.Core.Std.Chrono;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="INetwork" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible))]
public readonly partial struct INetwork
{
    public partial ENetworkType GetNetworkType();
    public partial IEventDispatcher<INetworkEventHandler> GetEventDispatcher();
    public partial IEventDispatcher<INetworkInEventHandler> GetInEventDispatcher();
    public partial IEventDispatcher<INetworkOutEventHandler> GetOutEventDispatcher();
    public partial bool SendPacket(IPlayer peer, SpanLite<byte> data, int channel, bool dispatchEvents = true);
    public partial bool BroadcastPacket(SpanLite<byte> data, int channel, IPlayer exceptPeer = default, bool dispatchEvents = true);
    public partial bool SendRPC(IPlayer peer, int id, SpanLite<byte> data, int channel, bool dispatchEvents = true);
    public partial bool BroadcastRPC(int id, SpanLite<byte> data, int channel, IPlayer exceptPeer = default, bool dispatchEvents = true);
    public partial NetworkStats GetStatistics(IPlayer player = default);
    public partial uint GetPing(IPlayer peer);
    public partial void Disconnect(IPlayer peer);
    public partial void Ban(BanEntry entry, [MarshalUsing(typeof(MillisecondsMarshaller))]TimeSpan expire);
    public partial void Unban(BanEntry entry);
    public partial void Update();
    public partial IIndexedEventDispatcher<ISingleNetworkInEventHandler> GetPerRPCInEventDispatcher();
    public partial IIndexedEventDispatcher<ISingleNetworkInEventHandler> GetPerPacketInEventDispatcher();
    public partial IIndexedEventDispatcher<ISingleNetworkOutEventHandler> GetPerRPCOutEventDispatcher();
    public partial IIndexedEventDispatcher<ISingleNetworkOutEventHandler> GetPerPacketOutEventDispatcher();
}