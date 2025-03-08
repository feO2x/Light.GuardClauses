using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.ExceptionFactory;
#if NET8_0
using System.Numerics;
#endif

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, using the default tolerance of 0.0001, or otherwise throws an
    /// <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MustBeLessThanOrApproximately(
        this double parameter,
        double other,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    ) =>
        parameter.MustBeLessThanOrApproximately(other, 0.0001, parameterName, message);

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, using the default tolerance of 0.0001, or otherwise throws an
    /// <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" />
    /// are passed to this delegate.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MustBeLessThanOrApproximately(
        this double parameter,
        double other,
        Func<double, double, Exception> exceptionFactory
    )
    {
        if (!parameter.IsLessThanOrApproximately(other))
        {
            Throw.CustomException(exceptionFactory, parameter, other);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MustBeLessThanOrApproximately(
        this double parameter,
        double other,
        double tolerance,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsLessThanOrApproximately(other, tolerance))
        {
            Throw.MustBeLessThanOrApproximately(parameter, other, tolerance, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" />,
    /// <paramref name="other" />, and <paramref name="tolerance" /> are passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MustBeLessThanOrApproximately(
        this double parameter,
        double other,
        double tolerance,
        Func<double, double, double, Exception> exceptionFactory
    )
    {
        if (!parameter.IsLessThanOrApproximately(other, tolerance))
        {
            Throw.CustomException(exceptionFactory, parameter, other, tolerance);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, using the default tolerance of 0.0001f, or otherwise throws an
    /// <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MustBeLessThanOrApproximately(
        this float parameter,
        float other,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        parameter.MustBeLessThanOrApproximately(other, 0.0001f, parameterName, message);

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, using the default tolerance of 0.0001, or otherwise throws an
    /// <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" />
    /// are passed to this delegate.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MustBeLessThanOrApproximately(
        this float parameter,
        float other,
        Func<float, float, Exception> exceptionFactory
    )
    {
        if (!parameter.IsLessThanOrApproximately(other))
        {
            Throw.CustomException(exceptionFactory, parameter, other);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MustBeLessThanOrApproximately(
        this float parameter,
        float other,
        float tolerance,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsLessThanOrApproximately(other, tolerance))
        {
            Throw.MustBeLessThanOrApproximately(parameter, other, tolerance, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" />,
    /// <paramref name="other" />, and <paramref name="tolerance" /> are passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MustBeLessThanOrApproximately(
        this float parameter,
        float other,
        float tolerance,
        Func<float, float, float, Exception> exceptionFactory
    )
    {
        if (!parameter.IsLessThanOrApproximately(other, tolerance))
        {
            Throw.CustomException(exceptionFactory, parameter, other, tolerance);
        }

        return parameter;
    }

#if NET8_0
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <typeparam name="T">The type that implements the <see cref="INumber{T}" /> interface.</typeparam>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustBeLessThanOrApproximately<T>(
        this T parameter,
        T other,
        T tolerance,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : INumber<T>
    {
        if (!parameter.IsLessThanOrApproximately(other, tolerance))
        {
            Throw.MustBeLessThanOrApproximately(parameter, other, tolerance, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is less than or approximately equal to the given
    /// <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="other">The value that <paramref name="parameter" /> should be less than or approximately equal to.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" />,
    /// <paramref name="other" />, and <paramref name="tolerance" /> are passed to this delegate.
    /// </param>
    /// <typeparam name="T">The type that implements the <see cref="INumber{T}" /> interface.</typeparam>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is not less than or approximately equal to <paramref name="other" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustBeLessThanOrApproximately<T>(
        this T parameter,
        T other,
        T tolerance,
        Func<T, T, T, Exception> exceptionFactory
    ) where T : INumber<T>
    {
        if (!parameter.IsLessThanOrApproximately(other, tolerance))
        {
            Throw.CustomException(exceptionFactory, parameter, other, tolerance);
        }

        return parameter;
    }
#endif
}