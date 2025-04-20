namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerClassData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerClassData
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0x185655ded843788b);

    public partial ref PlayerClass GetClass();
    public partial void SetSpawnInfo(ref PlayerClass info);
    public partial void SpawnPlayer();
}