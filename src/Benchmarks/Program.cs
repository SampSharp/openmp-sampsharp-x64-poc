using BenchmarkDotNet.Running;

namespace Benchmarks;

public class Program
{
    public static void Main()
    {
        new BoxingBenchmark().MethodInvoker_does_it_box();
        BenchmarkRunner.Run<BoxingBenchmark>();
    }
}