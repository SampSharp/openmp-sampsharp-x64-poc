using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerClickEventHandler
{
    void OnPlayerClickMap(IPlayer player, Vector3 pos);
    void OnPlayerClickPlayer(IPlayer player, IPlayer clicked, PlayerClickSource source);
}