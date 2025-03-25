using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SampSharp.Entities;

/// <summary>
/// Provides configuration for the SampSharp.Entities framework.
/// </summary>
public sealed class EcsConfiguration
{
    internal Action<ILoggingBuilder>? LoggingBuilder { get; private set; }
    internal UnhandledExceptionHandler? UnhandledExceptionHandler { get; private set; }
    internal Func<IServiceCollection, IServiceProvider>? ServiceProviderFactory { get; private set; }

    private EcsConfiguration()
    {
    }

    internal static EcsConfiguration Create(Action<EcsConfiguration>? configure)
    {
        var result = new EcsConfiguration();

        configure?.Invoke(result);

        return result;
    }

    /// <summary>
    /// Configures the logging used by the application.
    /// </summary>
    /// <param name="builder">A delegate that configures the logging builder.</param>
    /// <returns>The updated configuration.</returns>
    public EcsConfiguration ConfigureLogging(Action<ILoggingBuilder> builder)
    {
        LoggingBuilder = builder;
        return this;
    }

    /// <summary>
    /// Configures the unhandled exception handler used by the application.
    /// </summary>
    /// <param name="handler">The handler for unhandled exceptions during the execution of the application.</param>
    /// <returns>The updated configuration.</returns>
    public EcsConfiguration ConfigureUnhandledExceptionhandler(UnhandledExceptionHandler handler)
    {
        UnhandledExceptionHandler = handler;
        return this;
    }

    /// <summary>
    /// Configures the service provider factory used by the application.
    /// </summary>
    /// <typeparam name="TContainerBuilder">The type of the container builder.</typeparam>
    /// <param name="serviceProviderFactory">The factory to use to create the service provider.</param>
    /// <returns>The updated configuration.</returns>
    public EcsConfiguration UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        where TContainerBuilder : notnull
    {
        ServiceProviderFactory = services => serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(services));
        return this;
    }
}