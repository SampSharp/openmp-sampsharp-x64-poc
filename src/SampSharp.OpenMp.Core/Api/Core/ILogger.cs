using System.Text;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly unsafe partial struct ILogger
{
    public partial void PrintLn(byte* msg);
    public partial void LogLn(LogLevel level, byte* msg);
    public partial void PrintLnU8(byte* msg);
    public partial void LogLnU8(LogLevel level, byte* msg);
    
    public void LogLine(LogLevel level, string msg)
    {
        var arr = new byte[Encoding.UTF8.GetByteCount(msg) + 1];
        Encoding.UTF8.GetBytes(msg, arr);

        fixed (byte* msgPtr = arr)
        {
            LogLn(level, msgPtr);
        }
    }

    public void PrintLine(string msg)
    {
        var arr = new byte[Encoding.UTF8.GetByteCount(msg) + 1];
        Encoding.UTF8.GetBytes(msg, arr);

        fixed (byte* msgPtr = arr)
        {
            PrintLn(msgPtr);
        }
    }
}