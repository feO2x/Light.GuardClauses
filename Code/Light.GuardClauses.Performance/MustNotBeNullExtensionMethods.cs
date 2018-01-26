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