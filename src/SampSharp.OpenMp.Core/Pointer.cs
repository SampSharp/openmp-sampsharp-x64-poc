using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SampSharp.OpenMp.Core;

internal static unsafe class Pointer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToStruct<T>(nint pointer) where T : unmanaged
    {
        Debug.Assert(sizeof(T) == sizeof(nint));
        return *(T*)pointer;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo TypeCast<TFrom, TTo>(TFrom from) where TFrom : unmanaged where TTo : unmanaged
    {
        Debug.Assert(sizeof(TFrom) == sizeof(TTo));
        return *(TTo*)&from;
    }
}
