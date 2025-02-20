namespace SampSharp.OpenMp.Core;

/// <summary>
/// This attribute marks an open.mp API interface struct as partial. Partial API structs will not have their component/
/// extension interfaces implemented by the source generator.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpApiPartialAttribute : Attribute;