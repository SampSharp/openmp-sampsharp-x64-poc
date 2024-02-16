namespace SashManaged.SourceGenerator;

public static class Constants
{
    public const string SequentialStructLayoutAttribute = "[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]";

    public const string MarshallAttributeFQN = "SashManaged.OpenMpApiMarshallAttribute";

    public const string ApiAttributeFQN = "SashManaged.OpenMpApiAttribute";

    public const string EventHandlerAttributeFQN = "SashManaged.OpenMpEventHandlerAttribute";

    public const string OverloadAttributeFQN = "SashManaged.OpenMpApiOverloadAttribute";

    public const string ComponentFQN = "SashManaged.OpenMp.IComponent";

    public const string ComponentInterfaceFQN = "SashManaged.OpenMp.IComponentInterface";

    public const string ExtensionFQN = "SashManaged.OpenMp.IExtension";

    public const string ExtensionInterfaceFQN = "SashManaged.OpenMp.IExtensionInterface";
    
    public const string BlittableBooleanFQN = "SashManaged.BlittableBoolean";

    public const string BlittableRefFQN = "SashManaged.BlittableRef";
}