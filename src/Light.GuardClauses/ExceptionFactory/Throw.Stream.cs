using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentException" /> indicating that a stream does not support reading, using
    /// the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeReadable(string? parameterName = null, string? message = null) =>
        throw new ArgumentException(message ?? $"{parameterName ?? "The stream"} must be readable.", parameterName);

    /// <summary>
    /// Throws the default <see cref="ArgumentException" /> indicating that a stream does not support writing, using
    /// the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeWritable(string? parameterName = null, string? message = null) =>
        throw new ArgumentException(message ?? $"{parameterName ?? "The stream"} must be writable.", parameterName);

    /// <summary>
    /// Throws the default <see cref="ArgumentException" /> indicating that a stream does not support seeking, using
    /// the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeSeekable(string? parameterName = null, string? message = null) =>
        throw new ArgumentException(message ?? $"{parameterName ?? "The stream"} must be seekable.", parameterName);
}
