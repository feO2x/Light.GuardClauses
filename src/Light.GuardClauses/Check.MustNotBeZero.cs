using System;
#if NET8_0_OR_GREATER
using System.Numerics;
#endif
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int MustNotBeZero(
        this int parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter == 0)
        {
            Throw.MustNotBeZero(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static int MustNotBeZero(this int parameter, Func<int, Exception> exceptionFactory)
    {
        if (parameter == 0)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long MustNotBeZero(
        this long parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter == 0L)
        {
            Throw.MustNotBeZero(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static long MustNotBeZero(this long parameter, Func<long, Exception> exceptionFactory)
    {
        if (parameter == 0L)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal MustNotBeZero(
        this decimal parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter == 0m)
        {
            Throw.MustNotBeZero(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static decimal MustNotBeZero(this decimal parameter, Func<decimal, Exception> exceptionFactory)
    {
        if (parameter == 0m)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> compares equal to zero (including negative zero).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MustNotBeZero(
        this float parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter == 0f)
        {
            Throw.MustNotBeZero(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> compares equal to zero (including negative zero).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static float MustNotBeZero(this float parameter, Func<float, Exception> exceptionFactory)
    {
        if (parameter == 0f)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> compares equal to zero (including negative zero).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MustNotBeZero(
        this double parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter == 0d)
        {
            Throw.MustNotBeZero(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> compares equal to zero (including negative zero).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static double MustNotBeZero(this double parameter, Func<double, Exception> exceptionFactory)
    {
        if (parameter == 0d)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is <see cref="TimeSpan.Zero" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan MustNotBeZero(
        this TimeSpan parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter == TimeSpan.Zero)
        {
            Throw.MustNotBeZero(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is <see cref="TimeSpan.Zero" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static TimeSpan MustNotBeZero(this TimeSpan parameter, Func<TimeSpan, Exception> exceptionFactory)
    {
        if (parameter == TimeSpan.Zero)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <typeparam name="T">The type that implements the <see cref="INumber{T}" /> interface.</typeparam>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> compares equal to zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustNotBeZero<T>(
        this T parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : INumber<T>
    {
        if (parameter == T.Zero)
        {
            Throw.MustNotBeZero(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not zero, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <typeparam name="T">The type that implements the <see cref="INumber{T}" /> interface.</typeparam>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> compares equal to zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static T MustNotBeZero<T>(this T parameter, Func<T, Exception> exceptionFactory)
        where T : INumber<T>
    {
        if (parameter == T.Zero)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
#endif
}
