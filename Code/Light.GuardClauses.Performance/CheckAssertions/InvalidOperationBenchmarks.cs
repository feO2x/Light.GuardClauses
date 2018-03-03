using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CheckAssertions
{
    public class InvalidOperationBenchmarks : DefaultBenchmark
    {
        private const string Message = "You must not call this method when Condition is true";
        public bool Condition = false;

        [Benchmark(Baseline = true)]
        public bool ImperativeVersion()
        {
            if (Condition) throw new InvalidOperationException(Message);
            return Condition;
        }

        [Benchmark]
        public bool LightGuardClauses()
        {
            Check.InvalidOperation(Condition, Message);
            return Condition;
        }
    }
}