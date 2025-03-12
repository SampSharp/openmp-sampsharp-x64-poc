using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampSharp.Entities.Logging;
using SampSharp.Entities.SAMP;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

[Extension(0x57e43771d28c5e7e)]
internal class EcsManager : Extension
{
    private readonly EcsConfiguration _configuration;
    private IServiceProvider? _serviceProvider;

    public EcsManager(EcsConfiguration configuration)
    {
        _configuration = configuration;
    }

    internal void Run(StartupContext context)
    {
        var configurator = (IEcsStartup)context.Configurator;

        var info = new RuntimeInformation(configurator.GetType().Assembly);

        // Build the service provider
        BuildServiceProvider(context, info, configurator);

        // Run launch configurations
        context.UnhandledExceptionHandler = UnhandledExceptionHandler;

        configurator.Configure(new EcsBuilder(_serviceProvider));

        AddWrappedSystemTypes();

        // Fire initialize event
        OnGameModeInit();
    }

    [MemberNotNull(nameof(_serviceProvider))]
    private void BuildServiceProvider(StartupContext context, RuntimeInformation info, IEcsStartup configurator)
    {
        var services = new ServiceCollection();
        services.AddSingleton(info);
        services.AddSingleton(new OpenMp(context.Core, context.ComponentList));
        services.AddLogging(builder =>
        {
            builder.AddOpenMp();
            _configuration.LoggingBuilder?.Invoke(builder);
        });

        ConfigureDefaultServices(services);
        configurator.ConfigureServices(services);

        var factory = _configuration.ServiceProviderFactory ?? DefaultServiceFactory;
        _serviceProvider = factory(services);
    }

    private static IServiceProvider DefaultServiceFactory(IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }

    private void UnhandledExceptionHandler(string context, Exception exception)
    {
        if (_configuration.UnhandledExceptionHandler != null)
        {
            _configuration.UnhandledExceptionHandler(_serviceProvider!, context, exception);
        }
        else
        {
            _serviceProvider!.GetRequiredService<ILoggerFactory>().CreateLogger(context).LogError(exception, "Unhandled exception during: {context}", context);
        }
    }

    private void OnGameModeInit()
    {
        _serviceProvider?.GetRequiredService<IEventService>().Invoke("OnGameModeInit");
    }

    private void OnGameModeExit()
    {
        _serviceProvider?.GetRequiredService<IEventService>().Invoke("OnGameModeExit");
    }

    protected override void Cleanup()
    {
        OnGameModeExit();

        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void AddWrappedSystemTypes()
    {
        _serviceProvider!.GetRequiredService<SystemRegistry>().InitialSystemScan();
    }

    private static void ConfigureDefaultServices(IServiceCollection services)
    {
        services.AddSingleton<IEventService, EventService>()
            .AddSingleton<SystemRegistry>()
            .AddSingleton<ISystemRegistry>(x => x.GetRequiredService<SystemRegistry>())
            .AddSingleton<IEntityManager, EntityManager>()
            .AddSingleton<IOmpEntityProvider, OmpEntityProvider>()
            .AddSingleton<IServerService, ServerService>()
            .AddSingleton<IWorldService, WorldService>()
            .AddSingleton<IVehicleInfoService, VehicleInfoService>()
            // TODO: .AddSingleton<IPlayerCommandService, PlayerCommandService>()
            // TODO: .AddSingleton<IRconCommandService, RconCommandService>()
            .AddSingleton<ITimerService>(s => s.GetRequiredService<TimerSystem>())
            .AddSingleton<IDialogService, DialogService>()
            .AddSystem<DialogSystem>()
            .AddSystem<TimerSystem>()
            .AddSystem<TickingSystem>()
            .AddSystem<ActorSystem>()
            .AddSystem<ConsoleSystem>()
            .AddSystem<GangZoneSystem>()
            .AddSystem<MenuSystem>()
            .AddSystem<ObjectSystem>()
            .AddSystem<PickupSystem>()
            .AddSystem<PlayerChangeSystem>()
            .AddSystem<PlayerCheckSystem>()
            .AddSystem<PlayerClickSystem>()
            .AddSystem<PlayerConnectSystem>()
            .AddSystem<PlayerDamageSystem>()
            .AddSystem<PlayerShotSystem>()
            .AddSystem<PlayerSpawnSystem>()
            .AddSystem<PlayerStreamSystem>()
            .AddSystem<PlayerTextSystem>()
            .AddSystem<PlayerUpdateSystem>()
            .AddSystem<TextDrawSystem>()
            .AddSystem<VehicleSystem>()
            ;

    }
}