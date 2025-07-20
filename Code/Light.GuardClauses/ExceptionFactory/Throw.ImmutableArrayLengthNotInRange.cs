using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that an <see cref="ImmutableArray{T}" />'s length is not within the
    /// given range, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ImmutableArrayLengthNotInRange<T>(
        ImmutableArray<T> parameter,
        Range<int> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ??
            $"{parameterName ?? "The immutable array"} must have its length in between {range.CreateRangeDescriptionText("and")}, but it actually has length {parameter.Length}."
        );
}
