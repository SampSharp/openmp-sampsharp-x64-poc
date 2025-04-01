namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerConnectDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerConnectEventHandler
{
    void OnIncomingConnection(IPlayer player, string ipAddress, ushort port);
    void OnPlayerConnect(IPlayer player);
    void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason);
    void OnPlayerClientInit(IPlayer player);
}