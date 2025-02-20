using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

internal static class PoolEventHandlerInterop
{
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "PoolEventHandlerImpl_create", ExactSpelling = true)]
    public static extern nint PoolEventHandlerImpl_create(nint onPoolEntryCreated, nint onPoolEntryDestroyed);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "PoolEventHandlerImpl_delete", ExactSpelling = true)]
    public static extern void PoolEventHandlerImpl_delete(nint ptr);
}