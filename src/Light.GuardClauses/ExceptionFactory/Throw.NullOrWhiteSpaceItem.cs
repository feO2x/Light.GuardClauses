using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ExistingItemException" /> indicating that a collection contains a null, empty,
    /// or white-space-only string.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NullOrWhiteSpaceItem(
        string? item,
        int position,
        string? parameterName = null,
        string? message = null
    ) =>
        throw new ExistingItemException(
            parameterName,
            message ??
            $"{parameterName ?? "The collection"} must not contain null, empty, or white-space-only strings, but {GetFailureCategory(item)} was found at position {position}."
        );

    private static string GetFailureCategory(string? item) =>
        item is null     ? "a null string" :
        item.Length == 0 ? "an empty string" : "a white-space-only string";
}
