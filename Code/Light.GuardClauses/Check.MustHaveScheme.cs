using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="scheme">The scheme that the URI should have.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than the specified one.</exception>
    /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustHaveScheme(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        string scheme,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (string.Equals(parameter.MustBeAbsoluteUri(parameterName, message).Scheme, scheme) == false)
        {
            Throw.UriMustHaveScheme(parameter, scheme, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="scheme">The scheme that the URI should have.</param>
    /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one, or when <paramref name="parameter" /> is a relative URI, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustHaveScheme(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        string scheme,
        Func<Uri?, Exception> exceptionFactory
    )
    {
        if (string.Equals(parameter.MustBeAbsoluteUri(exceptionFactory).Scheme, scheme) == false)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the <paramref name="parameter" /> has the specified scheme, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="scheme">The scheme that the URI should have.</param>
    /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter" /> and <paramref name="scheme" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than the specified one, or when <paramref name="parameter" /> is a relative URI, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustHaveScheme(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        string scheme,
        Func<Uri?, string, Exception> exceptionFactory
    )
    {
        if (parameter is null || !parameter.IsAbsoluteUri || parameter.Scheme.Equals(scheme) == false)
        {
            Throw.CustomException(exceptionFactory, parameter, scheme);
        }

        return parameter;
    }
}
