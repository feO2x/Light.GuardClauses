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
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is less than zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int MustNotBeNegative(
        this int parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!(parameter >= 0))
        {
            Throw.MustNotBeNegative(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is less than zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static int MustNotBeNegative(this int parameter, Func<int, Exception> exceptionFactory)
    {
        if (!(parameter >= 0))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is less than zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long MustNotBeNegative(
        this long parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!(parameter >= 0L))
        {
            Throw.MustNotBeNegative(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is less than zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static long MustNotBeNegative(this long parameter, Func<long, Exception> exceptionFactory)
    {
        if (!(parameter >= 0L))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is less than zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal MustNotBeNegative(
        this decimal parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!(parameter >= 0m))
        {
            Throw.MustNotBeNegative(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is less than zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static decimal MustNotBeNegative(this decimal parameter, Func<decimal, Exception> exceptionFactory)
    {
        if (!(parameter >= 0m))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is less than zero or NaN.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MustNotBeNegative(
        this float parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!(parameter >= 0f))
        {
            Throw.MustNotBeNegative(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is less than zero or NaN.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static float MustNotBeNegative(this float parameter, Func<float, Exception> exceptionFactory)
    {
        if (!(parameter >= 0f))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is less than zero or NaN.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MustNotBeNegative(
        this double parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!(parameter >= 0d))
        {
            Throw.MustNotBeNegative(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is less than zero or NaN.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static double MustNotBeNegative(this double parameter, Func<double, Exception> exceptionFactory)
    {
        if (!(parameter >= 0d))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is less than <see cref="TimeSpan.Zero" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan MustNotBeNegative(
        this TimeSpan parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!(parameter >= TimeSpan.Zero))
        {
            Throw.MustNotBeNegative(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is less than <see cref="TimeSpan.Zero" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static TimeSpan MustNotBeNegative(this TimeSpan parameter, Func<TimeSpan, Exception> exceptionFactory)
    {
        if (!(parameter >= TimeSpan.Zero))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <typeparam name="T">The type that implements the <see cref="INumber{T}" /> interface.</typeparam>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is less than zero or NaN.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustNotBeNegative<T>(
        this T parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : INumber<T>
    {
        if (!(parameter >= T.Zero))
        {
            Throw.MustNotBeNegative(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is not negative (greater than or equal to zero), or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <typeparam name="T">The type that implements the <see cref="INumber{T}" /> interface.</typeparam>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is less than zero or NaN.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static T MustNotBeNegative<T>(this T parameter, Func<T, Exception> exceptionFactory)
        where T : INumber<T>
    {
        if (!(parameter >= T.Zero))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
#endif
}
