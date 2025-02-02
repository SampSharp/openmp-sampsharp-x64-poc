using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public interface IManagedExtension
{
    static abstract UID ExtensionId { get; }
}