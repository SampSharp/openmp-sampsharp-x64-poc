using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SampSharp.OpenMp.Core;

/// <summary>
/// Provides methods to work with structs which represent pointers to open.mp interfaces.
/// </summary>
internal static unsafe class StructPointer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T AsStruct<T>(nint pointer) where T : unmanaged
    {
        Debug.Assert(sizeof(T) == sizeof(nint));
        return *(T*)&pointer;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Dereference<T>(nint pointerToPointer) where T : unmanaged
    {
        return *(T*)pointerToPointer;
    }
}
