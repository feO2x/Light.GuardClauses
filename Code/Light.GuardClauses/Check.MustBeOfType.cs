
using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that <paramref name="parameter" /> can be cast to <typeparamref name="T" /> and returns the cast value, or otherwise throws a <see cref="TypeCastException" />.
    /// </summary>
    /// <param name="parameter">The value to be cast.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="TypeCastException">Thrown when <paramref name="parameter" /> cannot be cast to <typeparamref name="T" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeOfType<T>([NotNull, ValidatedNotNull, NoEnumeration] this object? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.MustNotBeNull(parameterName, message) is T castValue)
            return castValue;

        Throw.InvalidTypeCast(parameter, typeof(T), parameterName, message);
        return default;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> can be cast to <typeparamref name="T" /> and returns the cast value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be cast.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. The <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> cannot be cast to <typeparamref name="T" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeOfType<T>([NotNull, ValidatedNotNull, NoEnumeration] this object? parameter, Func<object?, Exception> exceptionFactory)
    {
        if (parameter is T castValue)
            return castValue;

        Throw.CustomException(exceptionFactory, parameter);
        return default;
    }
}