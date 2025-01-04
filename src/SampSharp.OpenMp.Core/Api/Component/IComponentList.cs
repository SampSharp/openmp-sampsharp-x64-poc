namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IComponentList
{
    public partial IComponent QueryComponent(UID id);

    public T QueryComponent<T>() where T : struct, IComponentInterface<T>
    {
        var component = QueryComponent(T.ComponentId).Handle;
        return T.FromHandle(component);
    }
}