using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the <see cref="ImmutableArray{T}" /> has at least the specified length, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The <see cref="ImmutableArray{T}" /> to be checked.</param>
    /// <param name="length">The minimum length the <see cref="ImmutableArray{T}" /> should have.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> has less than the specified length.</exception>
    /// <remarks>The default instance of <see cref="ImmutableArray{T}" /> will be treated as having length 0.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustHaveMinimumLength<T>(
        this ImmutableArray<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        var parameterLength = parameter.IsDefault ? 0 : parameter.Length;
        if (parameterLength < length)
        {
            Throw.InvalidMinimumImmutableArrayLength(parameter, length, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the <see cref="ImmutableArray{T}" /> has at least the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The <see cref="ImmutableArray{T}" /> to be checked.</param>
    /// <param name="length">The minimum length the <see cref="ImmutableArray{T}" /> should have.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="length" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> has less than the specified length.</exception>
    /// <remarks>The default instance of <see cref="ImmutableArray{T}" /> will be treated as having length 0.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustHaveMinimumLength<T>(
        this ImmutableArray<T> parameter,
        int length,
        Func<ImmutableArray<T>, int, Exception> exceptionFactory
    )
    {
        var parameterLength = parameter.IsDefault ? 0 : parameter.Length;
        if (parameterLength < length)
        {
            Throw.CustomException(exceptionFactory, parameter, length);
        }

        return parameter;
    }
}
