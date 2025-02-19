using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

public readonly struct IReadOnlyPool<T> where T : unmanaged
{
    private readonly nint _handle;

    public IReadOnlyPool(nint handle)
    {
        _handle = handle;
    }

    public nint Handle => _handle;

    public override int GetHashCode()
    {
        return _handle.GetHashCode();
    }

    public T Get(int index)
    {
        var data =  IReadOnlyPoolInterop.IReadOnlyPool_get(_handle, index);
        Union un = default;
        un.handle = data;
        return un.ptr!;
    }

    public void Bounds(out Pair<Size, Size> bounds)
    {
        IReadOnlyPoolInterop.IReadOnlyPool_bounds(_handle, out bounds);
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct Union
    {
        [FieldOffset(0)]
        public T ptr;
        
        [FieldOffset(0)]
        public nint handle;
    }
}