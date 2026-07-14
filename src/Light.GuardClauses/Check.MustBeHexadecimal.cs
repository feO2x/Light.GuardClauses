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
    /// Ensures that the string contains only ASCII hexadecimal characters. Empty strings are valid.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    /// <exception cref="StringException">Thrown when the string contains a non-hexadecimal character.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeHexadecimal(
        [NotNull][ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        parameter.MustNotBeNull(parameterName, message);
        if (!parameter.IsHexadecimal())
        {
            Throw.InvalidStringContent(parameterName, message, "must contain only ASCII hexadecimal characters");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string contains only ASCII hexadecimal characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static string MustBeHexadecimal(
        [NotNull][ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter is null || !parameter.IsHexadecimal())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span contains only ASCII hexadecimal characters. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeHexadecimal(
        this Span<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter).MustBeHexadecimal(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span contains only ASCII hexadecimal characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeHexadecimal(this Span<char> parameter, ReadOnlySpanExceptionFactory<char> exceptionFactory)
    {
        ((ReadOnlySpan<char>) parameter).MustBeHexadecimal(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only span contains only ASCII hexadecimal characters. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only character span.</returns>
    public static ReadOnlySpan<char> MustBeHexadecimal(
        this ReadOnlySpan<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsHexadecimal())
        {
            Throw.InvalidStringContent(parameterName, message, "must contain only ASCII hexadecimal characters");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only span contains only ASCII hexadecimal characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only character span.</returns>
    public static ReadOnlySpan<char> MustBeHexadecimal(
        this ReadOnlySpan<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        if (!parameter.IsHexadecimal())
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the memory contains only ASCII hexadecimal characters. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeHexadecimal(
        this Memory<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeHexadecimal(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory contains only ASCII hexadecimal characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeHexadecimal(this Memory<char> parameter, ReadOnlySpanExceptionFactory<char> exceptionFactory)
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeHexadecimal(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only memory contains only ASCII hexadecimal characters. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeHexadecimal(
        this ReadOnlyMemory<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustBeHexadecimal(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only memory contains only ASCII hexadecimal characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeHexadecimal(
        this ReadOnlyMemory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        parameter.Span.MustBeHexadecimal(exceptionFactory);
        return parameter;
    }
}
