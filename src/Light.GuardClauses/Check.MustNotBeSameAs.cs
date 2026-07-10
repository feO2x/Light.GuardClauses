using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that <paramref name="parameter" /> and <paramref name="other" /> do not point to the same object instance, or otherwise
    /// throws a <see cref="SameObjectReferenceException" />.
    /// </summary>
    /// <param name="parameter">The first reference to be checked.</param>
    /// <param name="other">The second reference to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SameObjectReferenceException">Thrown when both <paramref name="parameter" /> and <paramref name="other" /> point to the same object.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? MustNotBeSameAs<T>(
        [NoEnumeration] this T? parameter,
        [NoEnumeration] T? other,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : class
    {
        if (ReferenceEquals(parameter, other))
        {
            Throw.SameObjectReference(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> and <paramref name="other" /> do not point to the same object instance, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The first reference to be checked.</param>
    /// <param name="other">The second reference to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="SameObjectReferenceException">Thrown when both <paramref name="parameter" /> and <paramref name="other" /> point to the same object.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? MustNotBeSameAs<T>([NoEnumeration] this T? parameter, T? other, Func<T?, Exception> exceptionFactory) where T : class
    {
        if (ReferenceEquals(parameter, other))
        {
            Throw.CustomException(exceptionFactory, parameter);
        }
        
        return parameter;
    }
}
