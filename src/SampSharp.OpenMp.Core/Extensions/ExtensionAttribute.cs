using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ExtensionAttribute(ulong uid) : Attribute
{
    public UID Uid => new(uid);
}