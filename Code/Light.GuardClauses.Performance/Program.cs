using BenchmarkDotNet.Running;

namespace Light.GuardClauses.Performance
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<LoopCheck>();
        }
    }
}