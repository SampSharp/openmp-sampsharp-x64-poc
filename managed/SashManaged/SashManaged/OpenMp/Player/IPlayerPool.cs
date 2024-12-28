using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi2]
public readonly partial struct IPlayerPool
{
    public partial FlatPtrHashSet<IPlayer> Entries();
    public partial FlatPtrHashSet<IPlayer> Players();
    public partial FlatPtrHashSet<IPlayer> Bots();
    public partial IEventDispatcher2<IPlayerSpawnEventHandler> GetPlayerSpawnDispatcher();
    public partial IEventDispatcher2<IPlayerConnectEventHandler> GetPlayerConnectDispatcher();
    public partial IEventDispatcher2<IPlayerStreamEventHandler> GetPlayerStreamDispatcher();
    public partial IEventDispatcher2<IPlayerTextEventHandler> GetPlayerTextDispatcher();
    public partial IEventDispatcher2<IPlayerShotEventHandler> GetPlayerShotDispatcher();
    public partial IEventDispatcher2<IPlayerChangeEventHandler> GetPlayerChangeDispatcher();
    public partial IEventDispatcher2<IPlayerDamageEventHandler> GetPlayerDamageDispatcher();
    public partial IEventDispatcher2<IPlayerClickEventHandler> GetPlayerClickDispatcher();
    public partial IEventDispatcher2<IPlayerCheckEventHandler> GetPlayerCheckDispatcher();
    public partial IEventDispatcher2<IPlayerUpdateEventHandler> GetPlayerUpdateDispatcher();
    public partial IEventDispatcher2<IPlayerPoolEventHandler> GetPoolEventDispatcher();
    public partial bool IsNameTaken(StringView name, IPlayer skip);
    public partial void SendClientMessageToAll(ref Colour colour, StringView message);
    public partial void SendChatMessageToAll(IPlayer from, StringView message);
    public partial void SendGameTextToAll(StringView message, Milliseconds time, int style);
    public partial void HideGameTextForAll(int style);
    public partial void SendDeathMessageToAll(IPlayer killer, IPlayer killee, int weapon);
    public partial void SendEmptyDeathMessageToAll();
    public partial void CreateExplosionForAll(Vector3 vec, int type, float radius);
    public partial Pair<NewConnectionResult, IPlayer> RequestPlayer(ref PeerNetworkData netData, ref PeerRequestParams parms)    ;
    public partial void BroadcastPacket(SpanLite<byte> data, int channel, IPlayer skipFrom = default, bool dispatchEvents = true);
    public partial void BroadcastRPC(int id, SpanLite<byte> data, int channel, IPlayer skipFrom = default, bool dispatchEvents = true);
    public partial bool IsNameValid(StringView name);
    public partial void AllowNickNameCharacter(char character, bool allow);
    public partial bool IsNickNameCharacterAllowed(char character);
    public partial Colour GetDefaultColour(int pid);
}