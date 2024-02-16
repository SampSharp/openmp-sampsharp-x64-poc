namespace SashManaged;

[AttributeUsage(AttributeTargets.Interface)]
public class OpenMpEventHandlerAttribute : Attribute
{
    /// <summary>
    /// The name of the open.mp event handler. Defaults to the interface name without the first character (if the interface name starts with an 'I').
    /// </summary>
    public string? HandlerName { get; set; }
}