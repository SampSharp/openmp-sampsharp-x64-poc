﻿namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IComponentList
{
    public partial IComponent QueryComponent(UID id);

    public T QueryComponent<T>() where T : struct, IComponentInterface<T> // TODO: IComponentInterface in v2? same for extension probably
    {
        var component = QueryComponent(T.ComponentId).Handle;
        return T.FromHandle(component);
    }
}