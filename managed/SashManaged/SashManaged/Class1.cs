using System.Runtime.InteropServices;

namespace SashManaged;

public class Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SemanticVersion
    {
        public byte major;
        public byte minor;
        public byte patch;
        public ushort prerel;
    }

    public struct ICore
    {
        public IntPtr ptr;
    }

    enum SettableCoreDataType
    {
        ServerName,
        ModeText,
        MapName,
        Language,
        URL,
        Password,
        AdminPassword,
    };
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct StringView
    {
        private char * data_;
        private Size size_;

        public static StringView Create(char *data, Size length)
        {
            return new StringView
            {
                data_ = data,
                size_ = length
            };
        } 
    }

    public struct Size
    {
        public nint Value;

        public Size(nint value)
        {
            Value = value;
        }
    }

    [UnmanagedCallersOnly]
    public static unsafe void OnInit(ICore core)
    {
        Console.WriteLine("OnInit");

        var version = ICore_getVersion(core);

        Console.WriteLine($"{version.major}.{version.minor}.{version.patch}.{version.prerel}");


        var val = "from .NET code";
        var ptr = Marshal.StringToHGlobalAnsi(val);
        nint len = val.Length;

        ICore_setData(core, SettableCoreDataType.ServerName, StringView.Create((char*)ptr, new Size(len)));
        Marshal.FreeHGlobal(ptr);
    }
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern SemanticVersion ICore_getVersion(ICore core);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern void ICore_setData(ICore core, SettableCoreDataType type, StringView data);

    // need an entry point to build runtime config for this application
    public static void Main(){/*nop*/}
}
