using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that values of <paramref name="parameter" /> can be assigned to variables of
    /// <paramref name="requiredType" />, or otherwise throws an <see cref="ArgumentException" />.
    /// </summary>
    /// <param name="parameter">The candidate type to be checked.</param>
    /// <param name="requiredType">The type to which values of the candidate type must be assignable.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original candidate type.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when values of <paramref name="parameter" /> cannot be assigned to variables of
    /// <paramref name="requiredType" />.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="parameter" /> or <paramref name="requiredType" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; requiredType:null => halt")]
    public static Type MustBeAssignableTo(
        [NotNull] [ValidatedNotNull] this Type? parameter,
        [NotNull] [ValidatedNotNull] Type? requiredType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.MustNotBeNull(parameterName, message);
        requiredType.MustNotBeNull(nameof(requiredType), message);

        if (!requiredType.IsAssignableFrom(parameter))
        {
            Throw.MustBeAssignableTo(parameter, requiredType, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that values of <paramref name="parameter" /> can be assigned to variables of
    /// <paramref name="requiredType" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The candidate type to be checked.</param>
    /// <param name="requiredType">The type to which values of the candidate type must be assignable.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. The original candidate and required types are passed to this
    /// delegate.
    /// </param>
    /// <returns>The original candidate type.</returns>
    /// <exception cref="Exception">
    /// Your custom exception thrown when either type is null or the candidate type is not assignable to the required
    /// type.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation(
        "parameter:null => halt; parameter:notnull => notnull; requiredType:null => halt; exceptionFactory:null => halt"
    )]
    public static Type MustBeAssignableTo(
        [NotNull] [ValidatedNotNull] this Type? parameter,
        [NotNull] [ValidatedNotNull] Type? requiredType,
        Func<Type?, Type?, Exception> exceptionFactory
    )
    {
        if (parameter is null || requiredType is null || !requiredType.IsAssignableFrom(parameter))
        {
            Throw.CustomException(exceptionFactory, parameter, requiredType);
        }

        return parameter;
    }
}
