using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// Provides static factory methods that throw default exceptions.
    /// </summary>
    public static class Throw
    {
        /// <summary>
        /// Throws the default <see cref="ArgumentNullException" />, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeNull(string parameterName = null, string message = null) =>
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

        /// <summary>
        /// Throws the default <see cref="ArgumentDefaultException" /> indicating that a value is the default value of its type, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeDefault(string parameterName = null, string message = null) =>
            throw new ArgumentDefaultException(parameterName, message ?? $"{parameterName ?? "The value"} must not be the default value.");

        /// <summary>
        /// Throws the default <see cref="TypeCastException" /> indicating that a reference cannot be downcasted, using the optional parameter name and message.
        /// </summary>
        public static void InvalidTypeCast(object parameter, Type targetType, string parameterName = null, string message = null) =>
            throw new TypeCastException(parameterName, message ?? $"{parameterName ?? "The value"} \"{parameter}\" cannot be casted to \"{targetType}\".");

        /// <summary>
        /// Throws the default <see cref="EnumValueNotDefinedException" /> indicating that a value is not one of the constants defined in an enum, using the optional paramter name and message.
        /// </summary>
        public static void EnumValueNotDefined<T>(T parameter, string parameterName = null, string message = null) =>
            throw new EnumValueNotDefinedException(parameterName, message ?? $"{parameterName ?? "The value"} \"{parameter}\" must be one of the defined constants of enum \"{parameter.GetType()}\", but it is not.");

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException(Func<Exception> exceptionFactory) => throw exceptionFactory();

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="parameter" /> is passed to <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException<T>(Func<T, Exception> exceptionFactory, T parameter) => throw exceptionFactory(parameter);
    }
}