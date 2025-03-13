using BenchmarkDotNet.Attributes;
using SampSharp.Entities;

namespace Benchmarks;

[DryJob]
[MemoryDiagnoser]
public class BoxingBenchmark
{
    private readonly object[] _boxes = new object[256];
    
    private readonly MethodInvoker _invoker = CreateInvoke();
    private readonly SomeSystem _system = new();

    [Benchmark(OperationsPerInvoke = 100000 * 256, Baseline = true)]
    public void BoxedBool()
    {
        for (var j = 0; j < 100000; j++)
        {
            var state = false;
            for (var i = 0; i < 256; i++)
            {
                _boxes[i] = SimpleBox(state);
                state = !state;
            }
        }
    }
    
    [Benchmark(OperationsPerInvoke = 100000000)]
    public void MethodInvoker_does_it_box()
    {
        for (var j = 0; j < 100000000; j++)
        {
            var r= _invoker(_system, null, _system, null);
        }
    }
    
    [Benchmark(OperationsPerInvoke = 100000 * 256)]
    public void OptimizedBox()
    {
        for (var j = 0; j < 100000; j++)
        {
            var state = false;
            for (var i = 0; i < 256; i++)
            {
                _boxes[i] = AdvancedBox(state);
                state = !state;
            }
        }
    }

    public static object SimpleBox<T>(T eventResponse)
    {
        return eventResponse!;
    }
    
    public static object AdvancedBox<T>(T? eventResponse)
    {
        if (typeof(T) == typeof(bool))
        {
            return eventResponse is true ? ResponseValue.True : ResponseValue.False;
        }
        return eventResponse!;
    }

    public class SomeSystem : IServiceProvider
    {
        public bool BoolReturner()
        {
            return true;
        }

        public object? GetService(Type serviceType)
        {
            return null;
        }
    }

    private static MethodInvoker CreateInvoke()
    {
        return MethodInvokerFactory.Compile(typeof(SomeSystem).GetMethod(nameof(SomeSystem.BoolReturner))!, []);
    }
    public sealed record ResponseValue(bool Value)
    {
        public static ResponseValue True = new ResponseValue(true);
        public static ResponseValue False = new ResponseValue(false);
    }
}