using BenchmarkDotNet.Running;
using Light.GuardClauses.Performance.MustBeNull;

namespace Light.GuardClauses.Performance
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<MustBeNullBenchmark>();
        }
    }
}