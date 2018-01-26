using BenchmarkDotNet.Running;

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