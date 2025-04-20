using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IActorsComponent" /> interface.
/// </summary>
[OpenMpApi(typeof(IPoolComponent<IActor>))]
public readonly partial struct IActorsComponent
{
    /// <inheritdoc />
    public static UID ComponentId => new(0xc81ca021eae2ad5c);

    public partial IEventDispatcher<IActorEventHandler> GetEventDispatcher();

    public partial IActor Create(int skin, Vector3 pos, float angle);
}