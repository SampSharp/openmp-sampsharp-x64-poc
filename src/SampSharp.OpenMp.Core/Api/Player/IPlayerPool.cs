using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IPlayerPool
{
    public partial FlatPtrHashSet<IPlayer> Entries();
    public partial FlatPtrHashSet<IPlayer> Players();
    public partial FlatPtrHashSet<IPlayer> Bots();
    public partial IEventDispatcher<IPlayerSpawnEventHandler> GetPlayerSpawnDispatcher();
    public partial IEventDispatcher<IPlayerConnectEventHandler> GetPlayerConnectDispatcher();
    public partial IEventDispatcher<IPlayerStreamEventHandler> GetPlayerStreamDispatcher();
    public partial IEventDispatcher<IPlayerTextEventHandler> GetPlayerTextDispatcher();
    public partial IEventDispatcher<IPlayerShotEventHandler> GetPlayerShotDispatcher();
    public partial IEventDispatcher<IPlayerChangeEventHandler> GetPlayerChangeDispatcher();
    public partial IEventDispatcher<IPlayerDamageEventHandler> GetPlayerDamageDispatcher();
    public partial IEventDispatcher<IPlayerClickEventHandler> GetPlayerClickDispatcher();
    public partial IEventDispatcher<IPlayerCheckEventHandler> GetPlayerCheckDispatcher();
    public partial IEventDispatcher<IPlayerUpdateEventHandler> GetPlayerUpdateDispatcher();
    // public partial IEventDispatcher<IPlayerPoolEventHandler> GetPoolEventDispatcher();
    public partial bool IsNameTaken(string name, IPlayer skip);
    public partial void SendClientMessageToAll(ref Colour colour, string message);
    public partial void SendChatMessageToAll(IPlayer from, string message);
    public partial void SendGameTextToAll(string message, Milliseconds time, int style);
    public partial void HideGameTextForAll(int style);
    public partial void SendDeathMessageToAll(IPlayer killer, IPlayer killee, int weapon);
    public partial void SendEmptyDeathMessageToAll();
    public partial void CreateExplosionForAll(Vector3 vec, int type, float radius);
    public partial Pair<NewConnectionResult, IPlayer> RequestPlayer(ref PeerNetworkData netData, ref PeerRequestParams parms)    ;
    public partial void BroadcastPacket(SpanLite<byte> data, int channel, IPlayer skipFrom = default, bool dispatchEvents = true);
    public partial void BroadcastRPC(int id, SpanLite<byte> data, int channel, IPlayer skipFrom = default, bool dispatchEvents = true);
    public partial bool IsNameValid(string name);
    public partial void AllowNickNameCharacter(char character, bool allow);
    public partial bool IsNickNameCharacterAllowed(char character);
    public partial Colour GetDefaultColour(int pid);
}