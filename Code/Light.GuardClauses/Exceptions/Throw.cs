using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     Provides static methods that throw default exceptions.
    /// </summary>
    public abstract class Throw
    {
        /// <summary>
        ///     Throws the default <see cref="System.ArgumentNullException" />, using the optional parameter name and message.
        /// </summary>
        public static void ArgumentNullException(string parameterName = null, string message = null)
        {
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        /// <summary>
        ///     Throws the default <see cref="ArgumentException" /> indicating that a value is the default value of its type, using
        ///     the optional parameter name and message.
        /// </summary>
        public static void ArgumentDefaultException(string parameterName = null, string message = null)
        {
            throw new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be the default value.", parameterName);
        }

        /// <summary>
        ///     Throws the default <see cref="ArgumentNotNullException" /> indicating that a value is not null, using the optional
        ///     parameter name and message.
        /// </summary>
        public static void ArgumentNotNullException(object value, string parameterName = null, string message = null)
        {
            throw new ArgumentNotNullException(parameterName, message ?? $"{parameterName ?? "The value"} must be null, but it actually is \"{value}\".");
        }

        /// <summary>
        ///     Throws the default <see cref="TypeMismatchException" /> indicating that a reference cannot be downcasted, using the
        ///     optional parameter name and message.
        /// </summary>
        public static void TypeMismatchException(object parameter, Type targetType, string parameterName = null, string message = null)
        {
            throw new TypeMismatchException(parameterName, message ?? $"{parameterName ?? "The object"} \"{parameter}\" cannot be downcasted to \"{targetType}\".");
        }

        /// <summary>
        ///     Throws the default <see cref="NullableHasNoValueException" /> indicating that a <see cref="Nullable{T}" /> has no
        ///     value, using the optional parameter name and message.
        /// </summary>
        public static void NullableHasNoValueException(string parameterName = null, string message = null)
        {
            throw new NullableHasNoValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have a value, but it actually is null.");
        }

        /// <summary>
        /// Throws the default <see cref="NullableHasValueException"/> indicating that a <see cref="Nullable{T}"/> has a value, using the optional paramter name and message.
        /// </summary>
        public static void NullableHasValueException<T>(T value, string parameterName = null, string message = null) where T : struct
        {
            throw new NullableHasValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have no value, but it actually is \"{value}\".");
        }

        /// <summary>
        ///     Throws the exception that is returned by <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException(Func<Exception> exceptionFactory)
        {
            throw exceptionFactory();
        }

        /// <summary>
        ///     Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="value" /> is passed
        ///     to <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException<T>(Func<T, Exception> exceptionFactory, T value)
        {
            throw exceptionFactory(value);
        }
    }
}