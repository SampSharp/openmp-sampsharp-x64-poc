namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerClassData
{
    public static UID ExtensionId => new(0x185655ded843788b);

    public partial ref PlayerClass GetClass();
    public partial void SetSpawnInfo(ref PlayerClass info);
    public partial void SpawnPlayer();
}