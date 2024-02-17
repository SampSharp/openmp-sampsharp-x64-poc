namespace SashManaged.OpenMp;

public interface IIndexedEventDispatcher<in T>
{
    Size Count();
    Size Count(Size index);
    bool AddEventHandler(T handler, Size index, EventPriority priority = EventPriority.Default);
    bool RemoveEventHandler(T handler, Size index);
    bool HasEventHandler(T handler, Size index, out EventPriority priority);
}