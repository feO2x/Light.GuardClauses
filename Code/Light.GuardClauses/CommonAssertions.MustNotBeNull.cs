
// PLEASE NOTICE: this code is auto-generated. Any changes you make to this file are going to be overwritten when this file is recreated by the code generator.
// Check "CommonAssertions.MustNotBeNull.csx" for details.

using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    public static partial class CommonAssertions
    {
        /// <summary>
        /// Ensures that the specified parameter is not null, or otherwise throws an <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="parameter">The reference to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException"/> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null..</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }
        
        /// <summary>
        /// Ensures that the specified parameter is not null, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The reference to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null..</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, Func<Exception> exceptionFactory) where T : class
        {
            if (parameter == null)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }
        
    }
}
