using System.Collections.Concurrent;

namespace SampSharp.Entities.Containers;

internal class DefaultServiceContainer(Dictionary<Type, ServiceData> data) : IServiceProvider, IDisposable
{
    private readonly ConcurrentDictionary<Type, object> _empties = [];

    public object? GetService(Type serviceType)
    {
        if (data.TryGetValue(serviceType, out var s))
        {
            return s.Get(this);
        }

        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return _empties.GetOrAdd(serviceType, static (t) =>
            {
                var a = t.GetGenericArguments()[0];
                var b = Array.CreateInstance(a, 0);
                return b;
            });
        }

        return null;
    }

    public void Dispose()
    {
        var exceptions = new List<Exception>();
        foreach (var kv in data)
        {
            if (kv.Value.Instance is IDisposable disposable and not IServiceProvider)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException("An error occurred while disposing the service provider.", exceptions);
        }
    }
}