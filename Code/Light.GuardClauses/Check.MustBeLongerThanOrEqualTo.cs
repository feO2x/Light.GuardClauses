using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string is longer than or equal to the specified length, or otherwise throws a <see cref="StringLengthException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be longer than or equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when <paramref name="parameter" /> has a length shorter than <paramref name="length" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeLongerThanOrEqualTo(
        [NotNull] [ValidatedNotNull] this string? parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).Length < length)
        {
            Throw.StringNotLongerThanOrEqualTo(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is longer than or equal to the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be longer than or equal to.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or when it has a length shorter than <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeLongerThanOrEqualTo(
        [NotNull] [ValidatedNotNull] this string? parameter,
        int length,
        Func<string?, int, Exception> exceptionFactory
    )
    {
        if (parameter is null || parameter.Length < length)
        {
            Throw.CustomException(exceptionFactory, parameter, length);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span is longer than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The value that the span must be longer than or equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> is shorter than <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeLongerThanOrEqualTo<T>(
        this Span<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.Length < length)
        {
            Throw.SpanMustBeLongerThanOrEqualTo(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span is longer than or equal to the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The value that the span must be longer than or equal to.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is shorter than <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeLongerThanOrEqualTo<T>(
        this Span<T> parameter,
        int length,
        SpanExceptionFactory<T, int> exceptionFactory
    )
    {
        if (parameter.Length < length)
        {
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span is longer than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The value that the span must be longer than or equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> is shorter than <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeLongerThanOrEqualTo<T>(
        this ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.Length < length)
        {
            Throw.SpanMustBeLongerThanOrEqualTo(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span is longer than or equal to the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The value that the span must be longer than or equal to.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is shorter than <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeLongerThanOrEqualTo<T>(
        this ReadOnlySpan<T> parameter,
        int length,
        ReadOnlySpanExceptionFactory<T, int> exceptionFactory
    )
    {
        if (parameter.Length < length)
        {
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        }

        return parameter;
    }
}
