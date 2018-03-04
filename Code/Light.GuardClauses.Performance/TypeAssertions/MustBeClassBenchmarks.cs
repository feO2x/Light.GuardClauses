using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.TypeAssertions
{
    public class MustBeClassBenchmarks : DefaultBenchmark
    {
        public Type Type = typeof(string);

        [Benchmark(Baseline = true)]
        public Type ImperativeVersion()
        {
            if (!(Type.IsClass && Type.BaseType != typeof(MulticastDelegate))) throw new TypeException(nameof(Type));
            return Type;
        }

        [Benchmark]
        public Type LightGuardClauses() => Type.MustBeClass(nameof(Type));

        [Benchmark]
        public Type OldVersion() => Type.OldMustBeClass(nameof(Type));
    }

    public static class MustBeClassExtensions
    {
        public static Type OldMustBeClass(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.OldIsClass())
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a class, but it is not.", parameterName);
        }
    }
}
