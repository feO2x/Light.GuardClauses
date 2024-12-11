using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified <paramref name="condition" /> is true and throws an <see cref="InvalidOperationException" /> in this case.
    /// </summary>
    /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
    /// <param name="message">The message that will be passed to the <see cref="InvalidOperationException" /> (optional).</param>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="condition" /> is true.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvalidOperation(bool condition, string? message = null)
    {
        if (condition)
        {
            Throw.InvalidOperation(message);
        }
    }
}
