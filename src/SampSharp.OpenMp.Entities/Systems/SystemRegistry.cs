namespace SampSharp.Entities;

internal sealed class SystemRegistry(IServiceProvider serviceProvider) : ISystemRegistry
{
    private Dictionary<Type, ISystem[]>? _data;

    public void Configure(Type[] systemImplementationTypes)
    {
        if (_data != null)
        {
            throw new SystemRegistryException("The system registry has been locked an cannot be modified.");
        }

        var data = new Dictionary<Type, HashSet<ISystem>>();

        foreach (var type in systemImplementationTypes)
        {
            if (serviceProvider.GetService(type) is not ISystem instance)
            {
                throw new SystemRegistryException($"System of type {type} could not be found in the service provider.");
            }

            var currentType = type;

            while (currentType != null && currentType != typeof(object))
            {
                if (!data.TryGetValue(currentType, out var set))
                {
                    data[currentType] = set = [];
                }

                set.Add(instance);

                currentType = currentType.BaseType;
            }

            foreach (var interfaceType in type.GetInterfaces()
                         .Where(t => typeof(ISystem).IsAssignableFrom(t)))
            {
                if (!data.TryGetValue(interfaceType, out var set))
                {
                    data[interfaceType] = set = [];
                }

                set.Add(instance);
            }
        }

        // Convert hash sets to arrays.
        _data = new Dictionary<Type, ISystem[]>();
        foreach (var kv in data)
        {
            _data[kv.Key] = kv.Value.ToArray();
        }
    }

    public ReadOnlyMemory<ISystem> Get(Type type)
    {
        return _data?.TryGetValue(type, out var value) ?? false ? value : default(Memory<ISystem>);
    }

    public ReadOnlyMemory<ISystem> Get<TSystem>() where TSystem : ISystem
    {
        return Get(typeof(TSystem));
    }
}