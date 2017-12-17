using BenchmarkDotNet.Running;

namespace Light.GuardClauses.Performance
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<NullCheck>();
        }
    }
}