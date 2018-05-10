using System;
using JetBrains.Annotations;

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
        [ContractAnnotation("=> halt")]
        public static void MustNotBeNull(string parameterName = null, string message = null) =>
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

        /// <summary>
        /// Throws the default <see cref="ArgumentDefaultException" /> indicating that a value is the default value of its type, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void MustNotBeDefault(string parameterName = null, string message = null) =>
            throw new ArgumentDefaultException(parameterName, message ?? $"{parameterName ?? "The value"} must not be the default value.");

        /// <summary>
        /// Throws the default <see cref="TypeCastException" /> indicating that a reference cannot be downcasted, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void InvalidTypeCast(object parameter, Type targetType, string parameterName = null, string message = null) =>
            throw new TypeCastException(parameterName, message ?? $"{parameterName ?? "The value"} \"{parameter}\" cannot be casted to \"{targetType}\".");

        /// <summary>
        /// Throws the default <see cref="TypeIsNoEnumException"/> indicating that a type is no enum type, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void TypeIsNoEnum(Type type, string parameterName = null, string message = null) => 
            throw new TypeIsNoEnumException(parameterName, message ?? $"{parameterName ?? "The type"} \"{type}\" must be an enum type, but it actually is not.");

        /// <summary>
        /// Throws the default <see cref="EnumValueNotDefinedException" /> indicating that a value is not one of the constants defined in an enum, using the optional paramter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void EnumValueNotDefined<T>(T parameter, string parameterName = null, string message = null) =>
            throw new EnumValueNotDefinedException(parameterName, message ?? $"{parameterName ?? "The value"} \"{parameter}\" must be one of the defined constants of enum \"{parameter.GetType()}\", but it actually is not.");

        /// <summary>
        /// Throws the default <see cref="EmptyGuidException" /> indicating that a GUID is empty, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void EmptyGuid(string parameterName = null, string message = null) =>
            throw new EmptyGuidException(parameterName, message ?? $"{parameterName ?? "The value"} must be a valid GUID, but it actually is an empty one.");

        /// <summary>
        /// Throws an <see cref="InvalidOperationException" /> using the optional message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void InvalidOperation(string message = null) => throw new InvalidOperationException(message);

        /// <summary>
        /// Throws an <see cref="InvalidStateException" /> using the optional message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void InvalidState(string message = null) => throw new InvalidStateException(message);

        /// <summary>
        /// Throws the default <see cref="NullableHasNoValueException" /> indicating that a <see cref="Nullable{T}" /> has no value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void NullableHasNoValue(string parameterName = null, string message = null) =>
            throw new NullableHasNoValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have a value, but it actually is null.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be less than the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void MustNotBeLessThan<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be less than {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be less than the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void MustBeLessThan<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void CustomException(Func<Exception> exceptionFactory) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))();

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="parameter" /> is passed to <paramref name="exceptionFactory" />.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void CustomException<T>(Func<T, Exception> exceptionFactory, T parameter) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(parameter);

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="first" /> and <paramref name="second"/> are passed to <paramref name="exceptionFactory" />.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void CustomException<T1, T2>(Func<T1, T2, Exception> exceptionFactory, T1 first, T2 second) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second);
    }
}