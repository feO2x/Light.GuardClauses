using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the span is longer than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The value that the span must be longer than.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> is shorter than or equal to <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeLongerThan<T>(
        this Span<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.Length <= length)
        {
            Throw.SpanMustBeLongerThan(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span is longer than the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be longer than.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is shorter than or equal to <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeLongerThan<T>(
        this Span<T> parameter,
        int length,
        SpanExceptionFactory<T, int> exceptionFactory
    )
    {
        if (parameter.Length <= length)
        {
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span is longer than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The value that the span must be longer than.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> is shorter than or equal to <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeLongerThan<T>(
        this ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.Length <= length)
        {
            Throw.SpanMustBeLongerThan(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span is longer than the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be longer than.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is shorter than or equal to <paramref name="length" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeLongerThan<T>(
        this ReadOnlySpan<T> parameter,
        int length,
        ReadOnlySpanExceptionFactory<T, int> exceptionFactory
    )
    {
        if (parameter.Length <= length)
        {
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        }

        return parameter;
    }
}
