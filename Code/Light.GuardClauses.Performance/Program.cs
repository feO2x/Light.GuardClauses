using BenchmarkDotNet.Running;
using Light.GuardClauses.Performance.MustBeOfType;

namespace Light.GuardClauses.Performance
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<MustBeOfTypeBenchmarks>();
        }
    }
}