using System.Runtime.InteropServices;

namespace SashManaged;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct BlittableRef<T> where T : unmanaged
{
    private readonly T* _data;
    public ref T Value => ref *_data;

    private BlittableRef(T* data)
    {
        _data = data;
    }
    
    public static implicit operator BlittableRef<T>(nint pointer)
    {
        return new BlittableRef<T>((T*)pointer);
    }

    public static implicit operator nint(BlittableRef<T> blittableRef)
    {
        return (nint)blittableRef._data;
    }
}