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
    /// Ensures that the specified string is a valid URI of the supplied kind, or otherwise throws an
    /// <see cref="InvalidUriException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="uriKind">The kind of URI that the string must represent.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidUriException">
    /// Thrown when <paramref name="parameter" /> is not a valid URI of the supplied kind.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeUri(
        [NotNull] [ValidatedNotNull] this string? parameter,
        UriKind uriKind = UriKind.RelativeOrAbsolute,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.MustNotBeNull(parameterName, message);

        if (!Uri.TryCreate(parameter, uriKind, out _))
        {
            Throw.MustBeUri(parameter, uriKind, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified string is a valid URI of the supplied kind, or otherwise throws your custom
    /// exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="uriKind">The kind of URI that the string must represent.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates the exception to be thrown. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is null or not a valid URI of the supplied kind.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeUri(
        [NotNull] [ValidatedNotNull] this string? parameter,
        UriKind uriKind,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter is null || !Uri.TryCreate(parameter, uriKind, out _))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified string is a valid URI of the supplied kind, or otherwise throws your custom
    /// exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="uriKind">The kind of URI that the string must represent.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates the exception to be thrown. <paramref name="parameter" /> and
    /// <paramref name="uriKind" /> are passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is null or not a valid URI of the supplied kind.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeUri(
        [NotNull] [ValidatedNotNull] this string? parameter,
        UriKind uriKind,
        Func<string?, UriKind, Exception> exceptionFactory
    )
    {
        if (parameter is null || !Uri.TryCreate(parameter, uriKind, out _))
        {
            Throw.CustomException(exceptionFactory, parameter, uriKind);
        }

        return parameter;
    }
}
