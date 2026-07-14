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
    /// Ensures that the string contains no Unicode whitespace character. Empty strings are valid.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    /// <exception cref="StringException">Thrown when the string contains a whitespace character.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustNotContainWhiteSpace(
        [NotNull] [ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        parameter.MustNotBeNull(parameterName, message);
        parameter.AsSpan().MustNotContainWhiteSpace(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string contains no Unicode whitespace character, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static string MustNotContainWhiteSpace(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter is null)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        if (ContainsWhiteSpace(parameter.AsSpan()))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span contains no Unicode whitespace character. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustNotContainWhiteSpace(
        this Span<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter).MustNotContainWhiteSpace(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span contains no Unicode whitespace character, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustNotContainWhiteSpace(
        this Span<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter).MustNotContainWhiteSpace(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only span contains no Unicode whitespace character. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only character span.</returns>
    public static ReadOnlySpan<char> MustNotContainWhiteSpace(
        this ReadOnlySpan<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        if (ContainsWhiteSpace(parameter))
        {
            Throw.InvalidStringContent(parameterName, message, "must contain no Unicode whitespace characters");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only span contains no Unicode whitespace character, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only character span.</returns>
    public static ReadOnlySpan<char> MustNotContainWhiteSpace(
        this ReadOnlySpan<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        if (ContainsWhiteSpace(parameter))
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the memory contains no Unicode whitespace character. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustNotContainWhiteSpace(
        this Memory<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustNotContainWhiteSpace(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory contains no Unicode whitespace character, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustNotContainWhiteSpace(
        this Memory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustNotContainWhiteSpace(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only memory contains no Unicode whitespace character. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustNotContainWhiteSpace(
        this ReadOnlyMemory<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustNotContainWhiteSpace(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only memory contains no Unicode whitespace character, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustNotContainWhiteSpace(
        this ReadOnlyMemory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        parameter.Span.MustNotContainWhiteSpace(exceptionFactory);
        return parameter;
    }

    private static bool ContainsWhiteSpace(ReadOnlySpan<char> parameter)
    {
        foreach (var character in parameter)
        {
            if (char.IsWhiteSpace(character))
            {
                return true;
            }
        }

        return false;
    }
}
