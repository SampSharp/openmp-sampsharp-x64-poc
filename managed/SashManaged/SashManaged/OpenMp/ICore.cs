namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct ICore
{
    public partial SemanticVersion GetVersion();

    public partial void SetData(SettableCoreDataType type, StringView data);

    public partial IPlayerPool GetPlayers();

    public partial int GetNetworkBitStreamVersion();
}
