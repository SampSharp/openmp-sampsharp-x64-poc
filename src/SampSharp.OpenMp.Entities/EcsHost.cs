using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampSharp.Entities.Logging;
using SampSharp.Entities.SAMP;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

[Extension(0x57e43771d28c5e7e)]
internal class EcsHost(EcsHostBuilder hostBuilder) : Extension
{
    private IServiceProvider? _serviceProvider;

    private IServiceProvider ServiceProvider => _serviceProvider ?? throw new InvalidOperationException();

    public void Bind(IStartupContext context)
    {
        context.UseSynchronizationContext();

        context.Initialized += OnContextInitialized;
        context.Cleanup += OnContextCleanup;

        context.Core.AddExtension(this);
    }

    protected override void Cleanup()
    {
        OnGameModeExit();

        if (_serviceProvider is IDisposable disposable)
        {
            //  TODO: This cleanup is called so late - we can't unsubscribe event handlers anymore, but the disposables in registered systems will try to unsubscribe them. This may cause a System.ExecutionEngineException on shutdown.
            disposable.Dispose();
            _serviceProvider = null;
        }
    }

    private void Start(IStartupContext context)
    {
        var environment = new SampSharpEnvironment(context.Configurator.GetType().Assembly, context.Core, context.ComponentList);

        BuildServiceProvider(environment);

        context.UnhandledExceptionHandler = UnhandledExceptionHandler;
        hostBuilder.Configure(new EcsBuilder(ServiceProvider));

        LoadSystems();

        // Fire initial event
        OnGameModeInit();
    }

    private void BuildServiceProvider(SampSharpEnvironment environment)
    {
        var services = new ServiceCollection();
        services.AddSingleton(environment);
        services.AddLogging(builder =>
        {
            builder.AddOpenMp();
            hostBuilder.ConfigureLogger(builder);
        });

        ConfigureDefaultServices(services);

        hostBuilder.ConfigureServices(environment, services);

        var factory = hostBuilder.ServiceProviderFactory ?? DefaultServiceProviderFactory;
        _serviceProvider = factory(services);
    }

    private void UnhandledExceptionHandler(string context, Exception exception)
    {
        if (hostBuilder.UnhandledExceptionHandler != null)
        {
            hostBuilder.UnhandledExceptionHandler(ServiceProvider, context, exception);
        }
        else
        {
            DefaultExceptionHandler(context, exception);
        }
    }

    private void DefaultExceptionHandler(string context, Exception exception)
    {
        ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(context)
            .LogError(exception, "Unhandled exception during: {context}", context);
    }

    private static IServiceProvider DefaultServiceProviderFactory(IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }

    private void OnGameModeInit()
    {
        ServiceProvider.GetRequiredService<IEventDispatcher>().Invoke("OnGameModeInit");
    }

    private void OnGameModeExit()
    {
        ServiceProvider.GetRequiredService<IEventDispatcher>().Invoke("OnGameModeExit");
    }

    private void LoadSystems()
    {
        ServiceProvider.GetRequiredService<SystemRegistry>().LoadSystems();
    }

    private static void ConfigureDefaultServices(IServiceCollection services)
    {
        services
            .AddSingleton<EventDispatcher>()
            .AddSingleton<IEventDispatcher>(sp => sp.GetRequiredService<EventDispatcher>())
#pragma warning disable CS0618 // Type or member is obsolete
            .AddSingleton<IEventService>(sp => sp.GetRequiredService<EventDispatcher>())
#pragma warning restore CS0618 // Type or member is obsolete
            .AddSingleton<SystemRegistry>()
            .AddSingleton<ISystemRegistry>(x => x.GetRequiredService<SystemRegistry>())
            .AddSingleton<IEntityManager, EntityManager>()
            .AddSingleton<ITimerService>(s => s.GetRequiredService<TimerSystem>())
            .AddSystem<TimerSystem>()
            .AddSystem<TickingSystem>()
            .AddSamp()
            ;
    }

    private void OnContextInitialized(object? sender, EventArgs e)
    {
        var context = (IStartupContext)sender!;
        Start(context);
    }

    private void OnContextCleanup(object? sender, EventArgs e)
    {
        Dispose();
    }
}