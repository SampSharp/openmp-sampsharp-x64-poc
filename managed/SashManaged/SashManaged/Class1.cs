using System.Runtime.InteropServices;

namespace SashManaged;

public class Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LibArgs
    {
        public IntPtr Message;
        public int Number;
    }

    [UnmanagedCallersOnly]
    public static void EntryPoint(LibArgs libArgs)
    {
        Console.WriteLine($"Hello, world! from {nameof(EntryPoint)} in managed code");
        PrintLibArgs(libArgs);
    }

    private static void PrintLibArgs(LibArgs libArgs)
    {
        var message = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? Marshal.PtrToStringUni(libArgs.Message)
            : Marshal.PtrToStringUTF8(libArgs.Message);

        Console.WriteLine($"-- message: {message}");
        Console.WriteLine($"-- number: {libArgs.Number}");
    }

    public static void Main(){/*nop*/}
}
