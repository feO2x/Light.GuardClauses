using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="StringDoesNotMatchException" /> indicating that a string does not match a regular
    /// expression, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotMatch(
        string parameter,
        Regex regex,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new StringDoesNotMatchException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must match the regular expression \"{regex}\", but it actually is \"{parameter}\"."
        );
}
