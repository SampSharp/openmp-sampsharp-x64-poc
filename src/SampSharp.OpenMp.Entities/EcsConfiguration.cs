using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SampSharp.Entities;

public sealed class EcsConfiguration
{
    internal Action<ILoggingBuilder>? LoggingBuilder { get; private set; }
    internal UnhandledExceptionHandler? UnhandledExceptionHandler { get; private set; }
    internal Func<IServiceCollection, IServiceProvider>? ServiceProviderFactory { get; private set; }

    public EcsConfiguration ConfigureLogging(Action<ILoggingBuilder> builder)
    {
        LoggingBuilder = builder;
        return this;
    }

    public EcsConfiguration ConfigureUnhandledExceptionhandler(UnhandledExceptionHandler handler)
    {
        UnhandledExceptionHandler = handler;
        return this;
    }

    public EcsConfiguration UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        where TContainerBuilder : notnull
    {
        ServiceProviderFactory = services => serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(services));
        return this;
    }
}