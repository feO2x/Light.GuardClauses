using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif

namespace Light.GuardClauses
{
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws an <see cref="RelativeUriException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="RelativeUriException" /> (optional).</param>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
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
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter. This is used for the <see cref="ArgumentNullException" /> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeAbsoluteUri(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbsoluteUri == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI is a relative one, or otherwise throws a <see cref="AbsoluteUriException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="AbsoluteUriException" /> (optional).</param>
        /// <exception cref="AbsoluteUriException">Thrown when <paramref name="parameter"/> is an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeRelativeUri(this Uri parameter, string parameterName = null, string message = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbsoluteUri)
                Throw.MustBeRelativeUri(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI is a relative one, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is an absolute URI.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeRelativeUri(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbsoluteUri)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="scheme">The scheme that the URI should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidUriSchemeException" /> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
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
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter. This is used for the <see cref="ArgumentNullException" /> and <see cref="RelativeUriException" /> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
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
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> and <paramref name="scheme"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter. This is used for the <see cref="ArgumentNullException" /> and <see cref="RelativeUriException" /> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveScheme(this Uri parameter, string scheme, Func<Uri, string, Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustBeAbsoluteUri(parameterName).Scheme.Equals(scheme) == false)
                Throw.CustomException(exceptionFactory, parameter, scheme);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidUriSchemeException" /> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpsUrl(this Uri parameter, string parameterName = null, string message = null) => parameter.MustHaveScheme("https", parameterName, message);

        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="RelativeUriException"/> and <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpsUrl(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null) => parameter.MustHaveScheme("https", exceptionFactory, parameterName);

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidUriSchemeException" /> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "http".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpUrl(this Uri parameter, string parameterName = null, string message = null) => parameter.MustHaveScheme("http", parameterName, message);

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="RelativeUriException"/> and <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpUrl(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null) => parameter.MustHaveScheme("http", exceptionFactory, parameterName);

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidUriSchemeException" /> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "http" or "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpOrHttpsUrl(this Uri parameter, string parameterName = null, string message = null)
        {
            if (parameter.MustBeAbsoluteUri(parameterName).Scheme.Equals("https") == false && parameter.Scheme.Equals("http") == false)
                Throw.UriMustHaveOneSchemeOf(parameter, new[] { "https", "http" }, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws your custom exception />.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="RelativeUriException"/> and <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http" or "https".</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpOrHttpsUrl(this Uri parameter, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustBeAbsoluteUri(parameterName).Scheme.Equals("https") == false && parameter.Scheme.Equals("http") == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Ensures that the URI has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these strings must be equal to the scheme of the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidUriSchemeException" /> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when the scheme <paramref name="parameter"/> is not equal to one of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveOneSchemeOf<TCollection>(this Uri parameter, TCollection schemes, string parameterName = null, string message = null) where TCollection : class, IEnumerable<string>
        {
            parameter.MustBeAbsoluteUri(parameterName);

            if (schemes is ICollection<string> collection)
            {
                if (!collection.Contains(parameter.Scheme))
                    Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
                return parameter;
            }

            if (!schemes.MustNotBeNull(nameof(schemes)).Contains(parameter.Scheme))
                Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the URI has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these strings must be equal to the scheme of the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/></param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="RelativeUriException"/> and <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the scheme <paramref name="parameter"/> is not equal to one of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveOneSchemeOf<TCollection>(this Uri parameter, TCollection schemes, Func<Uri, TCollection, Exception> exceptionFactory, string parameterName = null) where TCollection : class, IEnumerable<string>
        {
            parameter.MustBeAbsoluteUri(parameterName);

            if (schemes is ICollection<string> collection)
            {
                if (!collection.Contains(parameter.Scheme))
                    Throw.CustomException(exceptionFactory, parameter, schemes);
                return parameter;
            }

            if (!schemes.MustNotBeNull(nameof(schemes)).Contains(parameter.Scheme))
                Throw.CustomException(exceptionFactory, parameter, schemes);
            return parameter;
        }
    }
}
