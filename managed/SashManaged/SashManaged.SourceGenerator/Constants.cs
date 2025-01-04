using System;
using System.Runtime.InteropServices;

namespace SashManaged.SourceGenerator;

public static class Constants
{
    // System

    /// <summary>
    /// System.Span&lt;byte&gt;
    /// </summary>
    public const string SpanOfBytesFQN = "System.Span<byte>";

    /// <summary>
    /// System.IEquatable
    /// </summary>
    public const string IEquatableFQN = "System.IEquatable";
    
    /// <summary>
    /// System.Delegate
    /// </summary>
    public static readonly string DelegateFQN = typeof(Delegate).FullName!;

    /// <summary>
    /// System.Runtime.InteropServices.Marshal
    /// </summary>
    public static readonly string MarshalFQN = typeof(Marshal).FullName!;

    /// <summary>
    /// System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute
    /// </summary>
    public const string MarshalUsingAttributeFQN = "System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute";

    /// <summary>
    /// System.Runtime.InteropServices.Marshalling.MarshalUsingAttribute
    /// </summary>
    public const string NativeMarshallingAttributeFQN = "System.Runtime.InteropServices.Marshalling.NativeMarshallingAttribute";

    /// <summary>
    /// System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute
    /// </summary>
    public const string CustomMarshallerAttributeFQN = "System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute";

    /// <summary>
    /// System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute.GenericPlaceholder
    /// </summary>
    public const string GenericPlaceholderFQN = "System.Runtime.InteropServices.Marshalling.CustomMarshallerAttribute.GenericPlaceholder";
    
    // SashManaged

    /// <summary>
    /// SashManaged.OpenMpApiAttribute
    /// </summary>
    public const string ApiAttributeFQN = "SashManaged.OpenMpApiAttribute";

    /// <summary>
    /// SashManaged.OpenMpEventHandlerAttribute
    /// </summary>
    public const string EventHandlerAttributeFQN = "SashManaged.OpenMpEventHandlerAttribute";

    /// <summary>
    /// SashManaged.OpenMpApiOverloadAttribute
    /// </summary>
    public const string OverloadAttributeFQN = "SashManaged.OpenMpApiOverloadAttribute";

    /// <summary>
    /// SashManaged.OpenMpApiFunctionAttribute
    /// </summary>
    public const string FunctionAttributeFQN = "SashManaged.OpenMpApiFunctionAttribute";

    /// <summary>
    /// SashManaged.NumberedTypeGeneratorAttribute
    /// </summary>
    public const string NumberedTypeGeneratorAttributeFQN = "SashManaged.NumberedTypeGeneratorAttribute";

    /// <summary>
    /// SashManaged.IPointer
    /// </summary>
    public const string PointerFQN = "SashManaged.IPointer";

    // SashManaged.OpenMp

    /// <summary>
    /// SashManaged.OpenMp.IEventHandler{TEventHandler}
    /// </summary>
    public const string EventHandlerFQN = "SashManaged.OpenMp.IEventHandler";
    
    /// <summary>
    /// SashManaged.OpenMp.NativeEventHandlerManager{TEventHandler}
    /// </summary>
    public const string NativeEventHandlerManagerFQN = "SashManaged.OpenMp.NativeEventHandlerManager";

    /// <summary>
    /// SashManaged.OpenMp.INativeEventHandlerManager{TEventHandler}
    /// </summary>
    public const string INativeEventHandlerManagerFQN = "SashManaged.OpenMp.INativeEventHandlerManager";

    /// <summary>
    /// SashManaged.OpenMp.IExtension
    /// </summary>
    public const string ExtensionFQN = "SashManaged.OpenMp.IExtension";

    /// <summary>
    /// SashManaged.OpenMp.IExtensionInterface{T}
    /// </summary>
    public const string ExtensionInterfaceFQN = "SashManaged.OpenMp.IExtensionInterface";

    /// <summary>
    /// SashManaged.OpenMp.IComponent
    /// </summary>
    public const string ComponentFQN = "SashManaged.OpenMp.IComponent";

    /// <summary>
    /// SashManaged.OpenMp.IComponentInterface{T}
    /// </summary>
    public const string ComponentInterfaceFQN = "SashManaged.OpenMp.IComponentInterface";

    /// <summary>
    /// SashManaged.OpenMp.EventHandlerNativeHandleStorage
    /// </summary>
    public const string EventHandlerNativeHandleStorageFQN = "SashManaged.OpenMp.EventHandlerNativeHandleStorage";
    
    /// <summary>
    /// SashManaged.OpenMp.StringViewMarshaller
    /// </summary>
    public const string StringViewMarshallerFQN = "SashManaged.OpenMp.StringViewMarshaller";

}