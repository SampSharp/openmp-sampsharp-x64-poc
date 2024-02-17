namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtensible))]
public readonly partial struct INetwork
{
    public partial ENetworkType GetNetworkType();
    public partial IEventDispatcher<INetworkEventHandler> GetEventDispatcher();
    public partial IEventDispatcher<INetworkInEventHandler> GetInEventDispatcher();
    // TODO: public partial IIndexedEventDispatcher<SingleNetworkInEventHandler>& getPerRPCInEventDispatcher();
    // public partial IIndexedEventDispatcher<SingleNetworkInEventHandler>& getPerPacketInEventDispatcher();
    public partial IEventDispatcher<INetworkOutEventHandler> GetOutEventDispatcher();
    // TODO: public partial IIndexedEventDispatcher<SingleNetworkOutEventHandler>& getPerRPCOutEventDispatcher();
    // TODO: public partial IIndexedEventDispatcher<SingleNetworkOutEventHandler>& getPerPacketOutEventDispatcher();
    // TODO: public partial bool sendPacket(IPlayer& peer, Span<uint8_t> data, int channel, bool dispatchEvents = true);
    // TODO: public partial bool broadcastPacket(Span<uint8_t> data, int channel, const IPlayer* exceptPeer = nullptr, bool dispatchEvents = true);
    // TODO: public partial bool sendRPC(IPlayer& peer, int id, Span<uint8_t> data, int channel, bool dispatchEvents = true);
    // TODO: public partial bool broadcastRPC(int id, Span<uint8_t> data, int channel, const IPlayer* exceptPeer = nullptr, bool dispatchEvents = true);
    public partial NetworkStats GetStatistics(IPlayer player = default);
    public partial uint GetPing(IPlayer peer);
    public partial void Disconnect(IPlayer peer);
    public partial void Ban(ref BanEntry entry, Milliseconds expire);
    public partial void Unban(ref BanEntry entry);
    public partial void Update();
}