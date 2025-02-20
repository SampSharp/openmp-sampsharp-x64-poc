using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SampSharp.OpenMp.Core;

internal static unsafe class Pointer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T AsStruct<T>(nint pointer) where T : unmanaged
    {
        Debug.Assert(sizeof(T) == sizeof(nint));
        return *(T*)&pointer;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Dereference<T>(nint pointer) where T : unmanaged
    {
        return *(T*)pointer;
    }
}
