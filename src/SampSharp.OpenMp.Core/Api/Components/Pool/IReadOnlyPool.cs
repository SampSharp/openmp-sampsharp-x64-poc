namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IReadOnlyPool<T> where T : unmanaged
{
    public T Get(int index)
    {
        var data =  IReadOnlyPoolInterop.IReadOnlyPool_get(_handle, index);
        return StructPointer.AsStruct<T>(data);
    }

    public void Bounds(out Pair<Size, Size> bounds)
    {
        IReadOnlyPoolInterop.IReadOnlyPool_bounds(_handle, out bounds);
    }
}