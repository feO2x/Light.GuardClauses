using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute; 

namespace Light.GuardClauses;

public static partial class Check
{
    /*
     * -------------------------------------
     * Must Not Be Less Than
     * Must Be Greater Than or Equal To
     * -------------------------------------
     */
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeLessThan<T>([NotNull, ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) < 0)
            Throw.MustNotBeLessThan(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeLessThan<T>([NotNull, ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs off
        if (parameter is null || parameter.CompareTo(other) < 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeGreaterThanOrEqualTo<T>([NotNull, ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) < 0)
            Throw.MustBeGreaterThanOrEqualTo(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeGreaterThanOrEqualTo<T>([NotNull, ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) < 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /*
     * -------------------------------------
     * Must Be Less Than
     * Must Not Be Greater Than or Equal To
     * -------------------------------------
     */
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeLessThan<T>([NotNull, ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) >= 0)
            Throw.MustBeLessThan(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeLessThan<T>([NotNull, ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) >= 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeGreaterThanOrEqualTo<T>([NotNull, ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) >= 0)
            Throw.MustNotBeGreaterThanOrEqualTo(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeGreaterThanOrEqualTo<T>([NotNull, ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) >= 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /*
     * -------------------------------------
     * Must Be Greater Than
     * Must Not Be Less Than or Equal To
     * -------------------------------------
     */
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeGreaterThan<T>([ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) <= 0)
            Throw.MustBeGreaterThan(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeGreaterThan<T>([ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) <= 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) <= 0)
            Throw.MustNotBeLessThanOrEqualTo(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) <= 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /*
     * -------------------------------------
     * Must Not Be Greater Than
     * Must Be Less Than or Equal To
     * -------------------------------------
     */
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeGreaterThan<T>([ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) > 0)
            Throw.MustNotBeGreaterThan(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeGreaterThan<T>([ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) > 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) > 0)
            Throw.MustBeLessThanOrEqualTo(parameter, other, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) > 0)
            Throw.CustomException(exceptionFactory, parameter!, other);
        return parameter;
    }

    /*
     * -------------------------------------
     * Ranges
     * -------------------------------------
     */
    /// <summary>
    /// Checks if the value is within the specified range.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
    /// <returns>True if the parameter is within the specified range, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIn<T>([ValidatedNotNull] this T parameter, Range<T> range) where T : IComparable<T> => range.IsValueWithinRange(parameter);

    /// <summary>
    /// Checks if the value is not within the specified range.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must not be in-between.</param>
    /// <returns>True if the parameter is not within the specified range, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotIn<T>([ValidatedNotNull] this T parameter, Range<T> range) where T : IComparable<T> => !range.IsValueWithinRange(parameter);

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is within the specified range, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
    /// <param name="parameter">The parameter to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameter" /> is not within <paramref name="range" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeIn<T>([ValidatedNotNull] this T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (!range.IsValueWithinRange(parameter.MustNotBeNullReference(parameterName, message)))
            Throw.MustBeInRange(parameter, range, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is within the specified range, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The parameter to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="range"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not within <paramref name="range" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeIn<T>([ValidatedNotNull] this T parameter, Range<T> range, Func<T, Range<T>, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
        if (parameter is null || !range.IsValueWithinRange(parameter))
            Throw.CustomException(exceptionFactory, parameter!, range);
        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is not within the specified range, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
    /// <param name="parameter">The parameter to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must not be in-between.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameter" /> is within <paramref name="range" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeIn<T>([ValidatedNotNull] this T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T>
    {
        if (range.IsValueWithinRange(parameter.MustNotBeNullReference(parameterName, message)))
            Throw.MustNotBeInRange(parameter, range, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is not within the specified range, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The parameter to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must not be in-between.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="range"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is within <paramref name="range" />, or when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeIn<T>([ValidatedNotNull] this T parameter, Range<T> range, Func<T, Range<T>, Exception> exceptionFactory) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
        if (parameter is null || range.IsValueWithinRange(parameter))
            Throw.CustomException(exceptionFactory, parameter!, range);
        return parameter;
    }
}