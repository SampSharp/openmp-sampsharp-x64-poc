namespace SashManaged.OpenMp;

public interface IPoolEventHandler<in T> where T : unmanaged
{
    void OnPoolEntryCreated(T entry);
    void OnPoolEntryDestroyed(T entry);
};