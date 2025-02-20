namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IComponentList
{
    public partial IComponent QueryComponent(UID id);

    public T QueryComponent<T>() where T : unmanaged, IComponentInterface
    {
        var component = QueryComponent(T.ComponentId).Handle;
        return Pointer.AsStruct<T>(component);
    }
}