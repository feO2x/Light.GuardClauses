using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     Provides static methods that throw default exceptions.
    /// </summary>
    public abstract class Throw
    {
        /// <summary>
        ///     Throws the default <see cref="System.ArgumentNullException" />, using the optional parameter name and message.
        /// </summary>
        public static void ArgumentNullException(string parameterName = null, string message = null)
        {
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /// <summary>
        ///     Throws the default <see cref="ArgumentException" /> indicating that a value is the default value of its type, using the optional parameter name and message.
        /// </summary>
        public static void ArgumentDefaultException(string parameterName, string message)
        {
            throw new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be the default value.", parameterName);
        }

        /// <summary>
        ///     Throws the default <see cref="ArgumentNotNullException" /> indicating that a value is not null, using the optional parameter name and message.
        /// </summary>
        public static void ArgumentNotNullException(object value, string parameterName = null, string message = null)
        {
            throw new ArgumentNotNullException(parameterName, message ?? $"{parameterName ?? "The value"} must be null, but it actually is \"{value}\".");
        }

        /// <summary>
        ///     Throws the exception that is returned by <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException(Func<Exception> exceptionFactory)
        {
            throw exceptionFactory();
        }

        /// <summary>
        ///     Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="value" /> is passed to <paramref name="exceptionFactory"/>.
        /// </summary>
        public static void CustomException<T>(Func<T, Exception> exceptionFactory, T value)
        {
            throw exceptionFactory(value);
        }
    }
}