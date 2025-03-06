using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that <paramref name="parameter" /> is within the specified range, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
    /// </summary>
    /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
    /// <param name="parameter">The parameter to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameter" /> is not within <paramref name="range" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustBeIn<T>(
        [NotNull] [ValidatedNotNull] this T parameter,
        Range<T> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : IComparable<T>
    {
        if (!range.IsValueWithinRange(parameter.MustNotBeNullReference(parameterName, message)))
        {
            Throw.MustBeInRange(parameter, range, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that <paramref name="parameter" /> is within the specified range, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The parameter to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="range" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not within <paramref name="range" />, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustBeIn<T>(
        [NotNull] [ValidatedNotNull] this T parameter,
        Range<T> range,
        Func<T, Range<T>, Exception> exceptionFactory
    ) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || !range.IsValueWithinRange(parameter))
        {
            Throw.CustomException(exceptionFactory, parameter!, range);
        }

        return parameter;
    }
}
