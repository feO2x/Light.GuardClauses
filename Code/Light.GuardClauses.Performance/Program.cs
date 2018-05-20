using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Light.GuardClauses.Performance.CollectionAssertions;

namespace Light.GuardClauses.Performance
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<MustHaveCountBenchmark>();
        }

        private static void RunAllBenchmarks()
        {
            var benchmarkTypes = typeof(Program).Assembly
                                                .ExportedTypes
                                                .Where(t => t.GetCustomAttribute<ClrJobAttribute>(true) != null)
                                                .ToList();

            foreach (var benchmarkType in benchmarkTypes)
            {
                BenchmarkRunner.Run(benchmarkType);
            }
        }
    }
}