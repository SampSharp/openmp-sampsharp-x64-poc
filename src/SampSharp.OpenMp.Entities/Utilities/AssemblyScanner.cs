﻿using System.Reflection;

namespace SampSharp.Entities.Utilities;

/// <summary>Represents a utility for scanning for types and members with specific attributes in loaded assemblies.</summary>
internal sealed class AssemblyScanner
{
    private List<Assembly> _assemblies = [];
    private List<Type> _classAttributes = [];
    private List<Type> _classImplements = [];
    private bool _includeInstanceMembers = true;
    private bool _includeNonPublicMembers;
    private bool _includeStaticMembers;
    private List<Type> _memberAttributes = [];
    private bool _includeAbstract;

    private BindingFlags MemberBindingFlags =>
        (_includeInstanceMembers
            ? BindingFlags.Instance
            : BindingFlags.Default) | (_includeStaticMembers
            ? BindingFlags.Static
            : BindingFlags.Default) | BindingFlags.Public | (_includeNonPublicMembers
            ? BindingFlags.NonPublic
            : BindingFlags.Default);

    /// <summary>Includes the specified <paramref name="assembly" /> in the scan.</summary>
    /// <param name="assembly">The assembly to include.</param>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner IncludeAssembly(Assembly assembly)
    {
        if (_assemblies.Contains(assembly))
        {
            return this;
        }

        var result = Clone();
        result._assemblies.Add(assembly);
        return result;
    }

    /// <summary>Includes the referenced assemblies of the previously included assemblies in the scan.</summary>
    /// <param name="skipSystem">If set to <c>true</c>, system assemblies (System.*, Microsoft.*, netstandard) are skipped in the scan.</param>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner IncludeReferencedAssemblies(bool skipSystem = true)
    {
        var assemblies = new List<Assembly>();

        foreach (var a in _assemblies)
        {
            AddToScan(a);
        }

        var result = Clone();
        result._assemblies = assemblies;
        return result;

        void AddToScan(Assembly asm)
        {
            if (assemblies.Contains(asm))
            {
                return;
            }

            assemblies.Add(asm);

            foreach (var assemblyRef in asm.GetReferencedAssemblies())
            {
                if (skipSystem && IsSystemAssembly(assemblyRef))
                {
                    continue;
                }

                AddToScan(Assembly.Load(assemblyRef));
            }
        }
    }

    private static bool IsSystemAssembly(AssemblyName assemblyRef)
    {
        return (assemblyRef.Name!.StartsWith("System", StringComparison.InvariantCulture) ||
                assemblyRef.Name.StartsWith("Microsoft", StringComparison.InvariantCulture) ||
                assemblyRef.Name.StartsWith("netstandard", StringComparison.InvariantCulture));
    }

    /// <summary>Includes static members in the scan.</summary>
    /// <param name="exclusive">If set to <c>true</c>, only include static members in the scan.</param>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner IncludeStatic(bool exclusive)
    {
        var result = Clone();
        result._includeStaticMembers = true;
        result._includeInstanceMembers = !exclusive;
        return result;
    }

    /// <summary>Includes non-public members in the scan.</summary>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner IncludeNonPublicMembers()
    {
        var result = Clone();
        result._includeNonPublicMembers = true;
        return result;
    }

    /// <summary>Includes members of abstract classes in the scan.</summary>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner IncludeAbstract()
    {
        var result = Clone();
        result._includeAbstract = true;
        return result;
    }

    /// <summary>Includes only members of classes which implement <typeparamref name="T" /> in the scan.</summary>
    /// <typeparam name="T">The class or interface the results of the scan should implement.</typeparam>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner Implements<T>()
    {
        var result = Clone();
        result._classImplements.Add(typeof(T));
        return result;
    }

    /// <summary>Includes only members of classes which have an attribute <typeparamref name="T" />.</summary>
    /// <typeparam name="T">The type of the attribute the class should have.</typeparam>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner HasClassAttribute<T>() where T : Attribute
    {
        var result = Clone();
        result._classAttributes.Add(typeof(T));
        return result;
    }

