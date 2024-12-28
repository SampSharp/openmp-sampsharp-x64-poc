namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IComponent))]
public readonly partial struct ICustomModelsComponent
{
    public static UID ComponentId => new(0x15E3CB1E7C77FFFF);

    public partial bool AddCustomModel(ModelType type, int id, int baseId, StringView dffName, StringView txdName, int virtualWorld = -1, byte timeOn = 0, byte timeOff = 0);
    public partial bool GetBaseModel(ref uint baseModelIdOrInput, ref uint customModel);
    public partial IEventDispatcher2<IPlayerModelsEventHandler> GetEventDispatcher();
    public partial StringView GetModelNameFromChecksum(uint checksum);
    public partial bool IsValidCustomModel(int modelId);
    public partial bool GetCustomModelPath(int modelId, ref StringView dffPath, ref StringView txdPath);
}