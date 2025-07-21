using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the <see cref="ImmutableArray{T}" /> has at most the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The <see cref="ImmutableArray{T}" /> to be checked.</param>
    /// <param name="length">The maximum length the <see cref="ImmutableArray{T}" /> should have.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> has more than the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustHaveMaximumLength<T>(
        this ImmutableArray<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.IsDefault && length < 0 || parameter.Length > length)
        {
            Throw.InvalidMaximumCollectionCount(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the <see cref="ImmutableArray{T}" /> has at most the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The <see cref="ImmutableArray{T}" /> to be checked.</param>
    /// <param name="length">The maximum length the <see cref="ImmutableArray{T}" /> should have.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> has more than the specified length.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustHaveMaximumLength<T>(
        this ImmutableArray<T> parameter,
        int length,
        Func<ImmutableArray<T>, int, Exception> exceptionFactory
    )
    {
        if (parameter.IsDefault && length < 0 || parameter.Length > length)
        {
            Throw.CustomException(exceptionFactory, parameter, length);
        }

        return parameter;
    }
}
