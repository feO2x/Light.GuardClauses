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
    /// Ensures that the specified object reference is not null, or otherwise throws an <see cref="ArgumentNullException" />.
    /// </summary>
    /// <param name="parameter">The object reference to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static T MustNotBeNull<T>(
        [NotNull, ValidatedNotNull, NoEnumeration] this T? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
        where T : class
    {
        if (parameter is null)
        {
            Throw.ArgumentNull(parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified object reference is not null, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The reference to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static T MustNotBeNull<T>(
        [NotNull, ValidatedNotNull, NoEnumeration] this T? parameter,
        Func<Exception> exceptionFactory
    )
        where T : class
    {
        if (parameter is null)
        {
            Throw.CustomException(exceptionFactory);
        }
        
        return parameter;
    }
}
