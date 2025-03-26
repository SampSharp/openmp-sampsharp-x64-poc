using System.Diagnostics.Contracts;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IComponentList"/> interface.
/// </summary>
[OpenMpApi]
public readonly partial struct IComponentList
{
    [Pure]
    public partial IComponent QueryComponent(UID id);

    [Pure]
    public T QueryComponent<T>() where T : unmanaged, IComponentInterface
    {
        var component = QueryComponent(T.ComponentId).Handle;
        return StructPointer.AsStruct<T>(component);
    }
}