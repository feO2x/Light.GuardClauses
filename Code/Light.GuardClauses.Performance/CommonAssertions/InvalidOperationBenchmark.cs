using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class InvalidOperationBenchmark : DefaultBenchmark
    {
        public bool Condition = false;
        public const string Message = "You must not call this method when Condition is true";

        [Benchmark(Baseline = true)]
        public bool ImperativeVersion()
        {
            if (Condition) throw new InvalidOperationException(Message);
            return Condition;
        }

        [Benchmark]
        public bool LightGuardClauses()
        {
            Guard.InvalidOperation(Condition, Message);
            return Condition;
        }
    }
}