using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the URI has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException" />.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="schemes">One of these strings must be equal to the scheme of the URI.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidUriSchemeException">Thrown when the scheme <paramref name="parameter" /> is not equal to one of the specified schemes.</exception>
    /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter" /> is relative and thus has no scheme.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="schemes" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; schemes:null => halt")]
    public static Uri MustHaveOneSchemeOf(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        IEnumerable<string> schemes,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        // ReSharper disable PossibleMultipleEnumeration
        parameter.MustBeAbsoluteUri(parameterName, message);

        if (schemes is ICollection<string> collection)
        {
            if (!collection.Contains(parameter.Scheme))
            {
                Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            }

            return parameter;
        }

        if (!schemes.MustNotBeNull(nameof(schemes), message).Contains(parameter.Scheme))
        {
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
        }

        return parameter;
        // ReSharper restore PossibleMultipleEnumeration
    }

    /// <summary>
    /// Ensures that the URI has one of the specified schemes, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The URI to be checked.</param>
    /// <param name="schemes">One of these strings must be equal to the scheme of the URI.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="schemes" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the scheme <paramref name="parameter" /> is not equal to one of the specified schemes, or when <paramref name="parameter" /> is a relative URI, or when <paramref name="parameter" /> is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="schemes" /> is null.</exception>
    /// <typeparam name="TCollection">The type of the collection containing the schemes.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Uri MustHaveOneSchemeOf<TCollection>(
        [NotNull] [ValidatedNotNull] this Uri? parameter,
        TCollection schemes,
        Func<Uri?, TCollection, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable<string>
    {
        if (parameter is null || !parameter.IsAbsoluteUri)
        {
            Throw.CustomException(exceptionFactory, parameter, schemes);
        }

        if (schemes is ICollection<string> collection)
        {
            if (!collection.Contains(parameter.Scheme))
            {
                Throw.CustomException(exceptionFactory, parameter, schemes);
            }

            return parameter;
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (schemes is null || !schemes.Contains(parameter.Scheme))
        {
            Throw.CustomException(exceptionFactory, parameter, schemes!);
        }

        return parameter;
    }
}
