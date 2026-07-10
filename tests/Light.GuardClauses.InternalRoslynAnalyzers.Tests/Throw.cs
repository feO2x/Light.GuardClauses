using System;

// ReSharper disable once CheckNamespace
namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// Provides static factory methods that throw default exceptions.
    /// </summary>
    public static class Throw
    {
        /// <summary>
        /// Throws the default <see cref="ArgumentNullException" />, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeNull(string parameterName = null, string message = null) =>
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException(Func<Exception> exceptionFactory) => throw exceptionFactory();
    }
}