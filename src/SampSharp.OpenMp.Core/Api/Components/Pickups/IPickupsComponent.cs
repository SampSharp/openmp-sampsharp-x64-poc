using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPickupsComponent"/> interface.
/// </summary>
[OpenMpApi(typeof(IPoolComponent<IPickup>))]
public readonly partial struct IPickupsComponent
{
    public static UID ComponentId => new(0xcf304faa363dd971);

    public partial IEventDispatcher<IPickupEventHandler> GetEventDispatcher();
    public partial IPickup Create(int modelId, byte type, Vector3 pos, uint virtualWorld, bool isStatic);
    public partial int ToLegacyID(int real);
    public partial int FromLegacyID(int legacy);
    public partial void ReleaseLegacyID(int legacy);
    public partial int ReserveLegacyID();
    public partial void SetLegacyID(int legacy, int real);
}