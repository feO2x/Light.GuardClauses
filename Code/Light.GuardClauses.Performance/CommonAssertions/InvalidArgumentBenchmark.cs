using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.ExceptionFactory;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class InvalidArgumentBenchmark
    {
        public bool Condition = false;
        public const string Message = "You must not call this method when Condition is true";
        public const string ParameterName = "parameter";

        [Benchmark(Baseline = true)]
        public bool ImperativeVersion()
        {
            if (Condition) throw new ArgumentException(Message, ParameterName);
            return Condition;
        }

        [Benchmark]
        public bool LightGuardClauses()
        {
            Check.InvalidArgument(Condition, ParameterName, Message);
            return Condition;
        }

        [Benchmark]
        public bool LightGuardClausesCustomException()
        {
            Check.InvalidArgument(Condition, () => new ArgumentException(Message, ParameterName));
            return Condition;
        }

        [Benchmark]
        public bool LightGuardClausesCustomExceptionManualInlining()
        {
            if (Condition)
                Throw.CustomException(() => new ArgumentException(Message, ParameterName));
            return Condition;
        }

        [Benchmark]
        public bool LightGuardClausesCustomExceptionWithParameter()
        {
            Check.InvalidArgument<string>(Condition, null, _ => new ArgumentException(Message, ParameterName));
            return Condition;
        }
    }
}