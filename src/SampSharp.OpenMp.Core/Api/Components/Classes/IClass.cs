namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IClass" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible), typeof(IIDProvider))]
public readonly partial struct IClass
{
    public partial ref PlayerClass GetClass();
    public partial void SetClass(ref PlayerClass data);
}