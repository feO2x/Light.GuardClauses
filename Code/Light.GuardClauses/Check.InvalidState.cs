using System.Runtime.CompilerServices;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified <paramref name="condition" /> is true and throws an <see cref="InvalidStateException" /> in this case.
    /// </summary>
    /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
    /// <param name="message">The message that will be passed to the <see cref="InvalidStateException" />.</param>
    /// <exception cref="InvalidStateException">Thrown when <paramref name="condition" /> is true.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvalidState(bool condition, string? message = null)
    {
        if (condition)
        {
            Throw.InvalidState(message);
        }
    }
}
