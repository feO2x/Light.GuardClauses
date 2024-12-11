using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified <paramref name="condition" /> is true and throws an <see cref="ArgumentException" /> in this case.
    /// </summary>
    /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the <see cref="ArgumentException" /> (optional).</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="condition" /> is true.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvalidArgument(bool condition, string? parameterName = null, string? message = null)
    {
        if (condition)
        {
            Throw.Argument(parameterName, message);
        }
    }

    /// <summary>
    /// Checks if the specified <paramref name="condition" /> is true and throws your custom exception in this case.
    /// </summary>
    /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="condition" /> is true.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static void InvalidArgument(bool condition, Func<Exception> exceptionFactory)
    {
        if (condition)
        {
            Throw.CustomException(exceptionFactory);
        }
    }

    /// <summary>
    /// Checks if the specified <paramref name="condition" /> is true and throws your custom exception in this case.
    /// </summary>
    /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
    /// <param name="parameter">The value that is checked in the <paramref name="condition" />. This value is passed to the <paramref name="exceptionFactory" />.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. The <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="condition" /> is true.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static void InvalidArgument<T>(bool condition, T parameter, Func<T, Exception> exceptionFactory)
    {
        if (condition)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }
    }
}
