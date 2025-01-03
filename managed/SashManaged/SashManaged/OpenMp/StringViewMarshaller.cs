using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using SashManaged.OpenMp;
using System.Runtime.CompilerServices;

namespace SashManaged;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ManagedToUnmanagedOut))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedRef, typeof(ManagedToUnmanagedRef))]
public static unsafe class StringViewMarshaller
{
    public ref struct ManagedToUnmanagedIn
    {
        public static int BufferSize => 128;

        private byte* _heapBuffer;
        private int _byteCount;

        private Span<byte> _buffer;

        public void FromManaged(string? managed, Span<byte> buffer)
        {
            _buffer = buffer;

            if (managed == null)
            {
                return;
            }

            _byteCount = Encoding.UTF8.GetByteCount(managed);

            if (_byteCount <= buffer.Length)
            {
                Encoding.UTF8.GetBytes(managed, buffer[.._byteCount]);
                _heapBuffer = null;
            }
            else
            {
                // buffer on stack too small, allocate on heap
                _heapBuffer = (byte*)Marshal.AllocHGlobal(_byteCount);

                var heapBuffer = new Span<byte>(_heapBuffer, _byteCount);
                Encoding.UTF8.GetBytes(managed, heapBuffer);
            }
            
        }

        // public ref byte GetPinnableReference()
        // {
        //     // should not be required, but let's be safe
        //     return ref _buffer.GetPinnableReference();
        // }

        public readonly StringView ToUnmanaged()
        {
            return _heapBuffer == null 
                ? new StringView((byte*)Unsafe.AsPointer(ref _buffer.GetPinnableReference()), new Size(_buffer.Length)) 
                : new StringView(_heapBuffer, new Size(_byteCount));
        }

        public void Free()
        {
            if (_heapBuffer != null)
            {
                Marshal.FreeHGlobal((nint)_heapBuffer);
                _heapBuffer = null;
            }
        }
    }

    public ref struct ManagedToUnmanagedOut
    {
        private string? _result;

        public void FromUnmanaged(StringView unmanaged)
        {
            _result = unmanaged.ToString();
        }
        
        public readonly string ToManaged()
        {
            return _result!;
        }

        public void Free()
        {
        }
    }

    public ref struct ManagedToUnmanagedRef
    {
        private byte* _heapBuffer;
        private int _byteCount;
        private string? _result;

        public void FromManaged(string? managed)
        {
            if (managed == null)
            {
                return;
            }

            _byteCount = Encoding.UTF8.GetByteCount(managed);
            _heapBuffer = (byte*)Marshal.AllocHGlobal(_byteCount);

            var heapBuffer = new Span<byte>(_heapBuffer, _byteCount);
            Encoding.UTF8.GetBytes(managed, heapBuffer);
        }

        public StringView ToUnmanaged()
        {
            return new StringView(_heapBuffer, new Size(_byteCount));
        }

        public void FromUnmanaged(StringView value)
        {
            _result = value.ToString();
        }

        public string? ToManaged()
        {
            return _result;
        }

        public void Free()
        {
            if(_heapBuffer != null)
            {
                Marshal.FreeHGlobal((nint)_heapBuffer);
                _heapBuffer = null;
                _byteCount = 0;
            }
        }
    }
}