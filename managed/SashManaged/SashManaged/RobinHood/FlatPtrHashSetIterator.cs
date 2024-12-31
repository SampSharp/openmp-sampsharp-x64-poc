using System.Runtime.InteropServices;

namespace SashManaged;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct FlatPtrHashSetIterator
{
    private readonly nint _value; // NodePtr mKeyVals{nullptr};
    private readonly nint _info; // uint8_t const* mInfo{nullptr};

    public nint Value => _value;

    public bool Equals(FlatPtrHashSetIterator other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is FlatPtrHashSetIterator other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
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