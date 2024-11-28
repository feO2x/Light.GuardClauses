using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified parameter is not null when <typeparamref name="T" /> is a reference type, or otherwise
    /// throws an <see cref="ArgumentNullException" />. PLEASE NOTICE: you should only use this assertion in generic contexts,
    /// use <see cref="MustNotBeNull{T}(T,string,string)" /> by default.
    /// </summary>
    /// <param name="parameter">The value to be checked for null.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeNullReference<T>(
        [NotNull, ValidatedNotNull, NoEnumeration]
        this T parameter,
        [CallerArgumentExpression("parameter")]
        string? parameterName = null,
        string? message = null
    )
    {
        if (default(T) != null)
        {
            // If we end up here, parameter cannot be null
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
            return parameter;
#pragma warning restore CS8777
        }

        if (parameter is null)
        {
            Throw.ArgumentNull(parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified parameter is not null when <typeparamref name="T" /> is a reference type, or otherwise
    /// throws your custom exception. PLEASE NOTICE: you should only use this assertion in generic contexts,
    /// use <see cref="MustNotBeNull{T}(T,Func{Exception})" /> by default.
    /// </summary>
    /// <param name="parameter">The value to be checked for null.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
    /// <exception cref="Exception">Your custom exception thrown when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeNullReference<T>(
        [NotNull, ValidatedNotNull, NoEnumeration]
        this T parameter,
        Func<Exception> exceptionFactory
    )
    {
        if (default(T) != null)
        {
            // If we end up here, parameter cannot be null
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
            return parameter;
#pragma warning restore CS8777
        }

        if (parameter is null)
        {
            Throw.CustomException(exceptionFactory);
        }

        return parameter;
    }
}