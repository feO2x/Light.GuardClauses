using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The <see cref="CommonAssertions" /> class contains the most common assertions like MustNotBeNull and assertions that are not directly related to
    ///     any categories like collection assertions or string assertions.
    /// </summary>
    public static class CommonAssertions
    {
        /// <summary>
        ///     Ensures that the specified parameter is not null, or otherwise throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The value to be checked  for null.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException" />.</param>
        /// <exception cref="ArgumentNullException"> Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter == null)
                Throw.ArgumentNull(parameterName, message);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified parameter is not null, or otherwise throws your custom exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The value to be checked for null.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNull<T>(this T parameter, Func<Exception> exceptionFactory) where T : class
        {
            if (parameter == null)
                Throw.CustomException(exceptionFactory);

            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified parameter is not the default value, or otherwise throws an <see cref="ArgumentNullException" />
        ///     for reference types, or an <see cref="ArgumentDefaultException" /> for value types.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException" /> or <see cref="ArgumentDefaultException" /> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentDefaultException">Thrown when <paramref name="parameter" /> is a value type and the default value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeDefault<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) == null)
            {
                if (parameter == null)
                    Throw.ArgumentNull(parameterName, message);
                return parameter;
            }

            if (parameter.Equals(default(T)))
                Throw.ArgumentDefault(parameterName, message);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified parameter is not the default value, or otherwise throws your custom exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception when <paramref name="parameter" /> is the default value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeDefault<T>(this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T) == null)
            {
                if (parameter == null)
                    Throw.CustomException(exceptionFactory);
                return default;
            }

            if (parameter.Equals(default(T)))
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified parameter is not null when <typeparamref name="T" /> is a reference type, or otherwise
        ///     throws an <see cref="ArgumentNullException" />. NOTICE: you should only use this assertion in generic contexts,
        ///     use <see cref="MustNotBeNull{T}(T,string,string)" /> by default.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException" /> (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullReference<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) != null)
                return parameter;

            if (parameter == null)
                Throw.ArgumentNull(parameterName, message);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified parameter is not null when <typeparamref name="T" /> is a reference type, or otherwise
        ///     throws your custom exception. NOTICE: you should only use this assertion in generic contexts,
        ///     use <see cref="MustNotBeNull{T}(T,Func{Exception})" /> by default.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullReference<T>(this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T) != null)
                return parameter;

            if (parameter == null)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        ///     Ensures that parameter is of the specified type and returns the downcasted value, or otherwise throws a <see cref="TypeMismatchException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="TypeMismatchException" /> (optional). </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeMismatchException">Thrown when <paramref name="parameter" /> cannot be downcasted to the specified value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeOfType<T>(this object parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter.MustNotBeNull(parameterName) is T castedValue)
                return castedValue;

            Throw.TypeMismatch(parameter, typeof(T), parameterName, message);
            return null;
        }

        /// <summary>
        ///     Ensures that parameter is of the specified type and returns the downcasted value, or otherwise throws your custom exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> cannot be downcasted to the specified value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeOfType<T>(this object parameter, Func<Exception> exceptionFactory, string parameterName = null) where T : class
        {
            if (parameter.MustNotBeNull(parameterName) is T castedValue)
                return castedValue;

            Throw.CustomException(exceptionFactory);
            return null;
        }

        /// <summary>
        ///     Ensures that parameter is of the specified type and returns the downcasted value, or otherwise throws your custom exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> cannot be downcasted to the specified value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeOfType<T>(this object parameter, Func<object, Exception> exceptionFactory, string parameterName = null) where T : class
        {
            if (parameter.MustNotBeNull(parameterName) is T castedValue)
                return castedValue;

            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }

        /// <summary>
        ///     Ensures that the specified nullable has a value, or otherwise throws a <see cref="NullableHasNoValueException" />.
        /// </summary>
        /// <typeparam name="T">The type of the struct encapsulated by the nullable.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="NullableHasNoValueException" /> (optional).</param>
        /// <exception cref="NullableHasNoValueException">Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustHaveValue<T>(this T? parameter, string parameterName = null, string message = null) where T : struct
        {
            if (parameter.HasValue == false)
                Throw.NullableHasNoValue(parameterName, message);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified nullable has a value, or otherwise throws your custom exception.
        /// </summary>
        /// <typeparam name="T">The type of the struct encapsulated by the nullable.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustHaveValue<T>(this T? parameter, Func<Exception> exceptionFactory) where T : struct
        {
            if (parameter.HasValue == false)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified nullable has no value, or otherwise throws a <see cref="NullableHasValueException" />.
        /// </summary>
        /// <typeparam name="T">The type of the struct encapsulated by the nullable.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="NullableHasNoValueException" /> (optional).</param>
        /// <exception cref="NullableHasValueException">Thrown when <paramref name="parameter" /> has a value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustNotHaveValue<T>(this T? parameter, string parameterName = null, string message = null) where T : struct
        {
            if (parameter.HasValue)
                Throw.NullableHasValue(parameter.Value, parameterName, message);
            return null;
        }

        /// <summary>
        ///     Ensures that the specified nullable has no value, or otherwise throws your custom exception.
        /// </summary>
        /// <typeparam name="T">The type of the struct encapsulated by the nullable.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> has a value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustNotHaveValue<T>(this T? parameter, Func<Exception> exceptionFactory) where T : struct
        {
            if (parameter.HasValue)
                Throw.CustomException(exceptionFactory);
            return null;
        }

        /// <summary>
        ///     Ensures that the specified nullable has no value, or otherwise throws your custom exception.
        /// </summary>
        /// <typeparam name="T">The type of the struct encapsulated by the nullable.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> has a value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustNotHaveValue<T>(this T? parameter, Func<T, Exception> exceptionFactory) where T : struct
        {
            if (parameter.HasValue)
                Throw.CustomException(exceptionFactory, parameter.Value);
            return null;
        }

        /// <summary>
        ///     Ensures that the specified value is defined in its corresponding enum type, or otherwise throws an <see cref="EnumValueNotDefinedException" />.
        /// </summary>
        /// <typeparam name="T">The enum type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="EnumValueNotDefinedException" /> (optional).</param>
        /// <exception cref="EnumValueNotDefinedException">Thrown when the specified enum value is not defined.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is not a value of an enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeValidEnumValue<T>(this T parameter, string parameterName = null, string message = null) where T : struct
#if !NETSTANDARD1_0
        , IConvertible
#endif
        {
            if (parameter.IsValidEnumValue() == false)
                Throw.EnumValueNotDefined(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified value is defined in its corresponding enum type, or otherwise throws an <see cref="EnumValueNotDefinedException" />.
        /// </summary>
        /// <typeparam name="T">The enum type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified enum value is not defined.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is not a value of an enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeValidEnumValue<T>(this T parameter, Func<Exception> exceptionFactory) where T : struct
#if !NETSTANDARD1_0
            , IConvertible
#endif
        {
            if (parameter.IsValidEnumValue() == false)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified value is defined in its corresponding enum type, or otherwise throws an <see cref="EnumValueNotDefinedException" />.
        /// </summary>
        /// <typeparam name="T">The enum type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified enum value is not defined.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is not a value of an enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeValidEnumValue<T>(this T parameter, Func<T, Exception> exceptionFactory) where T : struct
#if !NETSTANDARD1_0
            , IConvertible
#endif
        {
            if (parameter.IsValidEnumValue() == false)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        ///     Checks if the specified value is a valid enum value of its type.
        /// </summary>
        /// <param name="parameter">The enum value to be checked.</param>
        /// <returns>True if the specified value is a valid value of an enum type, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is not a value of an enum.</exception>
#if !NETSTANDARD1_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidEnumValue<T>(this T parameter) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                Throw.NoEnumValue(parameter);
            return EnumInfo<T>.IsFlagsEnum ? EnumInfo<T>.ValidateFlag(parameter) : EnumInfo<T>.ValidateConstant(parameter);
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidEnumValue<T>(this T parameter) where T : struct
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
                Throw.NoEnumValue(parameter);
            return EnumInfo<T>.IsFlagsEnum ? EnumInfo<T>.ValidateFlag(parameter) : EnumInfo<T>.ValidateConstant(parameter);
        }
#endif

        /// <summary>
        ///     Ensures that the specified GUID is not empty, or otherwise throws an <see cref="EmptyGuidException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="EmptyGuidException" /> (optional).</param>
        /// <exception cref="EmptyGuidException">Thrown when <paramref name="parameter" /> is an empty GUID.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid MustNotBeEmpty(this Guid parameter, string parameterName = null, string message = null)
        {
            if (parameter == Guid.Empty)
                Throw.EmptyGuid(parameterName, message);
            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified GUID is not empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is an empty GUID.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid MustNotBeEmpty(this Guid parameter, Func<Exception> exceptionFactory)
        {
            if (parameter == Guid.Empty)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        ///     Checks if the specified GUID is an empty one.
        /// </summary>
        /// <param name="parameter">The GUID to be checked.</param>
        /// <returns>True if the GUID is empty, else false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Guid parameter)
        {
            return parameter == Guid.Empty;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is false, or otherwise throws a <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustBeFalse(this bool parameter, string parameterName = null, string message = null)
        {
            if (parameter)
                Throw.BooleanTrue(parameterName, message);
            return false;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is false, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustBeFalse(this bool parameter, Func<Exception> exceptionFactory)
        {
            if (parameter)
                Throw.CustomException(exceptionFactory);
            return false;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is true, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is false.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustBeTrue(this bool parameter, string parameterName = null, string message = null)
        {
            if (!parameter)
                Throw.BooleanFalse(parameterName, message);
            return true;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is true, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is false.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustBeTrue(this bool parameter, Func<Exception> exceptionFactory)
        {
            if (!parameter)
                Throw.CustomException(exceptionFactory);
            return true;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is not false, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is false.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustNotBeFalse(this bool parameter, string parameterName = null, string message = null)
        {
            if (!parameter)
                Throw.BooleanFalse(parameterName, message);
            return true;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is not false, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is false.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustNotBeFalse(this bool parameter, Func<Exception> exceptionFactory)
        {
            if (!parameter)
                Throw.CustomException(exceptionFactory);
            return true;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is not true, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustNotBeTrue(this bool parameter, string parameterName = null, string message = null)
        {
            if (parameter)
                Throw.BooleanTrue(parameterName, message);
            return false;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is not true, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MustNotBeTrue(this bool parameter, Func<Exception> exceptionFactory)
        {
            if (parameter)
                Throw.CustomException(exceptionFactory);
            return false;
        }

        private readonly struct EnumInfo<T> where T : struct
#if !NETSTANDARD1_0
            , IConvertible
#endif
        {
#if !NETSTANDARD1_0
            public static readonly bool IsFlagsEnum = typeof(T).GetCustomAttribute(Types.FlagsAttributeType) != null;
#else
            public static readonly bool IsFlagsEnum = typeof(T).GetTypeInfo().GetCustomAttribute(Types.FlagsAttributeType) != null;
#endif

            // ReSharper disable StaticMemberInGenericType
            private static readonly long Int64FlagsPattern;
            private static readonly ulong UInt64FlagsPattern;
            private static readonly bool IsUInt64FlagsPattern;
            // ReSharper restore StaticMemberInGenericType

            private static readonly T[] EnumConstants;

            static EnumInfo()
            {
                if (IsFlagsEnum)
                {
#if !NETSTANDARD1_0
                    var fields = typeof(T).GetFields();
#else
                    var fields = typeof(T).GetTypeInfo().DeclaredFields.AsArray();
#endif

                    var underlyingType = fields[0].FieldType;
                    IsUInt64FlagsPattern = underlyingType == Types.UInt64Type;
                    var values = (T[]) Enum.GetValues(typeof(T));
                    for (var i = 0; i < values.Length; ++i)
                    {
                        if (IsUInt64FlagsPattern)
#if !NETSTANDARD1_0
                            UInt64FlagsPattern |= values[i].ToUInt64(null);
#else
                            UInt64FlagsPattern |= Convert.ToUInt64(values[i]);
#endif
                        else
#if !NETSTANDARD1_0
                            Int64FlagsPattern |= values[i].ToInt64(null);
#else
                            Int64FlagsPattern |= Convert.ToInt64(values[i]);
#endif
                    }
                }
                else
                    EnumConstants = (T[]) Enum.GetValues(typeof(T));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ValidateFlag(T enumValue) 
            {
                if (IsUInt64FlagsPattern)
                {
#if !NETSTANDARD1_0
                    var convertedUInt64Value = enumValue.ToUInt64(null);
#else
                    var convertedUInt64Value = Convert.ToUInt64(enumValue);
#endif
                    return (UInt64FlagsPattern & convertedUInt64Value) == convertedUInt64Value;
                }

#if !NETSTANDARD1_0
                var convertedInt64Value = enumValue.ToInt64(null);
#else
                var convertedInt64Value = Convert.ToInt64(enumValue);
#endif
                return (Int64FlagsPattern & convertedInt64Value) == convertedInt64Value;
            }

            public static bool ValidateConstant(T parameter)
            {
                var comparer = EqualityComparer<T>.Default;
                for (var i = 0; i < EnumConstants.Length; ++i)
                {
                    if (comparer.Equals(EnumConstants[i], parameter))
                        return true;
                }

                return false;
            }
        }

    }


}