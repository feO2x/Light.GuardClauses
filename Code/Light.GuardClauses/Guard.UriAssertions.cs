using System;
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
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
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
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
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
            if (string.Equals(parameter.MustBeAbsoluteUri(parameterName).Scheme, scheme, StringComparison.OrdinalIgnoreCase) == false)
                Throw.CustomException(exceptionFactory, parameter, scheme);
            return parameter;
        }
    }
}
