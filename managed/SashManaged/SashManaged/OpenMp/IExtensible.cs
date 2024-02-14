namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IExtensible
{
    public partial IExtension GetExtension(Uid id);
}