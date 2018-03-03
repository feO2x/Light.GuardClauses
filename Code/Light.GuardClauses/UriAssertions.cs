using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Provides assertion methods for the <see cref="Uri" /> class.
    /// </summary>
    public static partial class UriAssertions
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
        /// <param name="parameterName">The parameter name (used for the <see cref="ArgumentNullException" />, optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
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
        /// <param name="parameterName">The parameter name (used for the <see cref="ArgumentNullException" />, optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeAbsoluteUri(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbsoluteUri == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveScheme(this Uri parameter, string scheme, string parameterName = null, string message = null)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(parameterName).Scheme, scheme, StringComparison.OrdinalIgnoreCase) == false)
                Throw.UriMustHaveScheme(parameter, scheme, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (used for the <see cref="ArgumentNullException" /> and <see cref="RelativeUriException" />, optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveScheme(this Uri parameter, string scheme, Func<Exception> exceptionFactory, string parameterName = null)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(parameterName).Scheme, scheme, StringComparison.OrdinalIgnoreCase) == false)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (used for the <see cref="ArgumentNullException" /> and <see cref="RelativeUriException" />, optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveScheme(this Uri parameter, string scheme, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(parameterName).Scheme, scheme, StringComparison.OrdinalIgnoreCase) == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (used for the <see cref="ArgumentNullException" /> and <see cref="RelativeUriException" />, optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveScheme(this Uri parameter, string scheme, Func<Uri, string, Exception> exceptionFactory, string parameterName = null)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(parameterName).Scheme, scheme, StringComparison.OrdinalIgnoreCase) == false)
                Throw.CustomException(exceptionFactory, parameter, scheme);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpsUrl(this Uri parameter, string parameterName = null, string message = null) => parameter.MustHaveScheme("https", parameterName, message);

        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpsUrl(this Uri parameter, Func<Exception> exceptionFactory, string parameterName = null) => parameter.MustHaveScheme("https", exceptionFactory, parameterName);

        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpsUrl(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null) => parameter.MustHaveScheme("https", exceptionFactory, parameterName);

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "http".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpUrl(this Uri parameter, string parameterName = null, string message = null) => parameter.MustHaveScheme("http", parameterName, message);

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws your custom exception />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpUrl(this Uri parameter, Func<Exception> exceptionFactory, string parameterName = null) => parameter.MustHaveScheme("http", exceptionFactory, parameterName);

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws your custom exception />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpUrl(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null) => parameter.MustHaveScheme("http", exceptionFactory, parameterName);

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "http" or "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpOrHttpsUrl(this Uri parameter, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);

            if (parameter.Scheme.Equals("https") == false && parameter.Scheme.Equals("http") == false)
                Throw.UriMustHaveOneSchemeOf(parameter, new []{"https", "http"}, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws your custom exception />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http" or "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpOrHttpsUrl(this Uri parameter, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);

            if (parameter.Scheme.Equals("https") == false && parameter.Scheme.Equals("http") == false)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws your custom exception />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http" or "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustBeHttpOrHttpsUrl(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);

            if (parameter.Scheme.Equals("https") == false && parameter.Scheme.Equals("http") == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }
    }
}