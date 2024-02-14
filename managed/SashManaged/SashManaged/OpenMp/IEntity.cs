namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IEntity
{
    public partial GtaVector3 GetPosition();

    public partial void SetPosition(GtaVector3 position);

    public partial GTAQuat GetRotation();

    public partial void SetRotation(GTAQuat rotation);

    public partial int GetVirtualWorld();

    public partial void SetVirtualWorld(int vw);
}