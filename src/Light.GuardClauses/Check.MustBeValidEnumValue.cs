using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified enum value is valid, or otherwise throws an <see cref="EnumValueNotDefinedException" />. An enum value
    /// is valid when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type
    /// is marked with the <see cref="FlagsAttribute" />.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="parameter" /> is no valid enum value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustBeValidEnumValue<T>(
        this T parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
        where T : struct, Enum
    {
        if (!EnumInfo<T>.IsValidEnumValue(parameter))
        {
            Throw.EnumValueNotDefined(parameter, parameterName, message);
        }
        
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified enum value is valid, or otherwise throws your custom exception. An enum value
    /// is valid when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type
    /// is marked with the <see cref="FlagsAttribute" />.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. The <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid enum value, or when <typeparamref name="T" /> is no enum type.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static T MustBeValidEnumValue<T>(this T parameter, Func<T, Exception> exceptionFactory)
        where T : struct, Enum
    {
        if (!EnumInfo<T>.IsValidEnumValue(parameter))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }
        
        return parameter;
    }
}
