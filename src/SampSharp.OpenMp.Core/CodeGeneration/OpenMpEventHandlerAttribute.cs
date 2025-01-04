namespace SampSharp.OpenMp.Core;

/// <summary>
/// This attributes marks an interface as an open.mp event handler. The interface must be marked as partial.
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public class OpenMpEventHandlerAttribute : Attribute
{
    public string Library { get; set; } = "SampSharp";
    
    /// <summary>
    /// The name of the open.mp event handler. Defaults to the interface name without the first character (if the
    /// interface name starts with an 'I').
    /// </summary>
    public string? NativeTypeName { get; set; }
}