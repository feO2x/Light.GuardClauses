using System;
using System.Collections.Generic;
using System.Reflection;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The <see cref="CommonAssertions" /> class contains the most common assertions like <see cref="MustNotBeNull{T}" /> and assertions that are not directly related to
    ///     any categories like collection assertions or string assertions.
    /// </summary>
    public static class CommonAssertions
    {
        /// <summary>
        ///     Ensures that the specified parameter is not null, or otherwise throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is null (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the specified parameter is null and no <paramref name="exception" /> is specified.
        /// </exception>
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (parameter != null)
                return parameter;

            throw exception?.Invoke() ?? new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /// <summary>
        ///     Ensures that the specified parameter is not the default value, or otherwise throws an <see cref="ArgumentNullException" />
        ///     for reference types, or an <see cref="ArgumentException" /> for value types.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException" /> or <see cref="ArgumentException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is the default value (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is the default value of its type.</exception>
        public static T MustNotBeDefault<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (default(T) == null)
            {
                if (parameter == null)
                    throw exception?.Invoke() ?? new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
                return parameter;
            }
            if (parameter.Equals(default(T)))
                throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be the default value.");

            return parameter;
        }

        /// <summary>
        ///     Ensures that the specified parameter is null, or otherwise throws an <see cref="ArgumentNotNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentNotNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not null (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNotNullException">
        ///     Thrown when the specified parameter is not null and no <paramref name="exception" /> is specified.
        /// </exception>
        public static T MustBeNull<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (parameter == null)
                return null;

            throw exception?.Invoke() ?? (message == null ? new ArgumentNotNullException(parameterName, parameter) : new ArgumentNotNullException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that parameter is of the specified type and returns the downcasted value, or otherwise throws a <see cref="TypeMismatchException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeMismatchException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> cannot be downcasted (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="TypeMismatchException">
        ///     Thrown when the specified <paramref name="parameter" /> cannot be downcasted and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <returns>The downcasted reference to <paramref name="parameter" />.</returns>
        public static T MustBeOfType<T>(this object parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (parameter is T castedValue)
                return castedValue;

            throw exception?.Invoke() ?? new TypeMismatchException(message ?? $"{parameterName ?? "The object"} is of type {parameter.GetType().FullName} and cannot be downcasted to {typeof(T).FullName}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified Nullable has a value, or otherwise throws a <see cref="NullableHasNoValueException" />.
        /// </summary>
        /// <typeparam name="T">The type of the struct encapsulated by the Nullable.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="NullableHasNoValueException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified Nullable has no value (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="NullableHasNoValueException">
        ///     Thrown when the specified nullable has no value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static T? MustHaveValue<T>(this T? parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : struct
        {
            if (parameter.HasValue)
                return parameter;

            throw exception?.Invoke() ?? (message != null ? new NullableHasNoValueException(message, parameterName) : new NullableHasNoValueException(parameterName));
        }

        /// <summary>
        ///     Ensures that the specified Nullable has no value, or otherwise throws a <see cref="NullableHasValueException" />.
        /// </summary>
        /// <typeparam name="T">The type of the struct encapsulated by the Nullable.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="NullableHasValueException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified Nullable has a value (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="NullableHasValueException">
        ///     Thrown when the specified nullable has a value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static T? MustNotHaveValue<T>(this T? parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : struct
        {
            if (parameter.HasValue == false)
                return null;

            throw exception?.Invoke() ?? (message == null ? new NullableHasValueException(parameterName, parameter.Value) : new NullableHasValueException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that the specified value is defined in its corresponding enum type, or otherwise throws a <see cref="EnumValueNotDefinedException" />.
        /// </summary>
        /// <typeparam name="T">The enum type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="EnumValueNotDefinedException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified Nullable has a value (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="EnumValueNotDefinedException">
        ///     Thrown when the specified enum value is not defined and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is not a value of an enum.</exception>
        public static T MustBeValidEnumValue<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.IsValidEnumValue())
                return parameter;

            throw exception?.Invoke() ?? (message == null ? new EnumValueNotDefinedException(parameterName, parameter, typeof(T)) : new EnumValueNotDefinedException(message, parameterName));
        }

        /// <summary>
        ///     Checks if the specified value is a valid enum value of its type.
        /// </summary>
        /// <param name="parameter">The enum value to be checked.</param>
        /// <returns>True if the specified value is a valid value of an enum type, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        public static bool IsValidEnumValue(this object parameter)
        {
            var enumType = parameter.MustNotBeNull(nameof(parameter)).GetType();
            var typeInfo = enumType.GetTypeInfo();
            if (typeInfo.IsEnum == false)
                throw new ArgumentException($"The specified type \"{typeInfo}\" is not an enum.");

            var fields = typeInfo.DeclaredFields.AsReadOnlyList();

            // If enum does not have the flags attribute, then just get all fields via reflection and check if one is equal to the given value
            if (typeInfo.GetCustomAttribute(Types.FlagsAttributeType) == null)
            {
                for (var i = 0; i < fields.Count; i++)
                {
                    var field = fields[i];
                    if (field.IsStatic &&
                        field.IsLiteral &&
                        field.GetValue(null).Equals(parameter))
                        return true;
                }
                return false;
            }

            // Else get all values 
            var enumInfo = GetEnumInfo(fields);
            if (enumInfo.NumberOfConstants == 0)
                return false;

            return enumInfo.UnderlyingType == Types.UInt64Type ? CheckFlagsEnumValueUsingUInt64Conversion(Convert.ToUInt64(parameter), enumInfo.NumberOfConstants, fields) : CheckFlagsEnumValueUsingInt64Conversion(Convert.ToInt64(parameter), enumInfo.NumberOfConstants, fields);
        }

        private static bool CheckFlagsEnumValueUsingInt64Conversion(long parameter, int numberOfConstants, IReadOnlyList<FieldInfo> fields)
        {
            var array = new long[numberOfConstants];
            var currentStartIndex = default(int);

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (field.IsStatic && field.IsLiteral)
                {
                    array[currentStartIndex++] = Convert.ToInt64(field.GetValue(null));
                }
            }
            Array.Sort(array);
            var foundIndex = Array.BinarySearch(array, parameter);
            if (foundIndex >= 0 && foundIndex < array.Length)
                return true;

            var bit = 1L;
            if (parameter < bit)
                return false;

            currentStartIndex = 0;

            while (true)
            {
                if ((bit & parameter) != 0)
                {
                    currentStartIndex = UpdateIndexIfNecessary(bit, currentStartIndex, array);
                    if (currentStartIndex == -1)
                        return false;

                    if (FindTarget(bit, currentStartIndex, array) == false)
                        return false;
                }

                var newBit = bit << 1;
                if (newBit > parameter || newBit < bit)
                    break;

                bit = newBit;
            }

            return true;

            int UpdateIndexIfNecessary(long currentBit, int index, long[] sortedEnumValues)
            {
                var value = sortedEnumValues[index];
                while (value < currentBit)
                {
                    if (++index >= sortedEnumValues.Length)
                        return -1;

                    value = sortedEnumValues[index];
                }

                return index;
            }

            bool FindTarget(long currentBit, int startingIndex, long[] sortedEnumValues)
            {
                for (var i = startingIndex; i < sortedEnumValues.Length; i++)
                {
                    var enumValue = sortedEnumValues[i];
                    if ((enumValue & currentBit) != -1)
                        return true;
                }
                return false;
            }
        }

        private static bool CheckFlagsEnumValueUsingUInt64Conversion(ulong parameter, int numberOfConstants, IReadOnlyList<FieldInfo> fields)
        {
            var array = new ulong[numberOfConstants];
            var currentIndex = default(int);

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (field.IsStatic && field.IsLiteral)
                {
                    array[currentIndex++] = (ulong) field.GetValue(null);
                }
            }
            Array.Sort(array);
            if (Array.BinarySearch(array, parameter) != -1)
                return true;

            var bit = 1ul;
            currentIndex = 0;

            while (true)
            {
                var isActivated = (bit & parameter) != 0;
            }
        }

        private static EnumInfo GetEnumInfo(IReadOnlyList<FieldInfo> fields)
        {
            var numberOfConstants = default(int);
            var underlyingType = default(Type);
            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (!field.IsStatic || !field.IsLiteral)
                    continue;
                if (underlyingType == null)
                    underlyingType = field.FieldType;
                numberOfConstants++;
            }

            return new EnumInfo(numberOfConstants, underlyingType);
        }

        /// <summary>
        ///     Ensures that the specified GUID is not empty, or otherwise throws an <see cref="EmptyGuidException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="EmptyGuidException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified GUID is empty (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="EmptyGuidException">
        ///     Thrown when the specified GUID is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        public static Guid MustNotBeEmpty(this Guid parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter != Guid.Empty)
                return parameter;
            throw exception?.Invoke() ?? (message == null ? new EmptyGuidException(parameterName) : new EmptyGuidException(message, parameterName));
        }

        /// <summary>
        ///     Checks if the specified GUID is an empty one.
        /// </summary>
        /// <param name="parameter">The GUID to be checked.</param>
        /// <returns>True if the GUID is empty, else false.</returns>
        public static bool IsEmpty(this Guid parameter)
        {
            return parameter == Guid.Empty;
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is false, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified bool is true (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is true and no <paramref name="exception" /> is specified.</exception>
        public static bool MustBeFalse(this bool parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter == false)
                return false;

            throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The value"} must be false, but you specified true.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is true, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified bool is false (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is false and no <paramref name="exception" /> is specified.</exception>
        public static bool MustBeTrue(this bool parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter)
                return true;

            throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The value"} must be true, but you specified false.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is not false, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified bool is false (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is true and no <paramref name="exception" /> is specified.</exception>
        public static bool MustNotBeFalse(this bool parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter) return true;

            throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be false, but you specified false.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified Boolean value is not true, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="parameter">The paramter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified bool is true (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> is false and no <paramref name="exception" /> is specified.</exception>
        public static bool MustNotBeTrue(this bool parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter == false) return false;

            throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be true, but you specified true.", parameterName);
        }

        private struct EnumInfo
        {
            public readonly int NumberOfConstants;
            public readonly Type UnderlyingType;

            public EnumInfo(int numberOfConstants, Type underlyingType)
            {
                NumberOfConstants = numberOfConstants;
                UnderlyingType = underlyingType;
            }
        }
    }
}