using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities.SAMP;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

[Extension(0x57e43771d28c5e7e)]
internal class EcsManager : Extension
{
    private IServiceProvider? _serviceProvider;

    public void Initialize(StartupContext context)
    {
        var configurator = (IEcsStartup)context.Configurator;

        var services = new ServiceCollection();

        var info = new RuntimeInformation(configurator.GetType().Assembly);

        services.AddSingleton(info);
        services.AddSingleton(new OpenMp(context.Core, context.ComponentList));

        Configure(services);
        configurator.Configure(services);

        _serviceProvider = context.GetServiceBuilder().ServiceProviderFactory(services);

        var builder = new EcsBuilder(_serviceProvider);

        configurator.Configure(builder);

        AddWrappedSystemTypes();

        Run();
    }

    private void Run()
    {
        _serviceProvider!.GetRequiredService<IEventService>().Invoke("OnGameModeInit");
    }

    protected override void Cleanup()
    {
        _serviceProvider?.GetRequiredService<IEventService>().Invoke("OnGameModeExit");

        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void AddWrappedSystemTypes()
    {
        var types = _serviceProvider!.GetServices<SystemTypeWrapper>()
            .Select(w => w.Type)
            .ToArray();

        _serviceProvider!.GetRequiredService<ISystemRegistry>().Configure(types);
    }

    private static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IEventService, EventService>()
            .AddSingleton<ISystemRegistry, SystemRegistry>()
            .AddSingleton<IEntityManager, EntityManager>()
            .AddSingleton<IOmpEntityProvider, OmpEntityProvider>()
            // TODO: .AddSingleton<IServerService, ServerService>()
            .AddSingleton<IWorldService, WorldService>()
            // TODO: .AddSingleton<IVehicleInfoService, VehicleInfoService>()
            // TODO: .AddSingleton<IPlayerCommandService, PlayerCommandService>()
            // TODO: .AddSingleton<IRconCommandService, RconCommandService>()
            // TODO: .AddSingleton<ITimerService>(s => s.GetRequiredService<TimerSystem>())
            // TODO: .AddTransient<IDialogService, DialogService>()
            // TODO: .AddSystem<DialogSystem>()
            // TODO: .AddSystem<TimerSystem>()
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