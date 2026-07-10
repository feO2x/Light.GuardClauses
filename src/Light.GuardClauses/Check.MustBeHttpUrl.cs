using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified URI has the "http" scheme, or otherwise throws an <see cref="InvalidUriSchemeException" />.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter" /> uses a different scheme than "http".</exception>
    /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustBeHttpUrl(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        parameter.MustHaveScheme("http", parameterName, message);

    /// <summary>
    /// Ensures that the specified URI has the "http" scheme, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> uses a different scheme than "http", or when <paramref name="parameter" /> is a relative URI, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustBeHttpUrl(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        Func<Uri?, Exception> exceptionFactory
    ) =>
        parameter.MustHaveScheme("http", exceptionFactory);
}
