using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified GUID structurally identifies an RFC/IETF UUID version 7, or otherwise throws an <see cref="ArgumentException" />.
    /// </summary>
    /// <param name="parameter">The GUID to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original GUID.</returns>
    /// <exception cref="ArgumentException">Thrown when the UUID version is not 7 or the variant is not RFC/IETF.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid MustBeUuidVersion7(
        this Guid parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.IsUuidVersion7())
        {
            Throw.Argument(
                parameterName,
                message ?? $"{parameterName ?? "The GUID"} must be an RFC/IETF UUID version 7, but it actually is \"{parameter}\"."
            );
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified GUID structurally identifies an RFC/IETF UUID version 7, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The GUID to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <returns>The original GUID.</returns>
    /// <exception cref="Exception">Your custom exception thrown when the UUID version is not 7 or the variant is not RFC/IETF.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static Guid MustBeUuidVersion7(this Guid parameter, Func<Guid, Exception> exceptionFactory)
    {
        if (!parameter.IsUuidVersion7())
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
