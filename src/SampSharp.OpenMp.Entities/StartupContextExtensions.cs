using Microsoft.Extensions.DependencyInjection;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

public static class StartupContextExtensions
{
    public static StartupContext UseEntities(this StartupContext context)
    {
        if (context.Configurator is not IEcsStartup)
        {
            throw new InvalidOperationException("The startup type does not implement the 'IEcsStartup' interface.");
        }

        var manager = new EcsManager();
        context.Core.AddExtension(manager);
        context.Cleanup += (sender, args) => context.Core.RemoveExtension(manager);

        context.Initialized += OnContextInitialized; 
        return context;
    }

    public static StartupContext UseServiceProviderFactory<TContainerBuilder>(this StartupContext context, IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        where TContainerBuilder : notnull
    {
        var builder = context.GetServiceBuilder();

        builder.ServiceProviderFactory = services => serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(services));

        return context;
    }

    internal static ServiceBuilderExtension GetServiceBuilder(this StartupContext context)
    {
        var ext = context.Core.TryGetExtension<ServiceBuilderExtension>();

        if (ext == null)
        {
            ext = new ServiceBuilderExtension();
            context.Core.AddExtension(ext);
        }

        return ext;
    }

    private static void OnContextInitialized(object? sender, EventArgs e)
    {
        var context = (StartupContext)sender!;
        var manager = context.Core.GetExtension<EcsManager>();

        manager.Initialize(context);
    }
}