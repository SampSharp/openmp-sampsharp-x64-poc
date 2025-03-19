namespace SampSharp.OpenMp.Core;

public readonly struct BlittableBoolean(bool value) : IEquatable<BlittableBoolean>
{
    private const byte True = 1;
    private const byte False = 0;

    private readonly byte _data = value ? True : False;

    public bool Equals(BlittableBoolean other)
    {
        return (bool)this == (bool)other;
    }

    public override bool Equals(object? obj)
    {
        return (obj is BlittableBoolean other && Equals(other)) || (obj is bool b && (bool)this == b);
    }

    public override int GetHashCode()
    {
        return ((bool)this).GetHashCode();
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
        return  b._data != 0;
    }

    public static implicit operator BlittableBoolean(bool b)
    {
        return new BlittableBoolean(b);
    }

    public override string ToString()
    {
        return  ((bool)this).ToString();
    }
}