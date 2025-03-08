using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string is a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeEmailAddress(
        [NotNull] [ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsEmailAddress())
        {
            Throw.InvalidEmailAddress(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeEmailAddress(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (!parameter.IsEmailAddress())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is a valid email address using the provided regular expression,
    /// or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the input string is a valid email.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; emailAddressPattern:null => halt")]
    public static string MustBeEmailAddress(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Regex emailAddressPattern,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsEmailAddress(emailAddressPattern))
        {
            Throw.InvalidEmailAddress(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is a valid email address using the provided regular expression,
    /// or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the input string is a valid email.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="emailAddressPattern" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; emailAddressPattern:null => halt")]
    public static string MustBeEmailAddress(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Regex emailAddressPattern,
        Func<string?, Regex, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (emailAddressPattern is null || !parameter.IsEmailAddress(emailAddressPattern))
        {
            Throw.CustomException(exceptionFactory, parameter, emailAddressPattern!);
        }

        return parameter;
    }

#if NET8_0
    /// <summary>
    /// Ensures that the span represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustBeEmailAddress(
        this ReadOnlySpan<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsEmailAddress())
        {
            Throw.InvalidEmailAddress(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustBeEmailAddress(
        this ReadOnlySpan<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        if (!parameter.IsEmailAddress())
        {
            Throw.CustomSpanException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span represents a valid email address using the provided regular expression,
    /// or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the span represents a valid email.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("emailAddressPattern:null => halt")]
    public static ReadOnlySpan<char> MustBeEmailAddress(
        this ReadOnlySpan<char> parameter,
        Regex emailAddressPattern,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern));

        if (!parameter.IsEmailAddress(emailAddressPattern))
        {
            Throw.InvalidEmailAddress(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span represents a valid email address using the provided regular expression,
    /// or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the span represents a valid email.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> and <paramref name="emailAddressPattern" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address or when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> MustBeEmailAddress(
        this ReadOnlySpan<char> parameter,
        Regex emailAddressPattern,
        ReadOnlySpanExceptionFactory<char, Regex> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (emailAddressPattern is null || !parameter.IsEmailAddress(emailAddressPattern))
        {
            Throw.CustomSpanException(exceptionFactory, parameter, emailAddressPattern!);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the span represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeEmailAddress(
        this Span<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter).MustBeEmailAddress(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeEmailAddress(
        this Span<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter).MustBeEmailAddress(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span represents a valid email address using the provided regular expression,
    /// or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the span represents a valid email.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("emailAddressPattern:null => halt")]
    public static Span<char> MustBeEmailAddress(
        this Span<char> parameter,
        Regex emailAddressPattern,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter).MustBeEmailAddress(emailAddressPattern, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the span represents a valid email address using the provided regular expression,
    /// or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The span that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the span represents a valid email.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> and <paramref name="emailAddressPattern" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address or when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> MustBeEmailAddress(
        this Span<char> parameter,
        Regex emailAddressPattern,
        ReadOnlySpanExceptionFactory<char, Regex> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter).MustBeEmailAddress(emailAddressPattern, exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeEmailAddress(
        this Memory<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeEmailAddress(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeEmailAddress(
        this Memory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeEmailAddress(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the provided regular expression,
    /// or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the memory represents a valid email.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("emailAddressPattern:null => halt")]
    public static Memory<char> MustBeEmailAddress(
        this Memory<char> parameter,
        Regex emailAddressPattern,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeEmailAddress(emailAddressPattern, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the provided regular expression,
    /// or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the memory represents a valid email.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> and <paramref name="emailAddressPattern" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address or when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Memory<char> MustBeEmailAddress(
        this Memory<char> parameter,
        Regex emailAddressPattern,
        ReadOnlySpanExceptionFactory<char, Regex> exceptionFactory
    )
    {
        ((ReadOnlySpan<char>) parameter.Span).MustBeEmailAddress(emailAddressPattern, exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeEmailAddress(
        this ReadOnlyMemory<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustBeEmailAddress(parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeEmailAddress(
        this ReadOnlyMemory<char> parameter,
        ReadOnlySpanExceptionFactory<char> exceptionFactory
    )
    {
        parameter.Span.MustBeEmailAddress(exceptionFactory);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the provided regular expression,
    /// or otherwise throws an <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the memory represents a valid email.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("emailAddressPattern:null => halt")]
    public static ReadOnlyMemory<char> MustBeEmailAddress(
        this ReadOnlyMemory<char> parameter,
        Regex emailAddressPattern,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.Span.MustBeEmailAddress(emailAddressPattern, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the memory represents a valid email address using the provided regular expression,
    /// or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The memory that will be validated as an email address.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the memory represents a valid email.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. A string created from <paramref name="parameter" /> and <paramref name="emailAddressPattern" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid email address or when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<char> MustBeEmailAddress(
        this ReadOnlyMemory<char> parameter,
        Regex emailAddressPattern,
        ReadOnlySpanExceptionFactory<char, Regex> exceptionFactory
    )
    {
        parameter.Span.MustBeEmailAddress(emailAddressPattern, exceptionFactory);
        return parameter;
    }
#endif
}
