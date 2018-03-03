using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Provides assertions that check existing state instead of parameters.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Checks if the specified <paramref name="condition" /> is true and throws an <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidOperationException" />.</param>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="condition" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidOperation(bool condition, string message = null)
        {
            if (condition)
                Throw.InvalidOperation(message);
        }
    }
}