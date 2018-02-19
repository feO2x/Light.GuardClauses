using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Provides assertion methods for the <see cref="Uri" /> class.
    /// </summary>
    public static class UriAssertions
    {
        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws an <see cref="RelativeUriException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="RelativeUriException" /> (optional).</param>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeAbsoluteUri(this Uri parameter, string parameterName = null, string message = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbsoluteUri == false)
                Throw.MustBeAbsoluteUri(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The parameter name (used for the <see cref="ArgumentNullException" />.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeAbsoluteUri(this Uri parameter, Func<Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbsoluteUri == false)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The parameter name (used for the <see cref="ArgumentNullException" />.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeAbsoluteUri(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbsoluteUri == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the given scheme, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        /// The exception that will be thrown when <paramref name="uri" /> is not an absolute URI or does not have the specified scheme.
        /// Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
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
        /// Ensures that the specified URI has one of the given schemes, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="schemes"></param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        /// The exception that will be thrown when <paramref name="uri" /> is not an absolute URI or does not have one of the specified schemes.
        /// Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
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
                if (string.Equals(schemesCollection[i], uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    return uri;

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
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        /// The exception that will be thrown when <paramref name="uri" /> is not an absolute URI or does not have the "https" scheme.
        /// Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI or does not have the "https" scheme.</exception>
        public static Uri MustBeHttpsUrl(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            return uri.MustHaveScheme("https", parameterName, message, exception);
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        /// The exception that will be thrown when <paramref name="uri" /> is not an absolute URI or does not have the "http" scheme.
        /// Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI or does not have the "http" scheme.</exception>
        public static Uri MustBeHttpUrl(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            return uri.MustHaveScheme("http", parameterName, message, exception);
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="uri">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="exception">
        /// The exception that will be thrown when <paramref name="uri" /> is not an absolute URI or does not have the "http" or "https" scheme.
        /// Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri" /> is not an absolute URI or does not have the "http" or "https" scheme.</exception>
        public static Uri MustBeHttpOrHttpsUrl(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            return uri.MustHaveOneSchemeOf(new[] { "http", "https" });
        }
    }
}