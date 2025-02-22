namespace SampSharp.Entities.Containers;

internal class ServiceData(Type serviceType, object? instance, Func<IServiceProvider, object>? factory, Type[]? implementationTypes)
{
    public Type ServiceType { get; } = serviceType;
    public Type[]? ImplementationTypes { get; } = implementationTypes;

    public object? Instance { get; private set; } = instance;

    public object? Get(IServiceProvider provider)
    {
        if (Instance == null && factory != null)
        {
            Instance = factory(provider);
        }

        return Instance;
    }
}