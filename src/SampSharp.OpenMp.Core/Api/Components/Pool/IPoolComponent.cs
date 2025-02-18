using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IComponent))]
[OpenMpApiPartial]
public readonly partial struct IPoolComponent<T> where T : unmanaged
{
}

[OpenMpApi(typeof(IReadOnlyPool<>))]
public readonly partial struct IPool<T> where T : unmanaged
{
    public partial void Release(int index);
    public partial void Lock(int index);
    public partial bool Unlock(int index);
    public partial IEventDispatcher<IPoolEventHandler<T>> GetPoolEventDispatcher();
    public partial MarkedPoolIterator<T> Begin();
    public partial MarkedPoolIterator<T> End();
    public partial Size Count();
}

[StructLayout(LayoutKind.Sequential)]
public struct MarkedPoolIterator<T> where T : unmanaged
{
    private nint _pool;
    private int _lockedId;
    private FlatPtrHashSet<T> _entries;
    private FlatPtrHashSetIterator _iter;
}

[OpenMpApi]
public readonly partial struct IReadOnlyPool<T> where T : unmanaged
{
    public partial T Get(int index);

    public partial void Bounds(out Pair<Size, Size> bounds);
}