namespace SashManaged.OpenMp;

public interface IComponentInterface<out T> where T : struct, IComponentInterface<T>
{
    static abstract UID ComponentId { get; }
    static abstract T FromHandle(nint handle);
}