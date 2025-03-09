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
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="value" /> contains <paramref name="parameter" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (value.MustNotBeNull(nameof(value), message).Contains(parameter.MustNotBeNull(parameterName, message)))
        {
            Throw.Substring(parameter, value, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="value" /> contains <paramref name="parameter" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        Func<string?, string, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || value is null || value.Contains(parameter))
        {
            Throw.CustomException(exceptionFactory, parameter, value!);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="value" /> contains <paramref name="parameter" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        StringComparison comparisonType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        value.MustNotBeNull(nameof(value), message);
        parameter.MustNotBeNull(parameterName, message);
        if (value.IndexOf(parameter, comparisonType) != -1)
        {
            Throw.Substring(parameter, value, comparisonType, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="value" /> contains <paramref name="parameter" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf(
        [NotNull] [ValidatedNotNull] this string? parameter,
        string value,
        StringComparison comparisonType,
        Func<string?, string, StringComparison, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || value is null || value.IndexOf(parameter, comparisonType) != -1)
        {
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        }

        return parameter;
    }
}
