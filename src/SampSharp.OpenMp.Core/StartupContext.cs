using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public class StartupContext
{
    private ExceptionHandler _unhandledExceptionHandler;

    public StartupContext(SampSharpInitParams init)
    {
        Core = init.Core;
        ComponentList = init.ComponentList;
        Info = init.Info;
        _unhandledExceptionHandler = (context, ex) =>
        {
            Core.LogLine(LogLevel.Error, $"Unhandled exception during {context}:");
            Core.LogLine(LogLevel.Error, ex.ToString());
        };
        SampSharpExceptionHandler.SetExceptionHandler(_unhandledExceptionHandler);
    }

    public ICore Core { get; }
    public IComponentList ComponentList { get; }
    public SampSharpInfo Info { get; }

    public ExceptionHandler UnhandledExceptionHandler
    {
        get => _unhandledExceptionHandler;
        set
        {
            _unhandledExceptionHandler = value;
            SampSharpExceptionHandler.SetExceptionHandler(value);
        }
    }

    public Action? Cleanup { get; set; }

    public static void MainInfoProvider()
    {
        LaunchInstructions.Write();
    }
}

public static class LoggingStartupContextExtensions
{
    public static StartupContext ForwardConsoleOutputToOpenMpLogger(this StartupContext context)
    { 
        Console.SetOut(new LoggerTextWriter((ILogger)context.Core, LogLevel.Message));
        Console.SetError(new LoggerTextWriter((ILogger)context.Core, LogLevel.Error));

        return context;
    }

}
public class LoggerTextWriter(ILogger logger, LogLevel logLevel) : TextWriter
{
    private readonly StringBuilder _buffer = new();

    public override Encoding Encoding => Encoding.UTF8;
    
    public override void Write(char value)
    {
        if (value == '\n')
        {
            WriteBuffer();
        }
        else
        {
            _buffer.Append(value);
        }
    }

    public override void Write(string? value)
    {
        if (value == null)
        {
            return;
        }

        if (value.Contains('\n'))
        {
            foreach (var ch in value)
            {
                Write(ch);
            }
        }
        else
        {
            _buffer.Append(value);
        }
    }

    public override void WriteLine(string? value)
    {
        if (value == null)
        {
            WriteBuffer();
            return;
        }

        if (value.Contains('\n'))
        {
            foreach (var line in value.Split('\n'))
            {
                WriteLine(line.Trim('\r'));
            }
            return;
        }

        if (_buffer.Length > 0)
        {
            _buffer.Append(value);
            WriteBuffer();
        }
        else
        {
            WriteLineToLogger(value);
        }
    }

    public override void WriteLine()
    {
        WriteBuffer();
    }

    public override void Flush()
    {
        if (_buffer.Length > 0)
        {
            WriteBuffer();
        }
    }

    private void WriteBuffer()
    {
        WriteLineToLogger(_buffer.ToString());
        _buffer.Clear();
    }

    private void WriteLineToLogger(string? line)
    {
        logger.LogLine(logLevel, line ?? string.Empty);
    }
}
