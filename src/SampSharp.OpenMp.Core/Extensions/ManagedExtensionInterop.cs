using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

internal static class ManagedExtensionInterop
{
    [DllImport("SampSharp", EntryPoint = "ManagedExtensionImpl_create", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint CreateInternal(UID id, nint freePointer, nint handle);

    [DllImport("SampSharp", EntryPoint = "ManagedExtensionImpl_delete", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Delete(nint handle);
    
    [DllImport("SampSharp", EntryPoint = "ManagedExtensionImpl_getHandle", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetHandle(nint handle);
}