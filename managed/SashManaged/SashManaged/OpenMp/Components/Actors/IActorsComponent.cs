using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IComponent))]
public readonly partial struct IActorsComponent
{
    public static UID ComponentId => new(0xc81ca021eae2ad5c);

    public partial IEventDispatcher2<IActorEventHandler> GetEventDispatcher();

    public partial IActor Create(int skin, Vector3 pos, float angle);
}