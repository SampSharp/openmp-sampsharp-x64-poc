namespace SashManaged;

[AttributeUsage(AttributeTargets.Interface)]
public class OpenMpEventHandlerAttribute : Attribute
{
    /// <summary>
    /// The name of the open.mp event handler. Defaults to the interface name without the first character (if the interface name starts with an 'I').
    /// </summary>
    public string? HandlerName { get; set; }
}

[AttributeUsage(AttributeTargets.Interface)]
public class OpenMpEventHandler2Attribute : Attribute
{
    public string Library { get; set; } = "SampSharp";
    public string? NativeTypeName { get; set; }
}