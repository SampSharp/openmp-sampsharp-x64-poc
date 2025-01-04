using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerClickEventHandler
{
    void OnPlayerClickMap(IPlayer player, Vector3 pos);
    void OnPlayerClickPlayer(IPlayer player, IPlayer clicked, PlayerClickSource source);
}