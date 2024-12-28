using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerClickEventHandler : IEventHandler2
{
    void OnPlayerClickMap(IPlayer player, Vector3 pos);
    void OnPlayerClickPlayer(IPlayer player, IPlayer clicked, PlayerClickSource source);
}