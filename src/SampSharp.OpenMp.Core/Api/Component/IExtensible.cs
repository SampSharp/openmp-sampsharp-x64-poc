namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IExtensible
{
    public partial IExtension GetExtension(UID id);

    public T QueryExtension<T>() where T : IExtensionInterface<T>
    {
        var extension = GetExtension(T.ExtensionId).Handle;
        return T.FromHandle(extension);
    }
}
