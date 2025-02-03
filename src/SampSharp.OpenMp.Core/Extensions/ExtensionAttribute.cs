using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

// TODO: Add diagnostic for missing attribute.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ExtensionAttribute(ulong uid) : Attribute
{
    public UID Uid => new(uid);
}