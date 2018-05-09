using System;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeLessThan<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) < 0)
                Throw.MustNotBeLessThan(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeLessThan<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) < 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }
    }
}
