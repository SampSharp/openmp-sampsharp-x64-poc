namespace SashManaged;

public readonly struct BlittableBoolean(bool value) : IEquatable<BlittableBoolean>
{
    private readonly byte _data = value ? (byte)1 : (byte)0;

    private bool AsBool => _data != 0;

    public bool Equals(BlittableBoolean other)
    {
        return AsBool == other.AsBool;
    }

    public override bool Equals(object? obj)
    {
        return obj is BlittableBoolean other && Equals(other);
    }

    public override int GetHashCode()
    {
        return AsBool.GetHashCode();
    }
    
    public static bool operator ==(BlittableBoolean lhs, BlittableBoolean rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(BlittableBoolean lhs, BlittableBoolean rhs)
    {
        return !lhs.Equals(rhs);
    }

    public static implicit operator bool(BlittableBoolean b)
    {
        return b.AsBool;
    }

    public static implicit operator BlittableBoolean(bool b)
    {
        return new BlittableBoolean(b);
    }
}