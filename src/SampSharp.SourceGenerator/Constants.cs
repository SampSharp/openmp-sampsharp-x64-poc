using System;
using System.Runtime.InteropServices;

namespace SampSharp.SourceGenerator;

public static class Constants
{
    // System

    /// <summary>
    /// System.Span&lt;byte&gt;
    /// </summary>
    public const string SpanOfBytesFQN = "System.Span<byte>";

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
    
    // SampSharp.OpenMp.Core

    /// <summary>
    /// SampSharp.OpenMp.Core.OpenMpApiAttribute
    /// </summary>
    public const string ApiAttributeFQN = "SampSharp.OpenMp.Core.OpenMpApiAttribute";

    /// <summary>
    /// SampSharp.OpenMp.Core.OpenMpEventHandlerAttribute
    /// </summary>
    public const string EventHandlerAttributeFQN = "SampSharp.OpenMp.Core.OpenMpEventHandlerAttribute";

    /// <summary>
    /// SampSharp.OpenMp.Core.OpenMpApiOverloadAttribute
    /// </summary>
    public const string OverloadAttributeFQN = "SampSharp.OpenMp.Core.OpenMpApiOverloadAttribute";

    /// <summary>
    /// SampSharp.OpenMp.Core.OpenMpApiFunctionAttribute
    /// </summary>
    public const string FunctionAttributeFQN = "SampSharp.OpenMp.Core.OpenMpApiFunctionAttribute";

    /// <summary>
    /// SampSharp.OpenMp.Core.NumberedTypeGeneratorAttribute
    /// </summary>
    public const string NumberedTypeGeneratorAttributeFQN = "SampSharp.OpenMp.Core.NumberedTypeGeneratorAttribute";

    // SampSharp.OpenMp.Core.Api

    /// <summary>
    /// SampSharp.OpenMp.Core.Api.IEventHandler{TEventHandler}
    /// </summary>
    public const string EventHandlerFQN = "SampSharp.OpenMp.Core.Api.IEventHandler";
    
    /// <summary>
    /// SampSharp.OpenMp.Core.Api.NativeEventHandlerManager{TEventHandler}
    /// </summary>
    public const string NativeEventHandlerManagerFQN = "SampSharp.OpenMp.Core.Api.NativeEventHandlerManager";

    /// <summary>
    /// SampSharp.OpenMp.Core.Api.INativeEventHandlerManager{TEventHandler}
    /// </summary>
    public const string INativeEventHandlerManagerFQN = "SampSharp.OpenMp.Core.Api.INativeEventHandlerManager";

    /// <summary>
    /// SampSharp.OpenMp.Core.Api.IExtension
    /// </summary>
    public const string ExtensionFQN = "SampSharp.OpenMp.Core.Api.IExtension";

    /// <summary>
    /// SampSharp.OpenMp.Core.Api.IExtensionInterface{T}
    /// </summary>
    public const string ExtensionInterfaceFQN = "SampSharp.OpenMp.Core.Api.IExtensionInterface";

    /// <summary>
    /// SampSharp.OpenMp.Core.Api.IComponent
    /// </summary>
    public const string ComponentFQN = "SampSharp.OpenMp.Core.Api.IComponent";

    /// <summary>
    /// SampSharp.OpenMp.Core.Api.IComponentInterface{T}
    /// </summary>
    public const string ComponentInterfaceFQN = "SampSharp.OpenMp.Core.Api.IComponentInterface";

    /// <summary>
    /// SampSharp.OpenMp.Core.Api.EventHandlerNativeHandleStorage
    /// </summary>
    public const string EventHandlerNativeHandleStorageFQN = "SampSharp.OpenMp.Core.Api.EventHandlerNativeHandleStorage";
    
    /// <summary>
    /// SampSharp.OpenMp.Core.Api.StringViewMarshaller
    /// </summary>
    public const string StringViewMarshallerFQN = "SampSharp.OpenMp.Core.Api.StringViewMarshaller";


    public const string StartupContextFQN = "SampSharp.OpenMp.Core.StartupContext";
    public const string InitParamsFQN = "SampSharp.OpenMp.Core.SampSharpInitParams";
}