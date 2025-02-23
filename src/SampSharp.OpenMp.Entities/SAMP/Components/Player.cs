using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

public class Player : Component
{
    private readonly IPlayer _player;

    public IPlayer Native => _player;

    protected Player(IPlayer player)
    {
        _player = player;
    }
}
