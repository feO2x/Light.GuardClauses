using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

// ReSharper disable RedundantNullableFlowAttribute -- Caller might have NRTs turned off

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string starts with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must start with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref="StringComparison.CurrentCulture"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> does not start with <paramref name="value" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType = StringComparison.CurrentCulture,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).StartsWith(value, comparisonType))
        {
            Throw.StringDoesNotStartWith(parameter, value, comparisonType, parameterName, message);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string starts with the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must start with.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not start with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || !parameter.StartsWith(value))
        {
            Throw.CustomException(exceptionFactory, parameter, value!);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string starts with the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must start with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not start with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType,
        [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || !comparisonType.IsValidEnumValue() || !parameter.StartsWith(value, comparisonType))
        {
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        }
        return parameter;
    }
}
