﻿using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string is not null and trimmed at the end, or otherwise throws a <see cref="StringException" />.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">
    /// Thrown when <paramref name="parameter" /> is not trimmed at the end, i.e. they end with white space characters.
    /// Empty strings are regarded as trimmed.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtEnd(
        [NotNull] [ValidatedNotNull] this string? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsTrimmedAtEnd())
        {
            Throw.NotTrimmedAtEnd(parameter, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the string is not null and trimmed at the end, or otherwise throws your custom exception.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or not trimmed at the end. Empty strings are regarded as trimmed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtEnd(
        [NotNull] [ValidatedNotNull] this string? parameter,
        Func<string?, Exception> exceptionFactory
    )
    {
        if (parameter is null || !parameter.AsSpan().IsTrimmedAtEnd())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
