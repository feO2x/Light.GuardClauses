﻿using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified <paramref name="parameter"/> uses <see cref="DateTimeKind.Utc"/>, or otherwise throws an <see cref="InvalidDateTimeException"/>.
    /// </summary>
    /// <param name="parameter">The date time to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="parameter"/> does not use <see cref="DateTimeKind.Utc"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime MustBeUtc(this DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.Kind != DateTimeKind.Utc)
            Throw.MustBeUtcDateTime(parameter, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter"/> uses <see cref="DateTimeKind.Utc"/>, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The date time to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not use <see cref="DateTimeKind.Utc"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static DateTime MustBeUtc(this DateTime parameter, Func<DateTime, Exception> exceptionFactory)
    {
        if (parameter.Kind != DateTimeKind.Utc)
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter"/> uses <see cref="DateTimeKind.Local"/>, or otherwise throws an <see cref="InvalidDateTimeException"/>.
    /// </summary>
    /// <param name="parameter">The date time to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="parameter"/> does not use <see cref="DateTimeKind.Local"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime MustBeLocal(this DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.Kind != DateTimeKind.Local)
            Throw.MustBeLocalDateTime(parameter, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter"/> uses <see cref="DateTimeKind.Local"/>, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The date time to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not use <see cref="DateTimeKind.Local"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static DateTime MustBeLocal(this DateTime parameter, Func<DateTime, Exception> exceptionFactory)
    {
        if (parameter.Kind != DateTimeKind.Local)
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter"/> uses <see cref="DateTimeKind.Unspecified"/>, or otherwise throws an <see cref="InvalidDateTimeException"/>.
    /// </summary>
    /// <param name="parameter">The date time to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="parameter"/> does not use <see cref="DateTimeKind.Unspecified"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime MustBeUnspecified(this DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.Kind != DateTimeKind.Unspecified)
            Throw.MustBeUnspecifiedDateTime(parameter, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the specified <paramref name="parameter"/> uses <see cref="DateTimeKind.Unspecified"/>, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The date time to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not use <see cref="DateTimeKind.Unspecified"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static DateTime MustBeUnspecified(this DateTime parameter, Func<DateTime, Exception> exceptionFactory)
    {
        if (parameter.Kind != DateTimeKind.Unspecified)
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }
}