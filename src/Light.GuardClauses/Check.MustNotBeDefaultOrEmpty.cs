using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified <see cref="ImmutableArray{T}" /> is not default or empty, or otherwise throws an <see cref="Exceptions.EmptyCollectionException" />.
    /// </summary>
    /// <param name="parameter">The <see cref="ImmutableArray{T}" /> to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="Exceptions.EmptyCollectionException">Thrown when <paramref name="parameter" /> is default or empty.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustNotBeDefaultOrEmpty<T>(
        this ImmutableArray<T> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.IsDefaultOrEmpty)
        {
            Throw.EmptyCollection(parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <see cref="ImmutableArray{T}" /> is not default or empty, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The <see cref="ImmutableArray{T}" /> to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. The <see cref="ImmutableArray{T}" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is default or empty.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static ImmutableArray<T> MustNotBeDefaultOrEmpty<T>(
        this ImmutableArray<T> parameter,
        Func<ImmutableArray<T>, Exception> exceptionFactory
    )
    {
        if (parameter.IsDefaultOrEmpty)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
