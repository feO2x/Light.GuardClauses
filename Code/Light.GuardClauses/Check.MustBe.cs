using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the default equality comparer, or otherwise throws a <see cref="ValuesNotEqualException" />.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustBe<T>(
        this T parameter,
        T other,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!EqualityComparer<T>.Default.Equals(parameter, other))
        {
            Throw.ValuesNotEqual(parameter, other, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the default equality comparer, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustBe<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory)
    {
        if (!EqualityComparer<T>.Default.Equals(parameter, other))
        {
            Throw.CustomException(exceptionFactory, parameter, other);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws a <see cref="ValuesNotEqualException" />.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("equalityComparer:null => halt")]
    public static T MustBe<T>(
        this T parameter,
        T other,
        IEqualityComparer<T> equalityComparer,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!equalityComparer.MustNotBeNull(nameof(equalityComparer), message).Equals(parameter, other))
        {
            Throw.ValuesNotEqual(parameter, other, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="other" />, and <paramref name="equalityComparer" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal, or when <paramref name="equalityComparer" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("equalityComparer:null => halt")]
    public static T MustBe<T>(
        this T parameter,
        T other,
        IEqualityComparer<T> equalityComparer,
        Func<T, T, IEqualityComparer<T>, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (equalityComparer is null || !equalityComparer.Equals(parameter, other))
        {
            Throw.CustomException(exceptionFactory, parameter, other, equalityComparer!);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws a <see cref="ValuesNotEqualException" />.
    /// </summary>
    /// <param name="parameter">The first string to be compared.</param>
    /// <param name="other">The second string to be compared.</param>
    /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? MustBe(
        this string? parameter,
        string? other,
        StringComparison comparisonType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!string.Equals(parameter, other, comparisonType))
        {
            Throw.ValuesNotEqual(parameter, other, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The first string to be compared.</param>
    /// <param name="other">The second string to be compared.</param>
    /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? MustBe(
        this string? parameter,
        string? other,
        StringComparison comparisonType,
        Func<string?, string?, StringComparison, Exception> exceptionFactory
    )
    {
        if (!string.Equals(parameter, other, comparisonType))
        {
            Throw.CustomException(exceptionFactory, parameter, other, comparisonType);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws a <see cref="ValuesNotEqualException" />.
    /// </summary>
    /// <param name="parameter">The first string to be compared.</param>
    /// <param name="other">The second string to be compared.</param>
    /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparisonType" /> enum.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? MustBe(
        this string? parameter,
        string? other,
        StringComparisonType comparisonType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.Equals(other, comparisonType))
        {
            Throw.ValuesNotEqual(parameter, other, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The first string to be compared.</param>
    /// <param name="other">The second string to be compared.</param>
    /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparisonType" /> enum.</exception>
    public static string? MustBe(
        this string? parameter,
        string? other,
        StringComparisonType comparisonType,
        Func<string?, string?, StringComparisonType, Exception> exceptionFactory
    )
    {
        if (!parameter.Equals(other, comparisonType))
        {
            Throw.CustomException(exceptionFactory, parameter, other, comparisonType);
        }

        return parameter;
    }
}
