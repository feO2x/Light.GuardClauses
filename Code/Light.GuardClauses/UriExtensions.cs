using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Provides assertion methods for the <see cref="Uri" /> class.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        ///     Ensures that the specified URI is an absolute one, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="uri" /> is not an absolute URI.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeAbsoluteUri(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);

            if (uri.IsAbsoluteUri)
                return;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The URI"} must be an absolute URI, but you specified \"{uri}\".", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified URI has the given scheme, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="uri" /> is not an absolute URI.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> does not have the specified scheme.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveScheme(this Uri uri, string scheme, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);

            if (uri.IsAbsoluteUri && uri.Scheme == scheme)
                return;

            var subclause = uri.IsAbsoluteUri ? $"but actually has scheme \"{uri.Scheme}\" (\"{uri}\")." : $"but it has none because it is a relative URI (\"{uri}\").";
            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The URI"} must have scheme \"{scheme}\", {subclause}");
        }
    }
}