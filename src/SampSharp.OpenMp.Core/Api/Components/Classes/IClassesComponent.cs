using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IClassesComponent" /> interface.
/// </summary>
[OpenMpApi(typeof(IPoolComponent<IClass>))]
public readonly partial struct IClassesComponent
{
    /// <inheritdoc />
    public static UID ComponentId => new(0x8cfb3183976da208);

    public partial IEventDispatcher<IClassEventHandler> GetEventDispatcher();

    public partial IClass Create(int skin, int team, Vector3 spawn, float angle, ref WeaponSlots weapons);
}