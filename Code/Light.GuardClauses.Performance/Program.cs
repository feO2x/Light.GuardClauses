using BenchmarkDotNet.Running;
using Light.GuardClauses.Performance.MustNotBeNull;

namespace Light.GuardClauses.Performance
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<MustNotBeNullWithParameterName>();
            BenchmarkRunner.Run<MustNotBeNullWithCustomException>();
        }
    }
}