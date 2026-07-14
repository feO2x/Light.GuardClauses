using System;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the specified stream supports writing and returns the original stream, or otherwise throws an
    /// <see cref="ArgumentException" />. Validation reads only <see cref="Stream.CanWrite" /> and performs no I/O.
    /// </summary>
    /// <param name="parameter">The stream to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> does not support writing.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TStream MustBeWritable<TStream>(
        [NotNull, ValidatedNotNull] this TStream? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TStream : Stream
    {
        if (parameter is null)
        {
            Throw.ArgumentNull(parameterName, message);
        }

        if (!parameter.CanWrite)
        {
            Throw.MustBeWritable(parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the specified stream supports writing and returns the original stream, or otherwise throws your
    /// custom exception. Validation reads only <see cref="Stream.CanWrite" /> and performs no I/O.
    /// </summary>
    /// <param name="parameter">The stream to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates the exception to be thrown. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not support writing, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TStream MustBeWritable<TStream>(
        [NotNull, ValidatedNotNull] this TStream? parameter,
        Func<TStream?, Exception> exceptionFactory
    ) where TStream : Stream
    {
        if (parameter is null || !parameter.CanWrite)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }
}
