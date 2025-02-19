using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public struct MarkedPoolIterator<T> where T : unmanaged
{
    private nint _pool;
    private int _lockedId;
    private FlatPtrHashSet<T> _entries;
    private FlatPtrHashSetIterator _iter;
}