﻿namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IPlayerConnectEventHandler
{
    void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port);
    void OnPlayerConnect(IPlayer player);
    void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason);
    void OnPlayerClientInit(IPlayer player);
}