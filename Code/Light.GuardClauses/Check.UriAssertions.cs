using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses
{
    public static partial class Check
    {
        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws a <see cref="RelativeUriException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeAbsoluteUri([ValidatedNotNull] this Uri? parameter, string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IsAbsoluteUri == false)
                Throw.MustBeAbsoluteUri(parameter!, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not an absolute URI, or when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeAbsoluteUri([ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory)
        {
            if (parameter == null || parameter.IsAbsoluteUri == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the specified URI is a relative one, or otherwise throws an <see cref="AbsoluteUriException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="AbsoluteUriException">Thrown when <paramref name="parameter"/> is an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeRelativeUri([ValidatedNotNull] this Uri? parameter, string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IsAbsoluteUri)
                Throw.MustBeRelativeUri(parameter!, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the specified URI is a relative one, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is an absolute URI, or when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeRelativeUri([ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory)
        {
            if (parameter == null || parameter.IsAbsoluteUri)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveScheme([ValidatedNotNull] this Uri? parameter, string scheme, string? parameterName = null, string? message = null)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(parameterName, message).Scheme, scheme) == false)
                Throw.UriMustHaveScheme(parameter!, scheme, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one,
        /// or when <paramref name="parameter"/> is a relative URI,
        /// or when <paramref name="parameter"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveScheme([ValidatedNotNull] this Uri? parameter, string scheme, Func<Uri?, Exception> exceptionFactory)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(exceptionFactory).Scheme, scheme) == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> and <paramref name="scheme"/> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one,
        /// or when <paramref name="parameter"/> is a relative URI,
        /// or when <paramref name="parameter"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveScheme([ValidatedNotNull] this Uri? parameter, string scheme, Func<Uri?, string, Exception> exceptionFactory)
        {
            if (parameter == null || !parameter.IsAbsoluteUri || parameter.Scheme.Equals(scheme) == false)
                Throw.CustomException(exceptionFactory, parameter, scheme);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpsUrl([ValidatedNotNull] this Uri? parameter, string? parameterName = null, string? message = null) => parameter.MustHaveScheme("https", parameterName, message);

        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "https",
        /// or when <paramref name="parameter"/> is a relative URI,
        /// or when <paramref name="parameter"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpsUrl([ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory) => parameter.MustHaveScheme("https", exceptionFactory);

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "http".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpUrl([ValidatedNotNull] this Uri? parameter, string? parameterName = null, string? message = null) => parameter.MustHaveScheme("http", parameterName, message);

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http",
        /// or when <paramref name="parameter"/> is a relative URI,
        /// or when <paramref name="parameter"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpUrl([ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory) => parameter.MustHaveScheme("http", exceptionFactory);

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "http" or "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpOrHttpsUrl([ValidatedNotNull] this Uri? parameter, string? parameterName = null, string? message = null)
        {
            if (parameter.MustBeAbsoluteUri(parameterName, message).Scheme.Equals("https") == false && parameter!.Scheme.Equals("http") == false)
                Throw.UriMustHaveOneSchemeOf(parameter, new[] { "https", "http" }, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http" or "https",
        /// or when <paramref name="parameter"/> is a relative URI,
        /// or when <paramref name="parameter"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpOrHttpsUrl([ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory)
        {
            if (parameter.MustBeAbsoluteUri(exceptionFactory).Scheme.Equals("https") == false && parameter!.Scheme.Equals("http") == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the URI has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these strings must be equal to the scheme of the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when the scheme <paramref name="parameter"/> is not equal to one of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> or <paramref name="schemes"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; schemes:null => halt")]
        public static Uri MustHaveOneSchemeOf([ValidatedNotNull] this Uri? parameter, IEnumerable<string> schemes, string? parameterName = null, string? message = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustBeAbsoluteUri(parameterName, message);

            if (schemes is ICollection<string> collection)
            {
                if (!collection.Contains(parameter!.Scheme))
                    Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
                return parameter;
            }

            if (!schemes.MustNotBeNull(nameof(schemes), message).Contains(parameter!.Scheme))
                Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return parameter;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Ensures that the URI has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these strings must be equal to the scheme of the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/></param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when the scheme <paramref name="parameter"/> is not equal to one of the specified schemes,
        /// or when <paramref name="parameter"/> is a relative URI,
        /// or when <paramref name="parameter"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="schemes" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveOneSchemeOf<TCollection>([ValidatedNotNull] this Uri? parameter, TCollection schemes, Func<Uri?, TCollection, Exception> exceptionFactory) where TCollection : class, IEnumerable<string>
        {
            if (parameter is null || !parameter.IsAbsoluteUri)
                Throw.CustomException(exceptionFactory, parameter, schemes);

            if (schemes is ICollection<string> collection)
            {
                if (!collection.Contains(parameter!.Scheme))
                    Throw.CustomException(exceptionFactory, parameter, schemes);
                return parameter;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
            if (schemes is null || !schemes.Contains(parameter!.Scheme))
                Throw.CustomException(exceptionFactory, parameter, schemes!);
            return parameter!;
        }
    }
}
