using System;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the collection is not null or empty, or otherwise throws an <see cref="EmptyCollectionException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="EmptyCollectionException">Thrown when <paramref name="parameter" /> has no items.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustNotBeNullOrEmpty<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TCollection : class, IEnumerable
    {
        if (parameter.Count(parameterName, message) == 0)
        {
            Throw.EmptyCollection(parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the collection is not null or empty, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
    /// <exception cref="Exception">Thrown when <paramref name="parameter" /> has no items, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustNotBeNullOrEmpty<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        Func<TCollection?, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable
    {
        if (parameter is null || parameter.Count() == 0)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified string is not null or empty, or otherwise throws an <see cref="ArgumentNullException" /> or <see cref="EmptyStringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter" /> is an empty string.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustNotBeNullOrEmpty(
        [NotNull] [ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter is null)
        {
            Throw.ArgumentNull(parameterName, message);
        }

        if (parameter.Length == 0)
        {
            Throw.EmptyString(parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified string is not null or empty, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is an empty string or null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static string MustNotBeNullOrEmpty(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter.IsNullOrEmpty())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
