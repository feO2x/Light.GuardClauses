using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="RelativeUriException" /> indicating that a URI is relative instead of absolute,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeAbsoluteUri(
        Uri parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new RelativeUriException(
            parameterName,
            message ?? $"{parameterName ?? "The URI"} must be an absolute URI, but it actually is \"{parameter}\"."
        );

    /// <summary>
    /// Throws the default <see cref="AbsoluteUriException" /> indicating that a URI is absolute instead of relative,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeRelativeUri(
        Uri parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new AbsoluteUriException(
            parameterName,
            message ?? $"{parameterName ?? "The URI"} must be a relative URI, but it actually is \"{parameter}\"."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidUriSchemeException" /> indicating that a URI has an unexpected scheme,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void UriMustHaveScheme(
        Uri parameter,
        string scheme,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidUriSchemeException(
            parameterName,
            message ??
            $"{parameterName ?? "The URI"} must use the scheme \"{scheme}\", but it actually is \"{parameter}\"."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidUriSchemeException" /> indicating that a URI does not use one of a set of
    /// expected schemes, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void UriMustHaveOneSchemeOf(
        Uri parameter,
        IEnumerable<string> schemes,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidUriSchemeException(
            parameterName,
            message ??
            new StringBuilder().AppendLine($"{parameterName ?? "The URI"} must use one of the following schemes")
                               .AppendItemsWithNewLine(schemes)
                               .AppendLine($"but it actually is \"{parameter}\".")
                               .ToString()
        );
}
