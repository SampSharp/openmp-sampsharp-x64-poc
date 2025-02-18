namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPoolEventHandler<T> where T : unmanaged
{
    void OnPoolEntryCreated(T entry);
    void OnPoolEntryDestroyed(T entry);
};