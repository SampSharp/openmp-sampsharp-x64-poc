namespace SashManaged.OpenMp;

public interface IEventDispatcher<in T>
{
    bool AddEventHandler(T handler, EventPriority priority = EventPriority.Default);
    bool RemoveEventHandler(T handler);
    bool HasEventHandler(T handler, out EventPriority priority);
    Size Count();
}