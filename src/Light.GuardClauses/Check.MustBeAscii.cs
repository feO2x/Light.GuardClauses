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
    /// Ensures that the character is ASCII, or otherwise throws an <see cref="ArgumentException" />.
    /// </summary>
    /// <param name="parameter">The character to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated character.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char MustBeAscii(
        this char parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsAscii())
        {
            Throw.Argument(parameterName, message ?? $"{parameterName ?? "The character"} must be ASCII, but it actually is '{parameter}'.");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the character is ASCII, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated character.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static char MustBeAscii(this char parameter, Func<char, Exception> exceptionFactory)
    {
        if (!parameter.IsAscii())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the byte is ASCII, or otherwise throws an <see cref="ArgumentException" />.
    /// </summary>
    /// <param name="parameter">The byte to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated byte.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte MustBeAscii(
        this byte parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsAscii())
        {
            Throw.Argument(parameterName, message ?? $"{parameterName ?? "The byte"} must be ASCII, but it actually is {parameter}.");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the byte is ASCII, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The byte to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated byte.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static byte MustBeAscii(this byte parameter, Func<byte, Exception> exceptionFactory)
    {
        if (!parameter.IsAscii())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is non-null and contains only ASCII characters, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeAscii(
        [NotNull] [ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.MustNotBeNull(parameterName, message);
        if (!parameter.IsAscii())
        {
            Throw.InvalidStringContent(parameterName, message, "must contain only ASCII characters");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is non-null and contains only ASCII characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static string MustBeAscii(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter is null || !parameter.IsAscii())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the character span contains only ASCII characters, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeAscii(
        this Span<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter).MustBeAscii(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the character span contains only ASCII characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeAscii(
        this Span<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter).MustBeAscii(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character span contains only ASCII characters, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustBeAscii(
        this ReadOnlySpan<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsAscii())
        {
            Throw.InvalidStringContent(parameterName, message, "must contain only ASCII characters");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character span contains only ASCII characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only character span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustBeAscii(
        this ReadOnlySpan<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        if (!parameter.IsAscii())
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the character memory contains only ASCII characters, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeAscii(
        this Memory<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeAscii(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the character memory contains only ASCII characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeAscii(
        this Memory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeAscii(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character memory contains only ASCII characters, or otherwise throws a <see cref="StringException" />.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeAscii(
        this ReadOnlyMemory<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustBeAscii(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only character memory contains only ASCII characters, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only character memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeAscii(
        this ReadOnlyMemory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        parameter.Span.MustBeAscii(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the byte span contains only ASCII values.
    /// </summary>
    /// <param name="parameter">The byte span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated byte span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> MustBeAscii(
        this Span<byte> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<byte>) parameter).MustBeAscii(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the byte span contains only ASCII values, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The byte span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated byte span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> MustBeAscii(
        this Span<byte> parameter,
        ReadOnlySpanExceptionFactory<byte> exceptionFactory
    )
    {
        ((ReadOnlySpan<byte>) parameter).MustBeAscii(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only byte span contains only ASCII values.
    /// </summary>
    /// <param name="parameter">The read-only byte span to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only byte span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> MustBeAscii(
        this ReadOnlySpan<byte> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsAscii())
        {
            Throw.Argument(parameterName, message ?? $"{parameterName ?? "The byte span"} must contain only ASCII values.");
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only byte span contains only ASCII values, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only byte span to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only byte span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> MustBeAscii(
        this ReadOnlySpan<byte> parameter,
        ReadOnlySpanExceptionFactory<byte> exceptionFactory
    )
    {
        if (!parameter.IsAscii())
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the byte memory contains only ASCII values.
    /// </summary>
    /// <param name="parameter">The byte memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated byte memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<byte> MustBeAscii(
        this Memory<byte> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<byte>) parameter.Span).MustBeAscii(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the byte memory contains only ASCII values, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The byte memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated byte memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<byte> MustBeAscii(
        this Memory<byte> parameter,
        ReadOnlySpanExceptionFactory<byte> exceptionFactory
    )
    {
        ((ReadOnlySpan<byte>) parameter.Span).MustBeAscii(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only byte memory contains only ASCII values.
    /// </summary>
    /// <param name="parameter">The read-only byte memory to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The validated read-only byte memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<byte> MustBeAscii(
        this ReadOnlyMemory<byte> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustBeAscii(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the read-only byte memory contains only ASCII values, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The read-only byte memory to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <returns>The validated read-only byte memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<byte> MustBeAscii(
        this ReadOnlyMemory<byte> parameter,
        ReadOnlySpanExceptionFactory<byte> exceptionFactory
    )
    {
        parameter.Span.MustBeAscii(exceptionFactory);
        return parameter;
    }
}
