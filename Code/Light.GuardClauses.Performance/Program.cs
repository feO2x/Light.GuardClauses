using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Light.GuardClauses.Performance.EqualityAssertions;

namespace Light.GuardClauses.Performance
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<IsSameAsBenchmarks>();
        }

        private static void RunAllBenchmarks()
        {
            var ignoredTypes = new[]
                               {
                                   typeof(LoopCheck),
                                   typeof(IsSubsetOfGist),
                                   typeof(IsSubsetOfPerformance)
                               };

            var benchmarkTypes = typeof(Program).Assembly
                                                .ExportedTypes
                                                .Where(t => t.GetCustomAttribute<ClrJobAttribute>(true) != null &&
                                                            t.IsOneOf(ignoredTypes) == false)
                                                .ToList();

            foreach (var benchmarkType in benchmarkTypes)
            {
                BenchmarkRunner.Run(benchmarkType);
            }
        }
    }
}