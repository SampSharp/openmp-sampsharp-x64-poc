using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Pair<T1, T2> 
    where T1 : unmanaged 
    where T2 : unmanaged
{
    public readonly T1 First;
    public readonly T2 Second;

    public void Deconstruct(out T1 first, out T2 second)
    {
        first = First;
        second = Second;
    }

    public override string ToString()
    {
        return $"({First}, {Second})";
    }

    public static implicit operator (T1, T2)(Pair<T1, T2> pair)
    {
        return (pair.First, pair.Second);
    }

    public static implicit operator Pair<T1, T2>((T1 first,T2 second) pair)
    {
        return (pair.first, pair.second);
    }
}
