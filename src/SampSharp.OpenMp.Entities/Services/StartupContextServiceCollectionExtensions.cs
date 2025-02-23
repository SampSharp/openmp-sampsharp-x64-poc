using Microsoft.Extensions.DependencyInjection;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

public static class StartupContextServiceCollectionExtensions
{
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
}