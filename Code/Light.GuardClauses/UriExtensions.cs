using System;
using System.Collections.Generic;
using System.Text;
using Light.GuardClauses.FrameworkExtensions;

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
        public static Uri MustBeAbsoluteUri(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);

            if (uri.IsAbsoluteUri)
                return uri;

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
        ///     The exception that will be thrown when <paramref name="uri" /> is not an absolute URI and does not have the specified scheme.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI or does not have the specified scheme.</exception>
        public static Uri MustHaveScheme(this Uri uri, string scheme, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);

            if (uri.IsAbsoluteUri && string.Equals(uri.Scheme, scheme, StringComparison.OrdinalIgnoreCase))
                return uri;

            var subclause = uri.IsAbsoluteUri ? $"but actually has scheme \"{uri.Scheme}\" (\"{uri}\")." : $"but it has none because it is a relative URI (\"{uri}\").";
            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The URI"} must have scheme \"{scheme}\", {subclause}");
        }

        /// <summary>
        ///     Ensures that the specified URI has one of the given schemes, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="schemes"></param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="uri" /> is not an absolute URI and does not have one of the specified schemes.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI or does not have one of the specified schemes.</exception>
        public static Uri MustHaveOneSchemeOf(this Uri uri, IEnumerable<string> schemes, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);
            var schemesCollection = schemes.MustNotBeNullOrEmpty(nameof(schemes), "Your precondition is set up wrongly: schemes is null or an empty collection.").AsReadOnlyList();

            if (uri.IsAbsoluteUri == false)
                goto ThrowException;

            for (var i = 0; i < schemesCollection.Count; i++)
            {
                if (string.Equals(schemesCollection[i], uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    return uri;
            }

            ThrowException:
            var subclause = uri.IsAbsoluteUri ? $"but actually has scheme \"{uri.Scheme}\" (\"{uri}\")." : $"but it has none because it is a relative URI (\"{uri}\").";
            throw exception != null
                      ? exception()
                      : new ArgumentException(message ?? new StringBuilder().Append($"{parameterName ?? "The URI"} must have one of the following schemes:")
                                                                            .AppendItemsWithNewLine(schemesCollection)
                                                                            .Append(subclause)
                                                                            .ToString());
        }

        /// <summary>
        ///     Ensures that the specified URI has the "https" scheme, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="uri" /> is not an absolute URI and does not have the "https" scheme.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI or does not have the "https" scheme.</exception>
        public static Uri MustBeHttpsUrl(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            return uri.MustHaveScheme("https", parameterName, message, exception);
        }

        /// <summary>
        ///     Ensures that the specified URI has the "http" scheme, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="uri" /> is not an absolute URI and does not have the "http" scheme.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI or does not have the "http" scheme.</exception>
        public static Uri MustBeHttpUrl(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            return uri.MustHaveScheme("http", parameterName, message, exception);
        }
    }
}