using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class InvalidStateBenchmark
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
            Check.InvalidState(Condition, Message);
            return Condition;
        }
    }
}