using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified URI is a relative one, or otherwise throws an <see cref="AbsoluteUriException" />.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="AbsoluteUriException">Thrown when <paramref name="parameter" /> is an absolute URI.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustBeRelativeUri(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).IsAbsoluteUri)
        {
            Throw.MustBeRelativeUri(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified URI is a relative one, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is an absolute URI, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustBeRelativeUri(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        Func<Uri?, Exception> exceptionFactory
    )
    {
        if (parameter is null || parameter.IsAbsoluteUri)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
