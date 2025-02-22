using System.Collections;
using System.Reflection;

namespace SampSharp.Entities.Containers;

internal class DefaultServiceContainerBuilder(List<ServiceData> services)
{

    public IServiceProvider Compile()
    {
        var dictionary = Build();
        dictionary.Add(typeof(IServiceProvider), new ServiceData(typeof(IServiceProvider), null, sp => sp, null));

        Validate(dictionary);
        return new DefaultServiceContainer(dictionary);
    }

    private static void Validate(Dictionary<Type, ServiceData> dictionary)
    {
        var problems = new List<string>();
        var chain = new Stack<Type>();

        foreach (var kv in dictionary)
        {
            CheckCircularDependency(dictionary, chain, problems, kv.Key);
        }

        if (problems.Count > 0)
        {
            throw new InvalidOperationException($"Problems in dependency injection container.{Environment.NewLine}{Environment.NewLine}{string.Join(Environment.NewLine, problems)}");
        }
    }

    private static IEnumerable<Type> DetectDependencies(Type implementationType, List<string> problems)
    {
        var constructors = implementationType.GetConstructors();

        switch (constructors.Length)
        {
            case 0:
                problems.Add($"No public constructor found for {implementationType.FullName}");
                return [];
            case > 1:
                problems.Add($"Multiple public constructors found for {implementationType.FullName}");
                return [];
            default:
                {
                    var constructor = constructors[0];
                    return constructor.GetParameters().Select(x => x.ParameterType);
                }
        }
    }

    private static void CheckCircularDependency(Dictionary<Type, ServiceData> dictionary, Stack<Type> chain, List<string> problems, Type type)
    {
        if (!dictionary.TryGetValue(type, out var data))
        {
            problems.Add($"Missing dependency: {type.FullName}");
            return;
        }

        if (chain.Contains(type))
        {
            problems.Add($"Circular dependency: {string.Join(" -> ", chain.Select(x => x.FullName))} -> {type.FullName}");
            return;
        }

        chain.Push(type);

        if (data.ImplementationTypes != null)
        {
            foreach (var impl in data.ImplementationTypes)
            {
                chain.Push(impl);

                foreach (var dep in DetectDependencies(impl, problems))
                {
                    CheckCircularDependency(dictionary, chain, problems, dep);
                }

                chain.Pop();
            }
        }
  
        chain.Pop();
    }

    private Dictionary<Type, ServiceData> Build()
    {
        var dictionary = new Dictionary<Type, ServiceData>();
        foreach (var g in services.GroupBy(x => x.ServiceType))
        {
            var type = g.Key;
            var servicesOfType = g.ToArray();

            // single
            dictionary[type] = servicesOfType[0];

            // multi
            var enumType = typeof(IEnumerable<>).MakeGenericType(type);
            dictionary[enumType] = new ServiceData(enumType, null, provider =>
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var list = (IList)Activator.CreateInstance(listType)!;
                foreach (var service in servicesOfType)
                {
                    list.Add(service.Get(provider));
                }

                return list;
            }, servicesOfType.Where(x => x.ImplementationTypes != null).SelectMany(x => x.ImplementationTypes!).ToArray());
        }

        return dictionary;
    }
}