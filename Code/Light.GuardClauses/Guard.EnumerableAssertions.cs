using System;
using System.Collections;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection must have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidCollectionCountException" /> (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> does not have the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>(this TCollection parameter, int count, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter is ICollection collection)
            {
                if (collection.Count != count)
                    Throw.InvalidCollectionCount(parameter, count, parameterName, message);
                return parameter;
            }

            if (parameter is string @string)
            {
                if (@string.Length != count)
                    Throw.InvalidCollectionCount(parameter, count, parameterName, message);
                return parameter;
            }

            if (parameter.MustNotBeNull(parameterName).Count() != count)
                Throw.InvalidCollectionCount(parameter, count, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection must have.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not have the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>(this TCollection parameter, int count, Func<TCollection, int, Exception> exceptionFactory, string parameterName = null) where TCollection : class, IEnumerable
        {
            if (parameter is ICollection collection)
            {
                if (collection.Count != count)
                    Throw.CustomException(exceptionFactory, parameter, count);
                return parameter;
            }

            if (parameter is string @string)
            {
                if (@string.Length != count)
                    Throw.CustomException(exceptionFactory, parameter, count);
                return parameter;
            }

            if (parameter.MustNotBeNull(parameterName).Count() != count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter;
        }
    }
}
