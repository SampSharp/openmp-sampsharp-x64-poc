namespace SampSharp.OpenMp.Core.Api;

public interface IExtensionInterface<out T>
{
    static abstract UID ExtensionId { get; }
    static abstract T FromHandle(nint handle);
}