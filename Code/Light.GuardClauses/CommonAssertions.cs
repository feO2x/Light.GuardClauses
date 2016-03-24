using System;
using System.Diagnostics;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     This class contains the most common assertions like <see cref="MustNotBeNull{T}" /> and assertions that are not directly related to
    ///     any categories like collection assertions or string assertions.
    /// </summary>
    public static class CommonAssertions
    {
        /// <summary>
        ///     Ensures that the specified parameter is not <c>null</c>, or otherwise throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentNullException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is <c>null</c> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the specified parameter is <c>null</c> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null, Exception exception = null) where T : class
        {
            if (parameter == null)
                throw exception ?? new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /// <summary>
        ///     Ensures that the specified parameter is <c>null</c>, or otherwise throws an <see cref="ArgumentNotNullException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentNotNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not <c>null</c> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNotNullException">
        ///     Thrown when the specified parameter is not <c>null</c> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeNull<T>(this T parameter, string parameterName = null, string message = null, Exception exception = null) where T : class
        {
            if (parameter != null)
                throw exception ?? (message == null ? new ArgumentNotNullException(parameterName, parameter) : new ArgumentNotNullException(message, parameterName));
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
        public static T MustBeOfType<T>(this object parameter, string parameterName = null, string message = null, Exception exception = null) where T : class
        {
            var castedValue = parameter as T;
            if (castedValue == null)
                throw exception ?? new TypeMismatchException(message ?? $"{parameterName ?? "The object"} is of type {parameter.GetType().FullName} and cannot be downcasted to {typeof (T).FullName}.", parameterName);

            return castedValue;
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveValue<T>(this T? parameter, string parameterName = null, string message = null, Exception exception = null) where T : struct
        {
            if (parameter.HasValue == false)
                throw exception ?? (message != null ? new NullableHasNoValueException(message, parameterName) : new NullableHasNoValueException(parameterName));
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotHaveValue<T>(this T? parameter, string parameterName = null, string message = null, Exception exception = null) where T : struct
        {
            if (parameter.HasValue)
                throw exception ?? (message == null ? new NullableHasValueException(parameterName, parameter.Value) : new NullableHasValueException(message, parameterName));
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeValidEnumValue<T>(this T parameter, string parameterName = null, string message = null, Exception exception = null)
        {
            var enumType = typeof (T);
            if (Enum.IsDefined(enumType, parameter) == false)
                throw exception ?? (message == null ? new EnumValueNotDefinedException(parameterName, parameter, enumType) : new EnumValueNotDefinedException(message, parameterName));
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeEmpty(this Guid parameter, string parameterName = null, string message = null, Exception exception = null)
        {
            if (parameter == Guid.Empty)
                throw exception ?? (message == null ? new EmptyGuidException(parameterName) : new EmptyGuidException(message, parameterName));
        }
    }
}