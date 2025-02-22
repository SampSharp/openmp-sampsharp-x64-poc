using Microsoft.Extensions.DependencyInjection;

namespace SampSharp.Entities;

internal class EcsBuilder : IEcsBuilder
{
    private readonly IEventService _eventService;

    internal EcsBuilder(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));

        _eventService = services.GetRequiredService<IEventService>();
    }

    public IServiceProvider Services { get; }

    public IEcsBuilder EnableEvent(string name, Type[] parameters)
    {
        _eventService.EnableEvent(name, parameters);

        return this;
    }

    public IEcsBuilder UseMiddleware(string name, Func<EventDelegate, EventDelegate> middleware)
    {
        _eventService.UseMiddleware(name, middleware);

        return this;
    }
}