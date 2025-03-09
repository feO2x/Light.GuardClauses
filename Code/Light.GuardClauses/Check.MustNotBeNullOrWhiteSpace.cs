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
    /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws an <see cref="ArgumentNullException" />, an <see cref="EmptyStringException" />, or a <see cref="WhiteSpaceStringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="WhiteSpaceStringException">Thrown when <paramref name="parameter" /> contains only white space.</exception>
    /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter" /> is an empty string.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustNotBeNullOrWhiteSpace(
        [NotNull] [ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        parameter.MustNotBeNullOrEmpty(parameterName, message);

        foreach (var character in parameter)
        {
            if (!character.IsWhiteSpace())
            {
                return parameter;
            }
        }

        Throw.WhiteSpaceString(parameter, parameterName, message);
        return null;
    }

    /// <summary>
    /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null, empty, or contains only white space.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory: null => halt")]
    public static string MustNotBeNullOrWhiteSpace(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter.IsNullOrWhiteSpace())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
