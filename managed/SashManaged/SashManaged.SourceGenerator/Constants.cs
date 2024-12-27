using System;
using System.Runtime.InteropServices;

namespace SashManaged.SourceGenerator;

public static class Constants
{
    // old
    public const string SequentialStructLayoutAttribute = "[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]";

    // System
    public const string SpanOfBytesFQN = "System.Span<byte>";

    public const string IEquatableFQN = "System.IEquatable";
    
    public const string MarshalUsingAttributeFQN = "System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute";

    public const string NativeMarshallingAttributeFQN = "System.Runtime.InteropServices.Marshalling.NativeMarshallingAttribute";

    public const string CustomMarshallerAttributeFQN = "System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute";

    // SashManaged.OpenMp
    public const string StringViewFQN = "SashManaged.OpenMp.StringView";

    public const string SizeFQN = "SashManaged.OpenMp.Size";
    
    public const string EventHandler2FQN = "SashManaged.OpenMp.IEventHandler2";
    
    public const string ExtensionFQN = "SashManaged.OpenMp.IExtension";

    public const string ExtensionInterfaceFQN = "SashManaged.OpenMp.IExtensionInterface";
    
    public const string ComponentFQN = "SashManaged.OpenMp.IComponent";

    public const string ComponentInterfaceFQN = "SashManaged.OpenMp.IComponentInterface";
    
    public const string EventHandlerNativeHandleStorageFQN = "SashManaged.OpenMp.EventHandlerNativeHandleStorage";

    // SashManaged
    public const string MarshallAttributeFQN = "SashManaged.OpenMpApiMarshallAttribute";
    
    public const string ApiAttributeFQN = "SashManaged.OpenMpApiAttribute";

    public const string ApiAttribute2FQN = "SashManaged.OpenMpApi2Attribute";

    public const string HybridStringGeneratorAttributeFQN = "SashManaged.OpenMpHybridStringGeneratorAttribute";
    
    public const string EventHandlerAttributeFQN = "SashManaged.OpenMpEventHandlerAttribute";

    public const string EventHandlerAttribute2FQN = "SashManaged.OpenMpEventHandler2Attribute";

    public const string OverloadAttributeFQN = "SashManaged.OpenMpApiOverloadAttribute";

    public const string FunctionAttributeFQN = "SashManaged.OpenMpApiFunctionAttribute";

    public const string BlittableBooleanFQN = "SashManaged.BlittableBoolean";

    public const string BlittableRefFQN = "SashManaged.BlittableRef";

    public const string PointerFQN = "SashManaged.IPointer";
    
    public const string StringViewMarshallerFQN = "SashManaged.StringViewMarshaller";

    public const string BooleanMarshallerFQN = "SashManaged.BooleanMarshaller";
    
    public static readonly string DelegateFQN = typeof(Delegate).FullName!;

    public static readonly string MarshalFQN = typeof(Marshal).FullName!;
}