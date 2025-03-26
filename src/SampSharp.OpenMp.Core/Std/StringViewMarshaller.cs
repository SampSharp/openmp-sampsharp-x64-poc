using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace SampSharp.OpenMp.Core;

/// <summary>
/// Represents a marshaller entrypoint for marshalling <see langword="string"/> to a native <see cref="StringView"/> structure.
/// </summary>
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToNative))]
[CustomMarshaller(typeof(string), MarshalMode.UnmanagedToManagedOut, typeof(ManagedToNative))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(NativeToManaged))]
[CustomMarshaller(typeof(string), MarshalMode.UnmanagedToManagedIn, typeof(NativeToManaged))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedRef, typeof(Bidirectional))]
[CustomMarshaller(typeof(string), MarshalMode.UnmanagedToManagedRef, typeof(Bidirectional))]
public static unsafe class StringViewMarshaller
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ref struct ManagedToNative
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

            if (_byteCount < buffer.Length)
            {
                Encoding.UTF8.GetBytes(managed, buffer[.._byteCount]);
                buffer[_byteCount] = 0;
                _heapBuffer = null;
            }
            else
            {
                // buffer on stack too small, allocate on heap
                _heapBuffer = (byte*)Marshal.AllocHGlobal(_byteCount + 1);

                var heapBuffer = new Span<byte>(_heapBuffer, _byteCount + 1)
                {
                    [_byteCount] = 0
                };
                Encoding.UTF8.GetBytes(managed, heapBuffer);
            }
            
        }

        public readonly StringView ToUnmanaged()
        {
            return _heapBuffer == null 
                ? new StringView((byte*)Unsafe.AsPointer(ref _buffer.GetPinnableReference()), new Size(_byteCount)) 
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
    
    public static class NativeToManaged
    {
        public static string? ConvertToManaged(StringView unmanaged)
        {
            return unmanaged.ToString();
        }
    }

    public ref struct Bidirectional
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
            _heapBuffer = (byte*)Marshal.AllocHGlobal(_byteCount + 1);

            var heapBuffer = new Span<byte>(_heapBuffer, _byteCount + 1)
            {
                [_byteCount] = 0
            };
            Encoding.UTF8.GetBytes(managed, heapBuffer);
        }

        public readonly StringView ToUnmanaged()
        {
            return new StringView(_heapBuffer, new Size(_byteCount));
        }

        public void FromUnmanaged(StringView value)
        {
            _result = value.ToString();
        }

        public readonly string? ToManaged()
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}