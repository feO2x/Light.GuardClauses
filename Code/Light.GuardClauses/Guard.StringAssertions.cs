using System;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif
using Light.GuardClauses.Exceptions;
using JetBrains.Annotations;

namespace Light.GuardClauses
{
    public static partial class Guard
    {
        /// <summary>
        /// Checks if the specified string is null or empty.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("string:null => false")]
        public static bool IsNullOrEmpty(this string @string) => string.IsNullOrEmpty(@string);

        /// <summary>
        /// Ensures that the specified string is not null or empty, or otherwise throws an <see cref="ArgumentNullException"/> or <see cref="EmptyStringException"/>.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="EmptyStringException" /> or the <see cref="ArgumentNullException"/> (optional).</param>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotBeNullOrEmpty(this string parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                Throw.ArgumentNull(parameterName, message);
            if (parameter.Length == 0)
                Throw.EmptyString(parameterName, message);

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified string is not null or empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">Thrown when <paramref name="parameter"/> is an empty string or null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static string MustNotBeNullOrEmpty(this string parameter, Func<string, Exception> exceptionFactory)
        {
            if (parameter.IsNullOrEmpty())
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified string is null, empty, or contains only white space.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("string:null => false")]
        public static bool IsNullOrWhiteSpace(this string @string)
#if NET35
        {
            if (string.IsNullOrEmpty(@string))
                return true;

            foreach (var character in @string)
            {
                if (!char.IsWhiteSpace(character))
                    return false;
            }

            return true;
        }
#else
        => string.IsNullOrWhiteSpace(@string);
#endif

        /// <summary>
        /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws an <see cref="ArgumentNullException"/>, an <see cref="EmptyStringException"/>, or a <see cref="WhiteSpaceStringException"/>.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="WhiteSpaceStringException"/>, the <see cref="EmptyStringException" />, or the <see cref="ArgumentNullException"/> (optional).</param>
        /// <exception cref="WhiteSpaceStringException">Thrown when <paramref name="parameter"/> contains only white space.</exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotBeNullOrWhiteSpace(this string parameter, string parameterName = null, string message = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName, message);

            foreach (var character in parameter)
            {
                if (!char.IsWhiteSpace(character))
                    return parameter;
            }

            Throw.WhiteSpaceString(parameter, parameterName, message);
            return null;
        }

        /// <summary>
        /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null, empty, or contains only white space.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory: null => halt")]
        public static string MustNotBeNullOrWhiteSpace(this string parameter, Func<string, Exception> exceptionFactory)
        {
            parameter.MustNotBeNullOrEmpty(exceptionFactory);

            foreach (var character in parameter)
            {
                if (!char.IsWhiteSpace(character))
                    return parameter;
            }

            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }

    }
}
