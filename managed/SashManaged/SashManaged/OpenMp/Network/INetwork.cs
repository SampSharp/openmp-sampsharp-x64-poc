﻿namespace SashManaged.OpenMp;

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
    public partial void Ban(ref BanEntry entry, Milliseconds expire);
    public partial void Unban(ref BanEntry entry);
    public partial void Update();
    
    // TODO: Indexed event dispatcher based not implemented
    // Implementing these with the current handler implementation is rather useless because the current implementation only allows a single
    // event handler to be added. This would only allow a handler for a single event. Would need to find a more dynamic implementation which allows
    // multiple handlers to be added.
    //public partial IIndexedEventDispatcher<ISingleNetworkInEventHandler> GetPerRPCInEventDispatcher();
    //public partial IIndexedEventDispatcher<ISingleNetworkInEventHandler> GetPerPacketInEventDispatcher();
    //public partial IIndexedEventDispatcher<ISingleNetworkOutEventHandler> GetPerRPCOutEventDispatcher();
    //public partial IIndexedEventDispatcher<ISingleNetworkOutEventHandler> GetPerPacketOutEventDispatcher();
}