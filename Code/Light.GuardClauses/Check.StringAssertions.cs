using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using System.Runtime.CompilerServices;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string is not null and trimmed, or otherwise throws a <see cref="StringException"/>.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">
    /// Thrown when <paramref name="parameter"/> is not trimmed, i.e. they start or end with white space characters.
    /// Empty strings are regarded as trimmed.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmed([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsTrimmed())
            Throw.NotTrimmed(parameter, parameterName, message);
        return parameter;

    }

    /// <summary>
    /// Ensures that the string is not null and trimmed, or otherwise throws your custom exception.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or not trimmed. Empty strings are regarded as trimmed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmed([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (parameter is null || !parameter.AsSpan().IsTrimmed())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }

    /// <summary>
    /// Checks if the specified string is trimmed at the start, i.e. it does not start with
    /// white space characters. Inputting an empty string will return true.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regardNullAsTrimmed">
    /// The value indicating whether true or false should be returned from this method when the
    /// <paramref name="parameter" /> is null. The default value is true.
    /// </param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the start, else false.
    /// An empty string will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtStart(this string? parameter, bool regardNullAsTrimmed = true) =>
        parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmedAtStart();

    /// <summary>
    /// Checks if the specified character span is trimmed at the start, i.e. it does not start with
    /// white space characters. Inputting an empty span will return true.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the start, else false.
    /// An empty span will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtStart(this ReadOnlySpan<char> parameter) =>
        parameter.Length == 0 || !parameter[0].IsWhiteSpace();
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the start, or otherwise throws a <see cref="StringException"/>.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">
    /// Thrown when <paramref name="parameter"/> is not trimmed at the start, i.e. they start with white space characters.
    /// Empty strings are regarded as trimmed.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtStart([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsTrimmedAtStart())
            Throw.NotTrimmedAtStart(parameter, parameterName, message);
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the start, or otherwise throws your custom exception.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or not trimmed at the start. Empty strings are regarded as trimmed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtStart([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (parameter is null || !parameter.AsSpan().IsTrimmedAtStart())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }
    
    /// <summary>
    /// Checks if the specified string is trimmed at the end, i.e. it does not end with
    /// white space characters. Inputting an empty string will return true.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regardNullAsTrimmed">
    /// The value indicating whether true or false should be returned from this method when the
    /// <paramref name="parameter" /> is null. The default value is true.
    /// </param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the start, else false.
    /// An empty string will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtEnd(this string? parameter, bool regardNullAsTrimmed = true) =>
        parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmedAtEnd();

    /// <summary>
    /// Checks if the specified character span is trimmed at the end, i.e. it does not end with
    /// white space characters. Inputting an empty span will return true.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the end, else false.
    /// An empty span will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtEnd(this ReadOnlySpan<char> parameter) =>
        parameter.Length == 0 || !parameter[parameter.Length - 1].IsWhiteSpace();
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the end, or otherwise throws a <see cref="StringException"/>.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">
    /// Thrown when <paramref name="parameter"/> is not trimmed at the end, i.e. they end with white space characters.
    /// Empty strings are regarded as trimmed.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtEnd([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsTrimmedAtEnd())
            Throw.NotTrimmedAtEnd(parameter, parameterName, message);
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the end, or otherwise throws your custom exception.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or not trimmed at the end. Empty strings are regarded as trimmed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtEnd([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (parameter is null || !parameter.AsSpan().IsTrimmedAtEnd())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }
}