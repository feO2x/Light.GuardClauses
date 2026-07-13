using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified GUID is not empty, or otherwise throws an <see cref="EmptyGuidException" />.
    /// </summary>
    /// <param name="parameter">The GUID to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="EmptyGuidException">Thrown when <paramref name="parameter" /> is an empty GUID.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid MustNotBeEmpty(
        this Guid parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter == Guid.Empty)
        {
            Throw.EmptyGuid(parameterName, message);
        }
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified GUID is not empty, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The GUID to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is an empty GUID.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static Guid MustNotBeEmpty(this Guid parameter, Func<Exception> exceptionFactory)
    {
        if (parameter == Guid.Empty)
        {
            Throw.CustomException(exceptionFactory);
        }
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified span is not empty, or otherwise throws an <see cref="EmptyCollectionException" />.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustNotBeEmpty<T>(
        this Span<T> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<T>) parameter).MustNotBeEmpty(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified span is not empty, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> MustNotBeEmpty<T>(
        this Span<T> parameter,
        ReadOnlySpanExceptionFactory<T> exceptionFactory
    )
    {
        ((ReadOnlySpan<T>) parameter).MustNotBeEmpty(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified read-only span is not empty, or otherwise throws an <see cref="EmptyCollectionException" />.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustNotBeEmpty<T>(
        this ReadOnlySpan<T> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.IsEmpty)
        {
            Throw.EmptyCollection(parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified read-only span is not empty, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> MustNotBeEmpty<T>(
        this ReadOnlySpan<T> parameter,
        ReadOnlySpanExceptionFactory<T> exceptionFactory
    )
    {
        if (parameter.IsEmpty)
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified memory is not empty, or otherwise throws an <see cref="EmptyCollectionException" />.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<T> MustNotBeEmpty<T>(
        this Memory<T> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<T>) parameter.Span).MustNotBeEmpty(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified memory is not empty, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<T> MustNotBeEmpty<T>(
        this Memory<T> parameter,
        ReadOnlySpanExceptionFactory<T> exceptionFactory
    )
    {
        ((ReadOnlySpan<T>) parameter.Span).MustNotBeEmpty(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified read-only memory is not empty, or otherwise throws an <see cref="EmptyCollectionException" />.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<T> MustNotBeEmpty<T>(
        this ReadOnlyMemory<T> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustNotBeEmpty(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified read-only memory is not empty, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<T> MustNotBeEmpty<T>(
        this ReadOnlyMemory<T> parameter,
        ReadOnlySpanExceptionFactory<T> exceptionFactory
    )
    {
        parameter.Span.MustNotBeEmpty(exceptionFactory);
        return parameter;
    }
}
