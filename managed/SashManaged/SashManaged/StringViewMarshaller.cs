using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using SashManaged.OpenMp;

namespace SashManaged;

[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(StringViewMarshaller))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ManagedToUnmanagedOut))]
public static unsafe class StringViewMarshaller
{
    public static StringView ConvertToUnmanaged(string? managed)
    {
        if (managed == null)
        {
            return default;
        }
            
        var byteCount = Encoding.UTF8.GetByteCount(managed);
        var ptrBuffer = (byte*)Marshal.AllocHGlobal(byteCount);

        var span = new Span<byte>(ptrBuffer, byteCount);
        Encoding.UTF8.GetBytes(managed, span);

        return new StringView(ptrBuffer, byteCount);
    }

    public static string? ConvertToManaged(StringView unmanaged)
    {
        return unmanaged.ToString();
    }

    public static void Free(StringView unmanaged)
    {
        if (unmanaged._reference == null)
        {
            return;
        }
        Marshal.FreeHGlobal((nint)unmanaged._reference);
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

        public readonly void Free()
        {
        }
    }
    public ref struct ManagedToUnmanagedIn
    {
        public static int BufferSize => 128;

        private byte* _allocatedBuffer;
        private StringView _result;

        public void FromManaged(string? managed, Span<byte> buffer)
        {
            if (managed == null)
            {
                return;
            }

            var byteCount = Encoding.UTF8.GetByteCount(managed);

            Span<byte> spanToUse;
            if (byteCount <= buffer.Length)
            {
                spanToUse = buffer[..byteCount];
                _allocatedBuffer = null;
            }
            else
            {
                _allocatedBuffer = (byte*)Marshal.AllocHGlobal(byteCount);
                spanToUse = new Span<byte>(_allocatedBuffer, byteCount);
            }
            
            Encoding.UTF8.GetBytes(managed, spanToUse);

            _result = (ReadOnlySpan<byte>)spanToUse;
        }

        public readonly StringView ToUnmanaged()
        {
            return _result;
        }

        public void Free()
        {
            if (_allocatedBuffer != null)
            {
                Marshal.FreeHGlobal((nint)_allocatedBuffer);
                _allocatedBuffer = null;
            }
        }
    }
}