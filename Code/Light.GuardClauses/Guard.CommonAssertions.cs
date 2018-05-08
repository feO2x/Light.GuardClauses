using System;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif

namespace Light.GuardClauses
{
    /// <summary>
    /// The <see cref="Guard" /> class provides access to all assertions of Light.GuardClauses.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <param name="parameter">The object reference to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentNullException" /> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
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
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is a reference type and null.</exception>
        /// <exception cref="ArgumentDefaultException">Thrown when <paramref name="parameter" /> is a value type and the default value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
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
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is the default value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
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

        /// <summary>
        /// Ensures that the specified parameter is not null when <typeparamref name="T" /> is a reference type, or otherwise
        /// throws an <see cref="ArgumentNullException" />. PLEASE NOTICE: you should only use this assertion in generic contexts,
        /// use <see cref="MustNotBeNull{T}(T,string,string)" /> by default.
        /// </summary>
        /// <param name="parameter">The value to be checked for null.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentNullException" /> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNullReference<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) != null)
                return parameter;

            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified parameter is not null when <typeparamref name="T" /> is a reference type, or otherwise
        /// throws your custom exception. PLEASE NOTICE: you should only use this assertion in generic contexts,
        /// use <see cref="MustNotBeNull{T}(T,Func{Exception})" /> by default.
        /// </summary>
        /// <param name="parameter">The value to be checked for null.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNullReference<T>(this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T) != null)
                return parameter;

            if (parameter == null)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter"/> can be casted to <typeparamref name="T"/> and returns the casted value, or otherwise throws a <see cref="TypeCastException"/>.
        /// </summary>
        /// <param name="parameter">The value to be casted.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="TypeCastException" /> (optional).</param>
        /// <exception cref="TypeCastException">Thrown when <paramref name="parameter"/> cannot be casted to <typeparamref name="T"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeOfType<T>(this object parameter, string parameterName = null, string message = null)
        {
            if (parameter.MustNotBeNull(parameterName) is T castedValue)
                return castedValue;

            Throw.InvalidTypeCast(parameter, typeof(T), parameterName, message);
            return default;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter"/> can be casted to <typeparamref name="T"/> and returns the casted value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be casted.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the argument-null-check.</param>
        /// <exception cref="TypeCastException">Thrown when <paramref name="parameter"/> cannot be casted to <typeparamref name="T"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeOfType<T>(this object parameter, Func<Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustNotBeNull(parameterName) is T castedValue)
                return castedValue;

            Throw.CustomException(exceptionFactory);
            return default;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter"/> can be casted to <typeparamref name="T"/> and returns the casted value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be casted.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the argument-null-check.</param>
        /// <exception cref="TypeCastException">Thrown when <paramref name="parameter"/> cannot be casted to <typeparamref name="T"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeOfType<T>(this object parameter, Func<object, Exception> exceptionFactory, string parameterName = null)
        {
            if (parameter.MustNotBeNull(parameterName) is T castedValue)
                return castedValue;

            Throw.CustomException(exceptionFactory, parameter);
            return default;
        }

        /// <summary>
        /// Checks if the specified value is a valid enum value of its type. This is true when the specified value
        /// is one of the constants defined in the enum, or a valid flags combination when the enum type is marked
        /// with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="parameter">The enum value to be checked.</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsValidEnumValue<T>(this T parameter) where T : struct, IComparable, IFormattable
#if !NETSTANDARD1_0
          , IConvertible
#endif
            => EnumInfo<T>.IsValidEnumValue(parameter);

        /// <summary>
        /// Ensures that the specified enum value is valid, or otherwise throws an <see cref="EnumValueNotDefinedException"/>. An enum value
        /// is valid when the specified value  is one of the constants defined in the enum, or a valid flags combination when the enum type
        /// is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="EnumValueNotDefinedException" /> (optional).</param>
        /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="parameter"/> is no valid enum value.</exception>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="T"/> is no enum type.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustBeValidEnumValue<T>(this T parameter, string parameterName = null, string message = null) where T : struct, IComparable, IFormattable
#if !NETSTANDARD1_0
, IConvertible
#endif
        {
            if (!EnumInfo<T>.IsValidEnumValue(parameter))
                Throw.EnumValueNotDefined(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified enum value is valid, or otherwise throws your custom exception. An enum value
        /// is valid when the specified value  is one of the constants defined in the enum, or a valid flags combination when the enum type
        /// is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is no valid enum value.</exception>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="T"/> is no enum type.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustBeValidEnumValue<T>(this T parameter, Func<Exception> exceptionFactory) where T : struct, IComparable, IFormattable
#if !NETSTANDARD1_0
, IConvertible
#endif
        {
            if (!EnumInfo<T>.IsValidEnumValue(parameter))
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified enum value is valid, or otherwise throws your custom exception. An enum value
        /// is valid when the specified value  is one of the constants defined in the enum, or a valid flags combination when the enum type
        /// is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is no valid enum value.</exception>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="T"/> is no enum type.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustBeValidEnumValue<T>(this T parameter, Func<T, Exception> exceptionFactory) where T : struct, IComparable, IFormattable
#if !NETSTANDARD1_0
, IConvertible
#endif
        {
            if (!EnumInfo<T>.IsValidEnumValue(parameter))
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }
    }
}