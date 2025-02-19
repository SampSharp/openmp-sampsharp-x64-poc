using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

internal static class IPoolInterop
{
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IPool_release", ExactSpelling = true)]
    public static extern void IPool_release(nint handle, int index);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IPool_lock", ExactSpelling = true)]
    public static extern void IPool_lock(nint handle, int index);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IPool_unlock", ExactSpelling = true)]
    public static extern bool IPool_unlock(nint handle, int index);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IPool_getPoolEventDispatcher", ExactSpelling = true)]
    public static extern nint IPool_getPoolEventDispatcher(nint handle);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IPool_begin", ExactSpelling = true)]
    public static extern MarkedPoolIteratorData IPool_begin(nint handle);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IPool_end", ExactSpelling = true)]
    public static extern MarkedPoolIteratorData IPool_end(nint handle);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IPool_count", ExactSpelling = true)]
    public static extern Size IPool_count(nint handle);
}