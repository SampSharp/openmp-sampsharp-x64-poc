using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct BlittableRef<T> where T : struct
    {
        private readonly nint _ptr;

        public BlittableRef(nint ptr)
        {
            _ptr = ptr;
        }

        public bool HasValue => _ptr != 0;

        public unsafe ref T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException("Value is not set");
                }
                return ref Unsafe.AsRef<T>((void*)_ptr);
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
}