using System;
using System.Runtime.InteropServices;

namespace SashManaged.SourceGenerator;

public static class Constants
{
    // System
    public const string SpanOfBytesFQN = "System.Span<byte>";

    public const string IEquatableFQN = "System.IEquatable";
    
    public const string MarshalUsingAttributeFQN = "System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute";

    public const string NativeMarshallingAttributeFQN = "System.Runtime.InteropServices.Marshalling.NativeMarshallingAttribute";

    public const string CustomMarshallerAttributeFQN = "System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute";

    public const string GenericPlaceholderFQN = "System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute.GenericPlaceholder";

    // SashManaged.OpenMp
    public const string EventHandlerFQN = "SashManaged.OpenMp.IEventHandler";
    
    public const string ExtensionFQN = "SashManaged.OpenMp.IExtension";

    public const string ExtensionInterfaceFQN = "SashManaged.OpenMp.IExtensionInterface";
    
    public const string ComponentFQN = "SashManaged.OpenMp.IComponent";

    public const string ComponentInterfaceFQN = "SashManaged.OpenMp.IComponentInterface";
    
    public const string EventHandlerNativeHandleStorageFQN = "SashManaged.OpenMp.EventHandlerNativeHandleStorage";

    // SashManaged
    public const string ApiAttributeFQN = "SashManaged.OpenMpApiAttribute";

    public const string HybridStringGeneratorAttributeFQN = "SashManaged.OpenMpHybridStringGeneratorAttribute";
    
    public const string EventHandlerAttributeFQN = "SashManaged.OpenMpEventHandlerAttribute";

    public const string OverloadAttributeFQN = "SashManaged.OpenMpApiOverloadAttribute";

    public const string FunctionAttributeFQN = "SashManaged.OpenMpApiFunctionAttribute";

    public const string PointerFQN = "SashManaged.IPointer";
    
    public const string StringViewMarshallerFQN = "SashManaged.StringViewMarshaller";

    public static readonly string DelegateFQN = typeof(Delegate).FullName!;

    public static readonly string MarshalFQN = typeof(Marshal).FullName!;
}