    /// <summary>Includes only members which have an attribute <typeparamref name="T" />.</summary>
    /// <typeparam name="T">The type of the attribute the member should have.</typeparam>
    /// <returns>An updated scanner.</returns>
    public AssemblyScanner HasAttribute<T>() where T : Attribute
    {
        var result = Clone();
        result._memberAttributes.Add(typeof(T));
        return result;
    }

    private bool ApplyTypeFilter(Type type)
    {
        return type.IsClass && 
               (_includeAbstract || !type.IsAbstract) && 
               _classImplements.All(i => i.IsAssignableFrom(type)) &&
               _classAttributes.All(a => type.GetCustomAttribute(a) != null);
    }

    private bool ApplyMemberFilter(MemberInfo memberInfo)
    {
        return _memberAttributes.All(a => memberInfo.GetCustomAttribute(a) != null);
    }

    /// <summary>Runs the scan for methods and provides the attribute <typeparamref name="TAttribute" /> in the
    /// results.</summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns>The found methods with their attribute of type <typeparamref name="TAttribute" />.</returns>
    public IEnumerable<(Type type, TAttribute attribute)> ScanTypes<TAttribute>() where TAttribute : Attribute
    {
        return HasClassAttribute<TAttribute>()
            .ScanTypes()
            .Select(type => (type, attribute: type.GetCustomAttribute<TAttribute>()!));
    }

    /// <summary>Runs the scan for methods.</summary>
    /// <returns>The found methods.</returns>
    public IEnumerable<Type> ScanTypes()
    {
        return _assemblies.SelectMany(a => a.GetTypes())
            .Where(ApplyTypeFilter)
            .ToArray();
    }

    /// <summary>Runs the scan for methods and provides the attribute <typeparamref name="TAttribute" /> in the
    /// results.</summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns>The found methods with their attribute of type <typeparamref name="TAttribute" />.</returns>
    public IEnumerable<(Type target, MethodInfo method, TAttribute attribute)> ScanMethods<TAttribute>() where TAttribute : Attribute
    {
        return HasAttribute<TAttribute>()
            .ScanMethods()
            .Select(x => (x.target, x.method, attribute: x.method.GetCustomAttribute<TAttribute>()!));
    }

    /// <summary>Runs the scan for methods.</summary>
    /// <returns>The found methods.</returns>
    public IEnumerable<(Type target, MethodInfo method)> ScanMethods()
    {
        return _assemblies.SelectMany(a => a.GetTypes())
            .Where(ApplyTypeFilter)
            .SelectMany(t => t.GetMethods(MemberBindingFlags).Select(m => (t, m)))
            .Where(x => ApplyMemberFilter(x.m))
            .ToArray();
    }

    /// <summary>Runs the scan for fields and provides the attribute <typeparamref name="TAttribute" /> in the
    /// results.</summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns>The found fields with their attribute of type <typeparamref name="TAttribute" />.</returns>
    public IEnumerable<(FieldInfo field, TAttribute attribute)> ScanFields<TAttribute>() where TAttribute : Attribute
    {
        return HasAttribute<TAttribute>()
            .ScanFields()
            .Select(field => (field, attribute: field.GetCustomAttribute<TAttribute>()!));
    }

    /// <summary>Runs the scan for fields.</summary>
    /// <returns>The found fields.</returns>
    public IEnumerable<FieldInfo> ScanFields()
    {
        return _assemblies.SelectMany(a => a.GetTypes())
            .Where(ApplyTypeFilter)
            .SelectMany(t => t.GetFields(MemberBindingFlags))
            .Where(ApplyMemberFilter)
            .ToArray();
    }

    private AssemblyScanner Clone()
    {
        return new AssemblyScanner
        {
            _assemblies = [.._assemblies],
            _includeStaticMembers = _includeStaticMembers,
            _includeNonPublicMembers = _includeNonPublicMembers,
            _includeInstanceMembers = _includeInstanceMembers,
            _classImplements = [.._classImplements],
            _classAttributes = [.._classAttributes],
            _memberAttributes = [.._memberAttributes],
            _includeAbstract = _includeAbstract
        };
    }
}