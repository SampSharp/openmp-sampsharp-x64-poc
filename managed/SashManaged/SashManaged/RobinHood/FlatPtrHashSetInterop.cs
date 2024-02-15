using System.Runtime.InteropServices;
using SashManaged.OpenMp;

namespace SashManaged;

internal static class FlatPtrHashSetInterop
{
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatPtrHashSet_begin(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatPtrHashSet_end(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatPtrHashSet_inc(FlatPtrHashSetIterator value);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern Size FlatPtrHashSet_size(nint data);

}