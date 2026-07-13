using System;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class SpanMustHaveLengthBenchmark
    {
        public byte[] Source = Encoding.UTF8.GetBytes("Some old wounds never truly heal, and bleed again at the slightest word.");

        [Benchmark(Baseline = true)]
        public Span<byte> ImperativeVersion()
        {
            var span = new Span<byte>(Source, 0, 32);
            if (span.Length != 32)
                throw new InvalidCollectionCountException(nameof(span));
            return span;
        }

        [Benchmark]
        public Span<byte> LightGuardClausesCopyByValue() =>
            new Span<byte>(Source, 0, 32).MustHaveLengthCopyByValue(32, "span");

        [Benchmark]
        public Span<byte> LightGuardClausesInParameter()
        {
            var span = new Span<byte>(Source, 0, 32);
            span.MustHaveLengthInParameter(32, nameof(span));

            return span;
        }

        [Benchmark]
        public Span<byte> LightGuardClausesInOut()
        {
            var span = new Span<byte>(Source, 0, 32);
            span.MustHaveLengthInOut(32, nameof(span));
            return span;
        }
    }

    public static class SpanMustHaveLengthExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustHaveLengthCopyByValue<T>(this Span<T> parameter, int length, string parameterName = null, string message = null)
        {
            if (parameter.Length != length)
                Throw.InvalidSpanLength((ReadOnlySpan<T>) parameter, length, parameterName, message);
            return parameter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustHaveLengthInParameter<T>(in this Span<T> parameter, int length, string parameterName = null, string message = null)
        {
            if (parameter.Length != length)
                Throw.InvalidSpanLength((ReadOnlySpan<T>) parameter, length, parameterName, message);
            return parameter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Span<T> MustHaveLengthInOut<T>(ref this Span<T> parameter, int length, string parameterName = null, string message = null)
        {
            if (parameter.Length != length)
                Throw.InvalidSpanLength((ReadOnlySpan<T>) parameter, length, parameterName, message);
            return ref parameter;
        }
    }
}