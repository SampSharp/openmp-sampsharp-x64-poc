using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct BlittableStructRef<T> where T : struct
{
    private readonly nint _ptr;

    public BlittableStructRef(nint ptr)
    {
        _ptr = ptr;
    }

    public bool HasValue => _ptr != 0;

    public T Value
    {
        get
        {
            if (!HasValue)
            {
                throw new InvalidOperationException("Value is not set");
            }

            return Marshal.PtrToStructure<T>(_ptr);
        }
    }

    public T GetValueOrDefault()
    {
        return HasValue ? Value : default;
    }

    public T GetValueorDefault(T defaultValue)
    {
        return HasValue ? Value : defaultValue;
    }
}