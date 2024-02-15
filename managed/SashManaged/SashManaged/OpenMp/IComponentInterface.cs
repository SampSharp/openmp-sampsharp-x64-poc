namespace SashManaged.OpenMp;

public interface IComponentInterface<out T>
{
    static abstract UID ComponentId { get; }
    static abstract T FromHandle(nint handle);
}