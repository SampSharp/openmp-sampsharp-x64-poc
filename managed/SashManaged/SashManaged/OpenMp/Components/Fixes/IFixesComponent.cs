using SashManaged.Chrono;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct IFixesComponent
{
    public static UID ComponentId => new(0xb5c615eff0329ff7);

    public partial bool SendGameTextToAll(StringView message, Milliseconds time, int style);
    public partial bool HideGameTextForAll(int style);
    public partial void ClearAnimation(IPlayer player, IActor actor);
}