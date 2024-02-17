namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtensible))]
public readonly partial struct INetwork
{
    public partial ENetworkType GetNetworkType();
    public partial IEventDispatcher<INetworkEventHandler> GetEventDispatcher();
    public partial IEventDispatcher<INetworkInEventHandler> GetInEventDispatcher();
    // TODO: public partial IIndexedEventDispatcher<SingleNetworkInEventHandler>& getPerRPCInEventDispatcher();
    // TODO: public partial IIndexedEventDispatcher<SingleNetworkInEventHandler>& getPerPacketInEventDispatcher();
    public partial IEventDispatcher<INetworkOutEventHandler> GetOutEventDispatcher();
    // TODO: public partial IIndexedEventDispatcher<SingleNetworkOutEventHandler>& getPerRPCOutEventDispatcher();
    // TODO: public partial IIndexedEventDispatcher<SingleNetworkOutEventHandler>& getPerPacketOutEventDispatcher();
    public partial bool SendPacket(IPlayer peer, SpanLite<byte> data, int channel, bool dispatchEvents = true);
    public partial bool BroadcastPacket(SpanLite<byte> data, int channel, IPlayer exceptPeer = default, bool dispatchEvents = true);
    public partial bool SendRPC(IPlayer peer, int id, SpanLite<byte> data, int channel, bool dispatchEvents = true);
    public partial bool BroadcastRPC(int id, SpanLite<byte> data, int channel, IPlayer exceptPeer = default, bool dispatchEvents = true);
    public partial NetworkStats GetStatistics(IPlayer player = default);
    public partial uint GetPing(IPlayer peer);
    public partial void Disconnect(IPlayer peer);
    public partial void Ban(ref BanEntry entry, Milliseconds expire);
    public partial void Unban(ref BanEntry entry);
    public partial void Update();
}