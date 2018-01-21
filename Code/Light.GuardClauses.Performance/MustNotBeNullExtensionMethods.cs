using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses.Performance
{
    public static class MustNotBeNullExtensionMethods
    {
        /*
         * All parameters
         * No aggressive inlining
         * Throws exception in same method
         * Returns fast
         */
        public static T V1<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
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
        public static T V2<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
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
        public static T V3<T>(this T parameter, string parameterName = null, string message = null) where T : class
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
        public static T V4<T>(this T parameter, string parameterName = null) where T : class
        {
            if (parameter != null)
                return parameter;

            ThrowMustNotBeNullException(parameterName);
            return null;
        }

        /*
         * All parameters
         * Aggressive inlining
         * Throws exception in different method
         * Returns fast
         * No generic restriction
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T V5<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter != null)
                return parameter;

            throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /*
         * All parameters
         * Aggressive inlining
         * Throws exception in different method
         * Returns fast
         * No generics
         * No return parameter
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void V6(this object parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter != null)
                return;

            throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /*
         * All parameters
         * Aggressive inlining
         * Throws exception in same method
         * Returns fast
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T V7<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (parameter == null)
                throw exception?.Invoke() ?? throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

            return parameter;
        }

        private static void ThrowMustNotBeNullException(string parameterName)
        {
            throw new ArgumentNullException(parameterName);
        }

        private static void ThrowMustNotBeNullException(string parameterName, string message)
        {
            throw new ArgumentNullException(parameterName, message);
        }
    }
}