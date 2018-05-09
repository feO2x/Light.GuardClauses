using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class InvalidStateBenchmark : DefaultBenchmark
    {
        public const string Message = "Condition must not be true.";
        public bool Condition = false;

        [Benchmark(Baseline = true)]
        public bool ImperativeVersion()
        {
            if (Condition) throw new InvalidStateException(Message);
            return Condition;
        }

        [Benchmark]
        public bool LightGuardClauses()
        {
            Guard.InvalidState(Condition, Message);
            return Condition;
        }
    }
}