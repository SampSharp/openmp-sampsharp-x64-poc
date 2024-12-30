using System.Runtime.InteropServices;

namespace SashManaged
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct BlittableStructRef<T> where T : struct
    {
        private readonly nint _ptr;

        public T GetValue()
        {
            return Marshal.PtrToStructure<T>(_ptr);
        }
    }
}