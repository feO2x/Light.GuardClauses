using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// The <see cref="CommonAssertions" /> class contains the most common assertions like MustNotBeNull and assertions that are not directly related to
    /// any categories like collection assertions or string assertions.
    /// </summary>
    public static class CommonAssertions
    {
        /// <summary>
        /// Ensures that the specified reference is not null, or otherwise an <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="parameter">The reference to be checked.</param>
        /// <param name="parameterName">Foo</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException"/> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null..</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }
    }
}