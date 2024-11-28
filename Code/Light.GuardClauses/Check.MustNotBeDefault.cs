using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified parameter is not the default value, or otherwise throws an <see cref="ArgumentNullException" />
    /// for reference types, or an <see cref="ArgumentDefaultException" /> for value types.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is a reference type and null.</exception>
    /// <exception cref="ArgumentDefaultException">Thrown when <paramref name="parameter" /> is a value type and the default value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeDefault<T>(
        [NotNull, ValidatedNotNull] this T parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (default(T) is null)
        {
            if (parameter is null)
            {
                Throw.ArgumentNull(parameterName, message);
            }

            return parameter;
        }

        if (EqualityComparer<T>.Default.Equals(parameter, default!))
        {
            Throw.ArgumentDefault(parameterName, message);
        }

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
        return parameter;
#pragma warning restore CS8777
    }

    /// <summary>
    /// Ensures that the specified parameter is not the default value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The value to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is the default value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeDefault<T>(
        [NotNull, ValidatedNotNull] this T parameter,
        Func<Exception> exceptionFactory
    )
    {
        if (default(T) is null)
        {
            if (parameter is null)
            {
                Throw.CustomException(exceptionFactory);
            }

            return parameter;
        }

        if (EqualityComparer<T>.Default.Equals(parameter, default!))
        {
            Throw.CustomException(exceptionFactory);
        }

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
        return parameter;
#pragma warning restore CS8777
    }
}