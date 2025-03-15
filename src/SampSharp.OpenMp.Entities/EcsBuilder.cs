using Microsoft.Extensions.DependencyInjection;

namespace SampSharp.Entities;

internal class EcsBuilder : IEcsBuilder
{
    private readonly IEventDispatcher _eventDispatcher;

    internal EcsBuilder(IServiceProvider services)
    {
        Services = services;
        _eventDispatcher = services.GetRequiredService<IEventDispatcher>();
    }

    public IServiceProvider Services { get; }
    
    public IEcsBuilder UseMiddleware(string name, Func<EventDelegate, EventDelegate> middleware)
    {
        _eventDispatcher.UseMiddleware(name, middleware);
        return this;
    }
}