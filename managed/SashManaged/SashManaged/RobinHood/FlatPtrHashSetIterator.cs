using System.Runtime.InteropServices;

namespace SashManaged.RobinHood;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct FlatPtrHashSetIterator
{
    public readonly nint _keyVals;
    private readonly nint _info;

    public bool Equals(FlatPtrHashSetIterator other)
    {
        return _keyVals == other._keyVals;
    }

    public override bool Equals(object? obj)
    {
        return obj is FlatPtrHashSetIterator other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _keyVals.GetHashCode();
    }
    
    public static bool operator ==(FlatPtrHashSetIterator a, FlatPtrHashSetIterator b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(FlatPtrHashSetIterator a, FlatPtrHashSetIterator b)
    {
        return !a.Equals(b);
    }
}