using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Light.GuardClauses.Performance
{
    public static class Program
    {
        public static void Main()
        {
            RunAllBenchmarks();
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