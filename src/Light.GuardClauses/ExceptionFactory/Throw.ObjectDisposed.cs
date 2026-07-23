using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws an <see cref="ObjectDisposedException" /> using the optional object name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ObjectDisposed(string? objectName = null, string? message = null) =>
        throw new ObjectDisposedException(objectName, message);
}
