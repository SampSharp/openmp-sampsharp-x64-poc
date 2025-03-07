namespace SampSharp.Entities;

public delegate void UnhandledExceptionHandler(IServiceProvider serviceProvider, string context, Exception exception);