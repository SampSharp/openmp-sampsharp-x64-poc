using System.Numerics;
using SashManaged.Chrono;
using SashManaged.RobinHood;

namespace SashManaged.OpenMp;

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
    // TODO: public partial IEventDispatcher<PoolEventHandler<IPlayer>> GetPoolEventDispatcher()    ;

    public partial bool IsNameTaken(StringView name, IPlayer skip);
    public partial void SendClientMessageToAll(ref Colour colour, StringView message);
    public partial void SendChatMessageToAll(IPlayer from, StringView message);
    public partial void SendGameTextToAll(StringView message, Milliseconds time, int style);
    public partial void HideGameTextForAll(int style);
    public partial void SendDeathMessageToAll(IPlayer killer, IPlayer killee, int weapon);
    public partial void SendEmptyDeathMessageToAll();
    public partial void CreateExplosionForAll(Vector3 vec, int type, float radius);

    // TODO: public partial Pair<NewConnectionResult, IPlayer*> requestPlayer(const PeerNetworkData& netData, const PeerRequestParams& params)    ;
    // TODO: public partial void broadcastPacket(Span<uint8_t> data, int channel, const IPlayer* skipFrom = nullptr, bool dispatchEvents = true)    ;
    // TODO: public partial void broadcastRPC(int id, Span<uint8_t> data, int channel, const IPlayer* skipFrom = nullptr, bool dispatchEvents = true)    ;
    public partial bool IsNameValid(StringView name);
    public partial void AllowNickNameCharacter(char character, bool allow);
    public partial bool IsNickNameCharacterAllowed(char character);
    public partial Colour GetDefaultColour(int pid);
}