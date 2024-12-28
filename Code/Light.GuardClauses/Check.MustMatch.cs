using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string matches the specified regular expression, or otherwise throws a <see cref="StringDoesNotMatchException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regex">The regular expression used for pattern matching.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringDoesNotMatchException">Thrown when <paramref name="parameter" /> does not match the specified regular expression.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="regex" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; regex:null => halt")]
    public static string MustMatch(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Regex regex,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!regex.MustNotBeNull(nameof(regex), message).IsMatch(parameter.MustNotBeNull(parameterName, message)))
        {
            Throw.StringDoesNotMatch(parameter, regex, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string matches the specified regular expression, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regex">The regular expression used for pattern matching.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="regex" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not match the specified regular expression,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="regex" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string MustMatch(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Regex regex,
        Func<string?, Regex, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || regex is null || !regex.IsMatch(parameter))
        {
            Throw.CustomException(exceptionFactory, parameter, regex!);
        }

        return parameter;
    }
}
