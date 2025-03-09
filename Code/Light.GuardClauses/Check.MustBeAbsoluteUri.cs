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
    /// Ensures that the specified URI is an absolute one, or otherwise throws a <see cref="RelativeUriException" />.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is not an absolute URI.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustBeAbsoluteUri(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).IsAbsoluteUri == false)
        {
            Throw.MustBeAbsoluteUri(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified URI is an absolute one, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not an absolute URI, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustBeAbsoluteUri(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        Func<Uri?, Exception> exceptionFactory
    )
    {
        if (parameter is null || parameter.IsAbsoluteUri == false)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
