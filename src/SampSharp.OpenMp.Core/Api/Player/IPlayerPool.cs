using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using SampSharp.OpenMp.Core.RobinHood;
using SampSharp.OpenMp.Core.Std;
using SampSharp.OpenMp.Core.Std.Chrono;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerPool" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible), typeof(IReadOnlyPool<IPlayer>))]
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
    public partial IEventDispatcher<IPoolEventHandler<IPlayer>> GetPoolEventDispatcher();
    public partial bool IsNameTaken(string name, IPlayer skip);
    public partial void SendClientMessageToAll(ref Colour colour, string message);
    public partial void SendChatMessageToAll(IPlayer from, string message);
    public partial void SendGameTextToAll(string message, [MarshalUsing(typeof(MillisecondsMarshaller))]TimeSpan time, int style);
    public partial void HideGameTextForAll(int style);
    public partial void SendDeathMessageToAll(IPlayer killer, IPlayer killee, int weapon);
    public partial void SendEmptyDeathMessageToAll();
    public partial void CreateExplosionForAll(Vector3 vec, int type, float radius);
    private partial void RequestPlayer(ref PeerNetworkData netData, ref PeerRequestParams parms, out Pair<NewConnectionResult, IPlayer> result);

    public (NewConnectionResult, IPlayer) RequestPlayer(ref PeerNetworkData netData, ref PeerRequestParams parms)
    {
        RequestPlayer(ref netData, ref parms, out var result);
        return result;
    }

    public partial void BroadcastPacket(SpanLite<byte> data, int channel, IPlayer skipFrom = default, bool dispatchEvents = true);
    public partial void BroadcastRPC(int id, SpanLite<byte> data, int channel, IPlayer skipFrom = default, bool dispatchEvents = true);
    public partial bool IsNameValid(string name);
    public partial void AllowNickNameCharacter(char character, bool allow);
    public partial bool IsNickNameCharacterAllowed(char character);
    public partial Colour GetDefaultColour(int pid);

    public IReadOnlyPool<IPlayer> AsPool()
    {
        return (IReadOnlyPool<IPlayer>)this;
    }
}