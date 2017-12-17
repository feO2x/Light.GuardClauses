using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance
{
    public class NullCheck
    {
        public object Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark(Baseline = true)]
        public object InlineVersion()
        {
            if (Instance == null)
                throw new ArgumentNullException(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public object LightGuardClausesVersion()
        {
            return Instance.MustNotBeNull(nameof(Instance));
        }

        [Benchmark]
        public object NonGenericVersion()
        {
            return MustNotBeNull(Instance);
        }

        private static object MustNotBeNull(object @object, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (@object == null)
                throw exception?.Invoke() ?? new ArgumentNullException(parameterName, message);

            return @object;
        }
    }

    public class SampleEntity
    {
        public readonly Guid Id;

        public SampleEntity(Guid id)
        {
            Id = id.MustNotBeEmpty(nameof(id));
        }
    }
}