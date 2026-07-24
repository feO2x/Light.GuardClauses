using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that <paramref name="parameter" /> is a non-abstract class, or otherwise throws an
    /// <see cref="ArgumentException" />.
    /// </summary>
    /// <param name="parameter">The type to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original type.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="parameter" /> is not a class or is abstract.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Type MustBeConcreteClass(
        [NotNull] [ValidatedNotNull] this Type? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.MustNotBeNull(parameterName, message);

        if (!(parameter.IsClass && !parameter.IsAbstract))
        {
            Throw.MustBeConcreteClass(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is a non-abstract class, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The type to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. The original type is passed to this delegate.
    /// </param>
    /// <returns>The original type.</returns>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is null, is not a class, or is abstract.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static Type MustBeConcreteClass(
        [NotNull] [ValidatedNotNull] this Type? parameter,
        Func<Type?, Exception> exceptionFactory
    )
    {
        if (parameter is null || !(parameter.IsClass && !parameter.IsAbstract))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
