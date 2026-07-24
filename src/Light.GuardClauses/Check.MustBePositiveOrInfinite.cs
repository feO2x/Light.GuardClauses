using System;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is positive (greater than zero) or exactly equals
    /// <see cref="System.Threading.Timeout.InfiniteTimeSpan" />, or otherwise throws an
    /// <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameter" /> is neither positive nor exactly equal to
    /// <see cref="System.Threading.Timeout.InfiniteTimeSpan" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan MustBePositiveOrInfinite(
        this TimeSpan parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!(parameter > TimeSpan.Zero || parameter == Timeout.InfiniteTimeSpan))
        {
            Throw.MustBePositiveOrInfinite(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is positive (greater than zero) or exactly equals
    /// <see cref="System.Threading.Timeout.InfiniteTimeSpan" />, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">
    /// The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.
    /// </param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> is neither positive nor exactly equal to
    /// <see cref="System.Threading.Timeout.InfiniteTimeSpan" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static TimeSpan MustBePositiveOrInfinite(
        this TimeSpan parameter,
        Func<TimeSpan, Exception> exceptionFactory
    )
    {
        if (!(parameter > TimeSpan.Zero || parameter == Timeout.InfiniteTimeSpan))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
