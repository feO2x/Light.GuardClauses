using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string is a valid file extension, or otherwise throws an <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeFileExtension(
        [NotNull][ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsFileExtension())
        {
            Throw.NotFileExtension(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is a valid file extension, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or not a valid file extension.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeFileExtension(
        [NotNull][ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter is null || !parameter.IsFileExtension())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the character span is a valid file extension, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original character span.</returns>
    /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeFileExtension(
        this Span<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>)parameter).MustBeFileExtension(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the character span is a valid file extension, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <returns>The original character span.</returns>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    public static Span<char> MustBeFileExtension(
        this Span<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>)parameter).MustBeFileExtension(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the character memory is a valid file extension, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original character memory.</returns>
    /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeFileExtension(
        this Memory<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>)parameter.Span).MustBeFileExtension(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the character memory is a valid file extension, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <returns>The original character memory.</returns>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    public static Memory<char> MustBeFileExtension(
        this Memory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>)parameter.Span).MustBeFileExtension(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character memory is a valid file extension, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original read-only character memory.</returns>
    /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeFileExtension(
        this ReadOnlyMemory<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustBeFileExtension(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character memory is a valid file extension, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <returns>The original read-only character memory.</returns>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    public static ReadOnlyMemory<char> MustBeFileExtension(
        this ReadOnlyMemory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        parameter.Span.MustBeFileExtension(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character span is a valid file extension, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original read-only character span.</returns>
    /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustBeFileExtension(
        this ReadOnlySpan<char> parameter,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsFileExtension())
        {
            Throw.NotFileExtension(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character span is a valid file extension, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <returns>The original read-only character span.</returns>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not a valid file extension.</exception>
    public static ReadOnlySpan<char> MustBeFileExtension(
        this ReadOnlySpan<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        if (!parameter.IsFileExtension())
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
