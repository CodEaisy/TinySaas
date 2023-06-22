using BenchmarkDotNet.Running;
using Benchmarks.Jobs;

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
