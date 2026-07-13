using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the character span is neither empty nor all white space.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustNotBeEmptyOrWhiteSpace(
        this Span<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter).MustNotBeEmptyOrWhiteSpace(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the character span is neither empty nor all white space, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustNotBeEmptyOrWhiteSpace(
        this Span<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter).MustNotBeEmptyOrWhiteSpace(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character span is neither empty nor all white space.
    /// </summary>
    /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter" /> is empty.</exception>
    /// <exception cref="WhiteSpaceStringException">Thrown when <paramref name="parameter" /> contains only white space.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustNotBeEmptyOrWhiteSpace(
        this ReadOnlySpan<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.IsEmpty)
        {
            Throw.EmptyString(parameterName, message);
        }

        if (parameter.IsEmptyOrWhiteSpace())
        {
            Throw.WhiteSpaceSpan(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character span is neither empty nor all white space, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustNotBeEmptyOrWhiteSpace(
        this ReadOnlySpan<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        if (parameter.IsEmptyOrWhiteSpace())
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the character memory is neither empty nor all white space.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustNotBeEmptyOrWhiteSpace(
        this Memory<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustNotBeEmptyOrWhiteSpace(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the character memory is neither empty nor all white space, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustNotBeEmptyOrWhiteSpace(
        this Memory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustNotBeEmptyOrWhiteSpace(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character memory is neither empty nor all white space.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustNotBeEmptyOrWhiteSpace(
        this ReadOnlyMemory<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustNotBeEmptyOrWhiteSpace(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character memory is neither empty nor all white space, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustNotBeEmptyOrWhiteSpace(
        this ReadOnlyMemory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        parameter.Span.MustNotBeEmptyOrWhiteSpace(exceptionFactory);
        return parameter;
    }
}
