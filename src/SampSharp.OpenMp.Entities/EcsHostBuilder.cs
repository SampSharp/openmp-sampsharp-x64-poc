using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SampSharp.Entities;

internal sealed class EcsHostBuilder : IEcsHostBuilder
{
    private readonly List<Action<SampSharpEnvironment, IServiceCollection>> _serviceConfigurations = [];
    private readonly List<Action<IEcsApplicationBuilder>> _ecsConfigurations = [];
    private readonly List<Action<ILoggingBuilder>> _loggerConfigurations = [];

    private bool _systemsLoadingDisabled = false;

    internal UnhandledExceptionHandler? UnhandledExceptionHandler { get; private set; }
    internal Func<IServiceCollection, IServiceProvider>? ServiceProviderFactory { get; private set; }
    
    public IEcsHostBuilder Configure(Action<IEcsApplicationBuilder> build)
    {
        ArgumentNullException.ThrowIfNull(build);
        _ecsConfigurations.Add(build);
        return this;
    }

    public IEcsHostBuilder ConfigureServices(Action<SampSharpEnvironment, IServiceCollection> build)
    {
        ArgumentNullException.ThrowIfNull(build);
        _serviceConfigurations.Add(build);
        return this;
    }

    public IEcsHostBuilder ConfigureServices(Action<IServiceCollection> build)
    {
        ArgumentNullException.ThrowIfNull(build);
        return ConfigureServices((_, services) => build(services));
    }

    public IEcsHostBuilder ConfigureLogging(Action<ILoggingBuilder> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        _loggerConfigurations.Add(builder);
        return this;
    }

    public IEcsHostBuilder ConfigureUnhandledExceptionhandler(UnhandledExceptionHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        UnhandledExceptionHandler = handler;
        return this;
    }

    public IEcsHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        where TContainerBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(serviceProviderFactory);
        ServiceProviderFactory = services => serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(services));
        return this;
    }

    public IEcsHostBuilder DisableDefaultSystemsLoading()
    {
        _systemsLoadingDisabled = true;
        return this;
    }

    internal void Configure(IEcsApplicationBuilder builder)
    {
        foreach (var configuration in _ecsConfigurations)
        {
            configuration(builder);
        }

        _ecsConfigurations.Clear();
    }

    internal void ConfigureLogger(ILoggingBuilder builder)
    {
        foreach (var configuration in _loggerConfigurations)
        {
            configuration(builder);
        }

        _loggerConfigurations.Clear();
    }

    internal void ConfigureServices(SampSharpEnvironment environment, IServiceCollection services)
    {
        foreach (var configuration in _serviceConfigurations)
        {
            configuration(environment, services);
        }

        if (!_systemsLoadingDisabled)
        {
            services.AddSystemsInAssembly(environment.EntryAssembly);
            _systemsLoadingDisabled = true;
        }

        _serviceConfigurations.Clear();
    }
}