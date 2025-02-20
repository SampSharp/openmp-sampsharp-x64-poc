using System.Diagnostics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IComponentList
{
    public partial IComponent QueryComponent(UID id);

    public T QueryComponent<T>() where T : unmanaged, IComponentInterface<T>
    {
        var component = QueryComponent(T.ComponentId).Handle;
        var a = Pointer.AsStruct<T>(component);
        var b = T.FromHandle(component);
        //Debug.Assert(Equals(a, b));
        return b;
    }
}