using System.Reflection;
using SampSharp.Entities.Utilities;

namespace SampSharp.Entities;

internal class EventService : IEventService
{
    private static readonly Type[] _defaultParameterTypes =
    {
        typeof(string)
        //typeof(int[]),
        //typeof(bool[]),
        //typeof(float[]),
    };

    private readonly Dictionary<string, Event> _events = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IEntityManager _entityManager;
    private readonly RuntimeInformation _runtimeInformation;

    /// <summary>Initializes a new instance of the <see cref="EventService" /> class.</summary>
    public EventService(IServiceProvider serviceProvider, IEntityManager entityManager, RuntimeInformation runtimeInformation)
    {
        _serviceProvider = serviceProvider;
        _entityManager = entityManager;
        _runtimeInformation = runtimeInformation;

        CreateEventsFromAssemblies();
    }

    /// <inheritdoc />
    public void EnableEvent(string name, Type[] parameters)
    {
        var handler = BuildInvoke(name);
        // TODO: _gameModeClient.RegisterCallback(name, handler.Target, handler.Method, parameters);
    }

    /// <inheritdoc />
    public void UseMiddleware(string name, Func<EventDelegate, EventDelegate> middleware)
    {
        if (!_events.TryGetValue(name, out var @event))
        {
            _events[name] = @event = new Event(Invoke);
        }

        @event.Middleware.Add(middleware);

        // In order to chain the middleware from first to last, the middleware must be nested from last to first
        EventDelegate invoke = Invoke;
        for (var i = @event.Middleware.Count - 1; i >= 0; i--) invoke = @event.Middleware[i](invoke);

        @event.Invoke = invoke;
    }

    /// <inheritdoc />
    public object? Invoke(string name, params object[] arguments)
    {
        // TODO Could cache built invokers into a dictionary
        return BuildInvoke(name)(arguments);
    }

    private object? Invoke(EventContext context)
    {
        object? result = null;

        if (!_events.TryGetValue(context.Name, out var evt))
        {
            return null;
        }

        foreach (var sysEvt in evt.Invokers)
        {
            var system = _serviceProvider.GetService(sysEvt.TargetType);

            // System is not loaded. Skip invoking target.
            if (system == null)
            {
                continue;
            }

            result = sysEvt.Invoke(system, context) ?? result;
        }

        return result;
    }

    private void CreateEventsFromAssemblies()
    {
        // Find methods with EventAttribute in any ISystem in any assembly.
        var events = new AssemblyScanner()
            .IncludeAssembly(_runtimeInformation.EntryAssembly)
            .IncludeReferencedAssemblies()
            .IncludeNonPublicMembers()
            .Implements<ISystem>()
            .ScanMethods<EventAttribute>();

        // Gather event data, compile invoker and add the data to the events collection.
        foreach (var (method, attribute) in events)
        {
            // TODO: CoreLog.LogDebug("Adding event listener on {0}.{1}.", method.DeclaringType, method.Name);

            var name = attribute.Name ?? method.Name;

            if (!_events.TryGetValue(name, out var @event))
            {
                _events[name] = @event = new Event(Invoke);
            }

            var argsPtr = 0; // The current pointer in the event arguments array.
            var parameterSources = method.GetParameters()
                .Select(info => new MethodParameterSource(info))
                .ToArray();

            // Determine the source of each parameter.
            foreach (var source in parameterSources)
            {
                var type = source.Info.ParameterType;

                if (typeof(Component).IsAssignableFrom(type))
                {
                    // Components are provided by the entity in the arguments array of the event.
                    source.ParameterIndex = argsPtr++;
                    source.IsComponent = true;
                }
                else if (type.IsValueType || _defaultParameterTypes.Contains(type))
                {
                    // Default types are passed straight through.
                    source.ParameterIndex = argsPtr++;
                }
                else
                {
                    // Other types are provided trough Dependency Injection.
                    source.IsService = true;
                }
            }

            var invoker = CreateInvoker(method, parameterSources, argsPtr);
            @event.Invokers.Add(invoker);
        }
    }

    private InvokerInfo CreateInvoker(MethodInfo method, MethodParameterSource[] parameterInfos, int callbackParamCount)
    {
        var compiled = MethodInvokerFactory.Compile(method, parameterInfos);

        return new InvokerInfo
        {
            TargetType = method.DeclaringType!,
            Invoke = (instance, eventContext) =>
            {
                var args = eventContext.Arguments;
                if (callbackParamCount == args.Length)
                {
                    return compiled?.Invoke(instance, args, eventContext.EventServices, _entityManager);
                }

                // TODO: CoreLog.Log(CoreLogLevel.Error, $"Callback parameter count mismatch {callbackParamCount} != {args.Length}");
                return null;
            }
        };
    }

    private Func<object[], object?> BuildInvoke(string name)
    {
        var context = new EventContextImpl();
        context.SetName(name);
        context.SetEventServices(_serviceProvider);

        return args =>
        {
            context.SetArguments(args);

            if (!_events.TryGetValue(name, out var @event))
            {
                return null;
            }

            var result = @event.Invoke(context);

            return result switch
            {
                Task<bool> task => !task.IsCompleted ? null : task.Result,
                Task<int> task => !task.IsCompleted ? null : task.Result,
                Task => null,
                _ => result
            };
        };
    }

    private sealed class Event
    {
        public readonly List<InvokerInfo> Invokers = [];

        public readonly List<Func<EventDelegate, EventDelegate>> Middleware = [];

        public EventDelegate Invoke;

        public Event(EventDelegate invoke)
        {
            Invoke = invoke;
        }
    }

    private sealed class InvokerInfo
    {
        public required Func<object, EventContext, object?> Invoke { get; init; }
        public required Type TargetType { get; init; }
    }
}