namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct ICustomModelsComponent
{
    public static UID ComponentId => new(0x15E3CB1E7C77FFFF);

    public partial bool AddCustomModel(ModelType type, int id, int baseId, string dffName, string txdName, int virtualWorld = -1, byte timeOn = 0, byte timeOff = 0);
    public partial bool GetBaseModel(ref uint baseModelIdOrInput, ref uint customModel);
    public partial IEventDispatcher<IPlayerModelsEventHandler> GetEventDispatcher();
    public partial string GetModelNameFromChecksum(uint checksum);
    public partial bool IsValidCustomModel(int modelId);
    public partial bool GetCustomModelPath(int modelId, ref string dffPath, ref string txdPath);
}