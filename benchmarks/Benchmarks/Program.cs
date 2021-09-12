using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AppSingletonTests>();
            BenchmarkRunner.Run<TenantSingletonTests>();
        }
    }
}
