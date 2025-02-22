using Microsoft.Extensions.DependencyInjection;

namespace SampSharp.Entities.Containers;

internal class DefaultServiceContainerFactory : IServiceProviderFactory<DefaultServiceContainerBuilder>
{
    public DefaultServiceContainerBuilder CreateBuilder(IServiceCollection services)
    {
        var data = new List<ServiceData>();
        foreach (var service in services)
        {
            if (service.IsKeyedService)
            {
                throw new NotSupportedException("Keyed service is not supported.");
            }

            switch(service.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    if (service.ImplementationInstance != null)
                    {
                        data.Add(new ServiceData(service.ServiceType, service.ImplementationInstance, null, null));
                    }
                    else if(service.ImplementationFactory != null)
                    {
                        data.Add(new ServiceData(service.ServiceType, null, service.ImplementationFactory, null));
                    }
                    else if (service.ImplementationType != null)
                    {
                        data.Add(new ServiceData(service.ServiceType, null, sp => ActivatorUtilities.CreateInstance(sp, service.ImplementationType), [service.ImplementationType]));
                    }

                    break;
                default:
                    throw new NotSupportedException($"Lifetime {service.Lifetime} is not supported.");
            }
        }

        return new DefaultServiceContainerBuilder(data);
    }

    public IServiceProvider CreateServiceProvider(DefaultServiceContainerBuilder containerBuilder)
    {
        return containerBuilder.Compile();
    }
}