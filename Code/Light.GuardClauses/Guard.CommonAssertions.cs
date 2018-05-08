using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// The <see cref="Guard"/> class provides access to all assertions of Light.GuardClauses.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <param name="parameter">The object reference to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentNullException" /> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SL5)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The reference to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SL5)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNull<T>(this T parameter, Func<Exception> exceptionFactory) where T : class
        {
            if (parameter == null)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified parameter is not the default value, or otherwise throws an <see cref="ArgumentNullException" />
        /// for reference types, or an <see cref="ArgumentDefaultException" /> for value types.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is a reference type and null.</exception>
        /// <exception cref="ArgumentDefaultException">Thrown when <paramref name="parameter"/> is a value type and the default value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SL5)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeDefault<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) == null)
            {
                if (parameter == null)
                    Throw.MustNotBeNull(parameterName, message);
                return parameter;
            }

            if (parameter.Equals(default(T)))
                Throw.MustNotBeDefault(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified parameter is not the default value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is the default value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SL5)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeDefault<T>(this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T) == null)
            {
                if (parameter == null)
                    Throw.CustomException(exceptionFactory);
                return parameter;
            }

            if (parameter.Equals(default(T)))
                Throw.CustomException(exceptionFactory);
            return parameter;
        }
    }
}