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
    /// Ensures that the specified nullable has a value and returns it, or otherwise throws a <see cref="NullableHasNoValueException" />.
    /// </summary>
    /// <param name="parameter">The nullable to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="NullableHasNoValueException">Thrown when <paramref name="parameter" /> has no value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T MustHaveValue<T>(
        [NotNull, NoEnumeration] this T? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : struct
    {
        if (!parameter.HasValue)
        {
            Throw.NullableHasNoValue(parameterName, message);
        }

        return parameter.Value;
    }

    /// <summary>
    /// Ensures that the specified nullable has a value and returns it, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The nullable to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
    /// <exception cref="NullableHasNoValueException">Thrown when <paramref name="parameter" /> has no value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static T MustHaveValue<T>([NotNull, NoEnumeration] this T? parameter, Func<Exception> exceptionFactory)
        where T : struct
    {
        if (!parameter.HasValue)
        {
            Throw.CustomException(exceptionFactory);
        }

        return parameter.Value;
    }
}
