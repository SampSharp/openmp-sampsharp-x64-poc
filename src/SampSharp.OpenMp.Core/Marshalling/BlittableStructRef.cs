using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct BlittableStructRef<T> where T : struct
    {
        private readonly nint _ptr;

        public BlittableStructRef(nint ptr)
        {
            _ptr = ptr;
        }

        public bool IsNull => _ptr == 0;
        public T GetValue()
        {
            if (IsNull)
            {
                return default;
            }

            return Marshal.PtrToStructure<T>(_ptr);
        }
    }
}