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
    /// Ensures that the string's length is within the specified range, or otherwise throws a <see cref="StringLengthException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="range">The range where the string's length must be in-between.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when the length of <paramref name="parameter" /> is not with the specified <paramref name="range" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLengthIn(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Range<int> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!range.IsValueWithinRange(parameter.MustNotBeNull(parameterName, message).Length))
        {
            Throw.StringLengthNotInRange(parameter, range, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string's length is within the specified range, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="range">The range where the string's length must be in-between.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="range" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or its length is not within the specified range.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLengthIn(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Range<int> range,
        Func<string?, Range<int>, Exception> exceptionFactory
    )
    {
        if (parameter is null || !range.IsValueWithinRange(parameter.Length))
        {
            Throw.CustomException(exceptionFactory, parameter, range);
        }

        return parameter;
    }
}
