using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string has the specified length, or otherwise throws a <see cref="StringLengthException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The asserted length of the string.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when <paramref name="parameter" /> has a length other than <paramref name="length" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLength(
        [NotNull] [ValidatedNotNull] this string? parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).Length != length)
        {
            Throw.StringLengthNotEqualTo(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string has the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The asserted length of the string.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or when it has a length other than <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLength(
        [NotNull] [ValidatedNotNull] this string? parameter,
        int length,
        Func<string?, int, Exception> exceptionFactory
    )
    {
        if (parameter is null || parameter.Length != length)
        {
            Throw.CustomException(exceptionFactory, parameter, length);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span has the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length that the span must have.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> does not have the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustHaveLength<T>(
        this Span<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<T>) parameter).MustHaveLength(length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span has the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length that the span must have.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not have the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustHaveLength<T>(
        this Span<T> parameter,
        int length,
        SpanExceptionFactory<T, int> exceptionFactory
    )
    {
        if (parameter.Length != length)
        {
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span has the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length that the span must have.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not have the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustHaveLength<T>(
        this ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.Length != length)
        {
            Throw.InvalidSpanLength(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span has the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length that the span must have.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not have the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustHaveLength<T>(
        this ReadOnlySpan<T> parameter,
        int length,
        ReadOnlySpanExceptionFactory<T, int> exceptionFactory
    )
    {
        if (parameter.Length != length)
        {
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the immutable array has the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The immutable array to be checked.</param>
    /// <param name="length">The length that the immutable array must have.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> does not have the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustHaveLength<T>(
        this ImmutableArray<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        var actualLength = parameter.IsDefault ? 0 : parameter.Length;
        if (actualLength != length)
        {
            Throw.InvalidImmutableArrayLength(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the immutable array has the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The immutable array to be checked.</param>
    /// <param name="length">The length that the immutable array must have.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not have the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustHaveLength<T>(
        this ImmutableArray<T> parameter,
        int length,
        Func<ImmutableArray<T>, int, Exception> exceptionFactory
    )
    {
        var actualLength = parameter.IsDefault ? 0 : parameter.Length;
        if (actualLength != length)
        {
            Throw.CustomException(exceptionFactory, parameter, length);
        }

        return parameter;
    }
}
