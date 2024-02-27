namespace SashManaged;

[Obsolete("This should disappear in v2. This is replaced with MarshalUsingAttribute.")]
// In v1 means that memory is allocated on the heap and the struct is marshalled via StructureToPtr instead of default DllImport marshalling.
[AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
public class OpenMpApiMarshallAttribute : Attribute;