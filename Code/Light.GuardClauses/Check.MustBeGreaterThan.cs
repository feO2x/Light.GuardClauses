using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than <paramref name="parameter" />.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeGreaterThan<T>(
        [NotNull] [ValidatedNotNull] this T parameter,
        T other,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : IComparable<T>
    {
        if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) <= 0)
        {
            Throw.MustBeGreaterThan(parameter, other, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="other">The boundary value that must be less than <paramref name="parameter" />.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeGreaterThan<T>(
        [NotNull] [ValidatedNotNull] this T parameter,
        T other,
        Func<T, T, Exception> exceptionFactory
    ) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || parameter.CompareTo(other) <= 0)
        {
            Throw.CustomException(exceptionFactory, parameter!, other);
        }

        return parameter;
    }
}
