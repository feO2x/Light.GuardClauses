using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

/// <summary>
/// The <see cref="Check" /> class provides access to all assertions of Light.GuardClauses.
/// </summary>
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
    public static T MustBe<T>(this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!EqualityComparer<T>.Default.Equals(parameter, other))
            Throw.ValuesNotEqual(parameter, other, parameterName, message);
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
            Throw.CustomException(exceptionFactory, parameter, other);
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
    public static T MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!equalityComparer.MustNotBeNull(nameof(equalityComparer), message).Equals(parameter, other))
            Throw.ValuesNotEqual(parameter, other, parameterName, message);
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
    public static T MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, Func<T, T, IEqualityComparer<T>, Exception> exceptionFactory)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (equalityComparer is null || !equalityComparer.Equals(parameter, other))
            Throw.CustomException(exceptionFactory, parameter, other, equalityComparer!);
        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the default equality comparer, or otherwise throws a <see cref="ValuesEqualException" />.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ValuesEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustNotBe<T>(this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (EqualityComparer<T>.Default.Equals(parameter, other))
            Throw.ValuesEqual(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the default equality comparer, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustNotBe<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory)
    {
        if (EqualityComparer<T>.Default.Equals(parameter, other))
            Throw.CustomException(exceptionFactory, parameter, other);
        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws a <see cref="ValuesEqualException" />.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ValuesEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("equalityComparer:null => halt")]
    public static T MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (equalityComparer.MustNotBeNull(nameof(equalityComparer), message).Equals(parameter, other))
            Throw.ValuesEqual(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The first value to be compared.</param>
    /// <param name="other">The other value to be compared.</param>
    /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="other" />, and <paramref name="equalityComparer" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal, or when <paramref name="equalityComparer" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("equalityComparer:null => halt")]
    public static T MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, Func<T, T, IEqualityComparer<T>, Exception> exceptionFactory)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (equalityComparer is null || equalityComparer.Equals(parameter, other))
            Throw.CustomException(exceptionFactory, parameter, other, equalityComparer!);
        return parameter;
    }

    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this double value, double other, double tolerance) =>
        Math.Abs(value - other) < tolerance;

    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this double value, double other) =>
        Math.Abs(value - other) < 0.0001;

    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this float value, float other, float tolerance) =>
        Math.Abs(value - other) < tolerance;

    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001f.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than 0.0001f, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this float value, float other) =>
        Math.Abs(value - other) < 0.0001f;

    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this double value, double other, double tolerance) =>
        value > other || value.IsApproximately(other, tolerance);

    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the default tolerance of 0.0001.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this double value, double other) =>
        value > other || value.IsApproximately(other);
    
    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this float value, float other, float tolerance) =>
        value > other || value.IsApproximately(other, tolerance);

    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the default tolerance of 0.0001f.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this float value, float other) =>
        value > other || value.IsApproximately(other);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this double value, double other, double tolerance) =>
        value < other || value.IsApproximately(other, tolerance);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the default tolerance of 0.0001.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this double value, double other) =>
        value < other || value.IsApproximately(other);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this float value, float other, float tolerance) =>
        value < other || value.IsApproximately(other, tolerance);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the default tolerance of 0.0001f.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001f, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this float value, float other) =>
        value < other || value.IsApproximately(other);
}