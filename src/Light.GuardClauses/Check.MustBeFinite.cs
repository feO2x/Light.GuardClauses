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
    /// Ensures that the specified single-precision floating-point value is finite, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MustBeFinite(
        this float parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsFinite())
        {
            Throw.NotFinite(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified single-precision floating-point value is finite, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static float MustBeFinite(this float parameter, Func<float, Exception> exceptionFactory)
    {
        if (!parameter.IsFinite())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified double-precision floating-point value is finite, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MustBeFinite(
        this double parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsFinite())
        {
            Throw.NotFinite(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified double-precision floating-point value is finite, or otherwise throws your custom exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static double MustBeFinite(this double parameter, Func<double, Exception> exceptionFactory)
    {
        if (!parameter.IsFinite())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Ensures that the specified IEEE 754 floating-point value is finite, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <typeparam name="T">The IEEE 754 floating-point type.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustBeFinite<T>(
        this T parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : IFloatingPointIeee754<T>
    {
        if (!T.IsFinite(parameter))
        {
            Throw.NotFinite(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified IEEE 754 floating-point value is finite, or otherwise throws your custom exception.
    /// </summary>
    /// <typeparam name="T">The IEEE 754 floating-point type.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static T MustBeFinite<T>(this T parameter, Func<T, Exception> exceptionFactory)
        where T : IFloatingPointIeee754<T>
    {
        if (!T.IsFinite(parameter))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
#endif
}
