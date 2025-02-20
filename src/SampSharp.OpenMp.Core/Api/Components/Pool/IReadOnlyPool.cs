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
        return Pointer.AsStruct<T>(data);
    }

    public void Bounds(out Pair<Size, Size> bounds)
    {
        IReadOnlyPoolInterop.IReadOnlyPool_bounds(_handle, out bounds);
    }
}