using System.Diagnostics.Contracts;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IComponentList
{
    [Pure]
    public partial IComponent QueryComponent(UID id);

    [Pure]
    public T QueryComponent<T>() where T : unmanaged, IComponentInterface
    {
        var component = QueryComponent(T.ComponentId).Handle;
        return Pointer.AsStruct<T>(component);
    }
}