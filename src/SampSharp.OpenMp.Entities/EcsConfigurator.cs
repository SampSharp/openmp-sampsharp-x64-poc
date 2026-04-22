using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampSharp.Entities.Logging;
using SampSharp.Entities.SAMP;
using SampSharp.Entities.SAMP.Commands;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

[Extension(0x57e43771d28c5e7e)]
internal class EcsConfigurator(EcsConfiguration configuration) : Extension
{
    private IServiceProvider? _serviceProvider;

    public void Bind(IStartupContext context)
    {
        context.UseSynchronizationContext();

        context.Initialized += OnContextInitialized;
        context.Cleanup += OnContextCleanup;

        context.Core.AddExtension(this);
    }

    private void Run(IStartupContext context)
    {
        var configurator = (IEcsStartup)context.Configurator;

        var environment =
            new SampSharpEnvironment(configurator.GetType().Assembly, context.Core, context.ComponentList);

        // Build the service provider
        BuildServiceProvider(environment, configurator);

        // Run launch configurations
        context.UnhandledExceptionHandler = UnhandledExceptionHandler;

        configurator.Configure(new EcsBuilder(_serviceProvider));

        LoadSystems();

        // Fire initialize event
        OnGameModeInit();
    }

    [MemberNotNull(nameof(_serviceProvider))]
    private void BuildServiceProvider(SampSharpEnvironment environment, IEcsStartup configurator)
    {
        var services = new ServiceCollection();
        services.AddSingleton(environment);
        services.AddLogging(builder =>
        {
            builder.AddOpenMp();
            configuration.LoggingBuilder?.Invoke(builder);
        });

        ConfigureDefaultServices(services);
        configurator.ConfigureServices(services);

        var factory = configuration.ServiceProviderFactory ?? (s => s.BuildServiceProvider());
        _serviceProvider = factory(services);
    }

    private void UnhandledExceptionHandler(string context, Exception exception)
    {
        if (configuration.UnhandledExceptionHandler != null)
        {
            configuration.UnhandledExceptionHandler(_serviceProvider!, context, exception);
        }
        else
        {
            _serviceProvider!.GetRequiredService<ILoggerFactory>().CreateLogger(context)
                .LogError(exception, "Unhandled exception during: {context}", context);
        }
    }

    protected override void Cleanup()
    {
        OnGameModeExit();

        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void OnGameModeInit()
    {
        _serviceProvider?.GetRequiredService<IEventDispatcher>().Invoke("OnGameModeInit");
    }

    private void OnGameModeExit()
    {
        _serviceProvider?.GetRequiredService<IEventDispatcher>().Invoke("OnGameModeExit");
    }

    private void LoadSystems()
    {
        _serviceProvider!.GetRequiredService<SystemRegistry>().LoadSystems();
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
            .AddPlayerCommands()
            // TODO: .AddSingleton<IRconCommandService, RconCommandService>()
            .AddSingleton<ITimerService>(s => s.GetRequiredService<TimerSystem>())
            .AddSystem<TimerSystem>()
            .AddSystem<TickingSystem>()
            .AddSamp()
            ;
    }

    private void OnContextInitialized(object? sender, EventArgs e)
    {
        var context = (IStartupContext)sender!;
        Run(context);
    }

    private void OnContextCleanup(object? sender, EventArgs e)
    {
        Dispose();
    }
}