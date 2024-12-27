using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the span is shorter than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeShorterThan<T>(this Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.Length >= length)
            Throw.SpanMustBeShorterThan(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span is shorter than the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeShorterThan<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
    {
        if (parameter.Length >= length)
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span is shorter than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeShorterThan<T>(this ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.Length >= length)
            Throw.SpanMustBeShorterThan(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span is shorter than the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeShorterThan<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
    {
        if (parameter.Length >= length)
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than or equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeShorterThanOrEqualTo<T>(this Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.Length > length)
            Throw.SpanMustBeShorterThanOrEqualTo(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than or equal to.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustBeShorterThanOrEqualTo<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
    {
        if (parameter.Length > length)
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than or equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeShorterThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.Length > length)
            Throw.SpanMustBeShorterThanOrEqualTo(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span to be checked.</param>
    /// <param name="length">The length value that the span must be shorter than or equal to.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustBeShorterThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
    {
        if (parameter.Length > length)
            Throw.CustomSpanException(exceptionFactory, parameter, length);
        return parameter;
    }
}