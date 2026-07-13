using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeNullBenchmark
    {
        public SampleEntity Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark(Baseline = true)]
        public SampleEntity Imperative()
        {
            if (Instance == null)
                throw new ArgumentNullException(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity ImperativeWithNullcoalescing() => Instance ?? throw new ArgumentNullException(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV1() => Instance.MustNotBeNullV1(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV2() => Instance.MustNotBeNullV2(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV3() => Instance.MustNotBeNullV3(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV4() => Instance.MustNotBeNullV4(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV5() => Instance.MustNotBeNullV5(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV6()
        {
            Instance.MustNotBeNullV6(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity CustomParameterV7() => Instance.MustNotBeNullV7(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV8() => Instance.MustNotBeNullV8(nameof(Instance));

        [Benchmark]
        public SampleEntity CustomParameterV10() => Instance.MustNotBeNullV10(nameof(Instance));

        [Benchmark]
        public SampleEntity LightGuardClausesCustomParameter() => Instance.MustNotBeNull(nameof(Instance));

        [Benchmark]
        public SampleEntity ImperativeCustomException()
        {
            if (Instance == null)
                throw new ArgumentException("Instance must not be null.", nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity CustomExceptionV1() => Instance.MustNotBeNullV1(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity CustomExceptionV2() => Instance.MustNotBeNullV2(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity CustomExceptionV5() => Instance.MustNotBeNullV5(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity CustomExceptionV6()
        {
            Instance.MustNotBeNullV6(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));
            return Instance;
        }

        [Benchmark]
        public SampleEntity CustomExceptionV7() => Instance.MustNotBeNullV7(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity CustomExceptionV9() => Instance.MustNotBeNullV9(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity CustomExceptionV11() => Instance.MustNotBeNullV11(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity CustomExceptionV12() => Instance.MustNotBeNullV12(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity LightGuardClausesCustomException() => Instance.MustNotBeNull(() => new ArgumentException("Instance must not be null.", nameof(Instance)));
    }

    public static class MustNotBeNullExtensionMethods
    {
        /*
         * All parameters
         * No aggressive inlining
         * Throws exception in same method
         * Returns fast
         */
        public static T MustNotBeNullV1<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (parameter != null)
                return parameter;

            throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /*
         * All parameters
         * Aggressive inlining
         * Throws exception in same method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV2<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (parameter != null)
                return parameter;

            throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }


        /*
         * Only parameterName and message parameters
         * Aggressive inlining
         * Throws exception in different method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV3<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter != null)
                return parameter;

            ThrowMustNotBeNullException(parameterName, message);
            return null;
        }

        /*
         * Only parameterName parameter
         * Aggressive inlining
         * Throws exception in different method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV4<T>(this T parameter, string parameterName = null) where T : class
        {
            if (parameter != null)
                return parameter;

            ThrowMustNotBeNullException(parameterName);
            return null;
        }

        /*
         * All parameters
         * Aggressive inlining
         * Throws exception in same method
         * Returns fast
         * No generic restriction
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV5<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter != null)
                return parameter;

            throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /*
         * All parameters
         * Aggressive inlining
         * Throws exception in same method
         * Returns fast
         * No generics
         * No return parameter
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustNotBeNullV6(this object parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter != null)
                return;

            throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /*
         * All parameters
         * Aggressive inlining
         * Throws exception in same method
         * if block throws exception
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV7<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (parameter == null)
                throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

            return parameter;
        }

        /*
         * Only parameterName and message parameters
         * Aggressive inlining
         * Throws exception in same method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV8<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter != null)
                return parameter;

            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /*
         * Only exception parameter
         * Aggressive inlining
         * Throws exception in same method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV9<T>(this T parameter, Func<Exception> exception) where T : class
        {
            if (parameter != null)
                return parameter;

            throw exception();
        }

        /*
         * Only exception parameter
         * Aggressive inlining
         * Throws exception in local function
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV10<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter != null)
                return parameter;

            ThrowMustNotBeNullException(parameterName, message);
            return null;

            void ThrowMustNotBeNullException(string p, string m)
            {
                throw new ArgumentNullException(p, m ?? $"{p ?? "The value"} must not be null.");
            }
        }

        /*
         * Only exception parameter
         * Aggressive inlining
         * Throws exception in different method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV11<T>(this T parameter, Func<Exception> exception) where T : class
        {
            if (parameter != null)
                return parameter;

            ThrowCustomException(exception);
            return null;
        }

        /*
         * Only exception parameter
         * Aggressive inlining
         * Throws exception in different method, creating the exception in the same method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullV12<T>(this T parameter, Func<Exception> exception) where T : class
        {
            if (parameter != null)
                return parameter;

            ThrowCustomException(exception());
            return null;
        }

        private static void ThrowMustNotBeNullException(string parameterName)
        {
            throw new ArgumentNullException(parameterName);
        }

        private static void ThrowMustNotBeNullException(string parameterName, string message)
        {
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        private static void ThrowCustomException(Func<Exception> exception)
        {
            throw exception();
        }

        private static void ThrowCustomException(Exception exception)
        {
            throw exception;
        }
    }
}