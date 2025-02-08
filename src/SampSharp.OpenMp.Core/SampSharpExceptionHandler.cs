using System.Diagnostics;

namespace SampSharp.OpenMp.Core;

public static class SampSharpExceptionHandler
{
    private static ExceptionHandler _exceptionHandler = DefaultExceptionHandler;

    private static void DefaultExceptionHandler(string context, Exception exception)
    {
        Console.WriteLine($"Uncaught exception during {context}:");
        Console.WriteLine(exception.ToString());
    }

    internal static void SetExceptionHandler(ExceptionHandler handler)
    {
        _exceptionHandler = handler;
    }

    public static void HandleException(string context, Exception exception)
    {
        try
        {
            _exceptionHandler(context, exception);
        }
        catch(Exception ex)
        {
            try
            {
                if (Console.IsOutputRedirected)
                {
                    using var sw = new StreamWriter(Console.OpenStandardOutput());
                    sw.WriteLine($"An exception occurred while handling an exception ({context}):");
                    sw.WriteLine(ex.ToString());
                }
                else
                {
                    Console.WriteLine($"An exception occurred while handling an exception ({context}):");
                    Console.WriteLine(ex.ToString());
                }
            }
            catch
            {
                // void
            }
        }
        
        if (Debugger.IsAttached)
        {
            Debugger.BreakForUserUnhandledException(exception);
        }
    }
}