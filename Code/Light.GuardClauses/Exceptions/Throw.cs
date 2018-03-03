using System;
using System.Collections.Generic;
using System.Text;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// Provides static methods that throw default exceptions.
    /// </summary>
    public static class Throw
    {
        /// <summary>
        /// Throws the default <see cref="ArgumentNullException" />, using the optional parameter name and message.
        /// </summary>
        public static void ArgumentNull(string parameterName = null, string message = null) =>
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

        /// <summary>
        /// Throws the default <see cref="ArgumentDefaultException" /> indicating that a value is the default value of its type, using the optional parameter name and message.
        /// </summary>
        public static void ArgumentDefault(string parameterName = null, string message = null) =>
            throw new ArgumentDefaultException(parameterName, message ?? $"{parameterName ?? "The value"} must not be the default value.");

        /// <summary>
        /// Throws the default <see cref="TypeMismatchException" /> indicating that a reference cannot be downcasted, using the optional parameter name and message.
        /// </summary>
        public static void TypeMismatch(object parameter, Type targetType, string parameterName = null, string message = null) =>
            throw new TypeMismatchException(parameterName, message ?? $"{parameterName ?? "The object"} \"{parameter}\" cannot be downcasted to \"{targetType}\".");

        /// <summary>
        /// Throws the default <see cref="NullableHasNoValueException" /> indicating that a <see cref="Nullable{T}" /> has no value, using the optional parameter name and message.
        /// </summary>
        public static void NullableHasNoValue(string parameterName = null, string message = null) =>
            throw new NullableHasNoValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have a value, but it actually is null.");

        /// <summary>
        /// Throws the default <see cref="NullableHasValueException" /> indicating that a <see cref="Nullable{T}" /> has a value, using the optional paramter name and message.
        /// </summary>
        public static void NullableHasValue<T>(T value, string parameterName = null, string message = null) where T : struct =>
            throw new NullableHasValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have no value, but it actually is \"{value}\".");

        /// <summary>
        /// Throws the default <see cref="EnumValueNotDefinedException" /> indicating that a value is not one of the constants defined in an enum, using the optional paramter name and message.
        /// </summary>
        public static void EnumValueNotDefined<T>(T parameter, string parameterName = null, string message = null) =>
            throw new EnumValueNotDefinedException(parameterName, message ?? $"{parameterName ?? "The value"} \"{parameter}\" must be one of the defined constants of enum \"{parameter.GetType()}\", but it is not.");

        /// <summary>
        /// Throws the default <see cref="EmptyGuidException" /> indicating that a GUID is empty, using the optional parameter name and message.
        /// </summary>
        public static void EmptyGuid(string parameterName = null, string message = null) =>
            throw new EmptyGuidException(parameterName, message ?? $"{parameterName ?? "The value"} must be a valid GUID, but it actually is an empty one.");

        /// <summary>
        /// Throws the default <see cref="ArgumentException" /> indicating that a boolean is true instead of false, using the optional parameter name and message.
        /// </summary>
        public static void BooleanTrue(string parameterName = null, string message = null) =>
            throw new ArgumentException(message ?? $"{parameterName ?? "The boolean value"} must be false, but it actually is true.", parameterName);

        /// <summary>
        /// Throws the default <see cref="ArgumentException" /> indicating that a boolean is false instead of true, using the optional parameter name and message.
        /// </summary>
        public static void BooleanFalse(string parameterName = null, string message = null) =>
            throw new ArgumentException(message ?? $"{parameterName ?? "The boolean value"} must be true, but it actually is false.", parameterName);

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be less than the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeLessThan<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be less than {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be less than the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustBeLessThan<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be less than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeLessThanOrEqualTo<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be less than or equal to {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be less than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustBeLessThanOrEqualTo<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than or equal to {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be greater than the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeGreaterThan<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be greater than {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be greater than the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustBeGreaterThan<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be greater than {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be greater than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeGreaterThanOrEqualTo<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be greater than or equal to {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be greater than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        public static void MustBeGreaterThanOrEqualTo<T>(T parameter, T boundary, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be greater than or equal to {boundary}, but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentException" /> indicating that a value is no enum value (i.e. its type is no enum type), using the optional parameter name and message.
        /// </summary>
        public static void NoEnumValue<T>(T parameter, string parameterName = null, string message = null) where T : struct, IFormattable, IComparable
#if !NETSTANDARD1_0
    , IConvertible
#endif
            => throw new ArgumentException(message ?? $"The value \"{parameter}\" is no enum value.", parameterName);

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a value is not within a specified range, using the optional parameter name and message.
        /// </summary>
        public static void MustBeInRange<T>(T parameter, Range<T> range, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be between {range.From} ({(range.IsFromInclusive ? "inclusive" : "exclusive")}) and {range.To} ({(range.IsToInclusive ? "inclusive" : "exclusive")}), but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a value is within a specified range, using the optional parameter name and message.
        /// </summary>
        public static void MustNotBeInRange<T>(T parameter, Range<T> range, string parameterName = null, string message = null) where T : IComparable<T> =>
            throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be between {range.From} ({(range.IsFromInclusive ? "inclusive" : "exclusive")}) and {range.To} ({(range.IsToInclusive ? "inclusive" : "exclusive")}), but it actually is {parameter}.");

        /// <summary>
        /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using <see cref="DateTimeKind.Utc" />, using the optional parameter name and message.
        /// </summary>
        public static void MustBeUtcDateTime(DateTime parameter, string parameterName = null, string message = null) =>
            throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} \"{parameter:O}\" must use kind {DateTimeKind.Utc}, but it actually uses {parameter.Kind}.");

        /// <summary>
        /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using <see cref="DateTimeKind.Local" />, using the optional parameter name and message.
        /// </summary>
        public static void MustBeLocalDateTime(DateTime parameter, string parameterName = null, string message = null) =>
            throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} \"{parameter:O}\" must use kind {DateTimeKind.Local}, but it actually uses {parameter.Kind}.");

        /// <summary>
        /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using <see cref="DateTimeKind.Unspecified" />, using the optional parameter name and message.
        /// </summary>
        public static void MustBeUnspecifiedDateTime(DateTime parameter, string parameterName = null, string message = null) =>
            throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} \"{parameter:O}\" must use kind {DateTimeKind.Unspecified}, but it actually uses {parameter.Kind}.");

        /// <summary>
        /// Throws the default <see cref="RelativeUriException" /> indicating that a URI is relative instead of absolute, using the optional parameter name and message.
        /// </summary>
        public static void MustBeAbsoluteUri(Uri uri, string parameterName = null, string message = null) =>
            throw new RelativeUriException(parameterName, message ?? $"{parameterName ?? "The URI"} must be an absolute URI, but it actually is \"{uri}\".");

        /// <summary>
        /// Throws the default <see cref="InvalidUriSchemeException" /> indicating that a URI has an unexpected scheme, using the optional parameter name and message.
        /// </summary>
        public static void UriMustHaveScheme(Uri uri, string scheme, string parameterName = null, string message = null) =>
            throw new InvalidUriSchemeException(parameterName, message ?? $"{parameterName ?? "The URI"} must use the scheme \"{scheme}\", but it actually is \"{uri}\".");

        /// <summary>
        /// Throws the default <see cref="InvalidUriSchemeException" /> indicating that a URI does not use one of a set of expected schemes, using the optional parameter name and message.
        /// </summary>
        public static void UriMustHaveOneSchemeOf(Uri uri, IEnumerable<string> schemes, string parameterName = null, string message = null) =>
            throw new InvalidUriSchemeException(parameterName,
                                                message ??
                                                new StringBuilder().Append($"{parameterName ?? "The URI"} must use one of the following schemes:")
                                                                   .AppendItemsWithNewLine(schemes)
                                                                   .Append($"but it actually is \"{uri}\".")
                                                                   .ToString());

        /// <summary>
        /// Throws a <see cref="InvalidOperationException" /> using the optional message.
        /// </summary>
        public static void InvalidOperation(string message = null) => throw new InvalidOperationException(message);

        /// <summary>
        /// Throws a <see cref="InvalidStateException" /> using the optional message.
        /// </summary>
        public static void InvalidState(string message = null) => throw new InvalidStateException(message);

        /// <summary>
        /// Throws the default <see cref="TypeException"/> indicating that two types are not equivalent, using the optional parameter name and message.
        /// </summary>
        public static void TypesNotEquivalent(Type parameter, Type other, string parameterName = null, string message = null) =>
            throw new TypeException(parameterName, message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be equivalent to \"{other.ToStringOrNull()}\", but it actually is not.");


        /// <summary>
        /// Throws the default <see cref="TypeException"/> indicating that two types are equivalent, using the optional parameter name and message.
        /// </summary>
        public static void TypesEquivalent(Type parameter, Type other, string parameterName = null, string message = null) =>
            throw new TypeException(parameterName, message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be equivalent to \"{other?.ToString() ?? "null"}\", but it actually is.");
        

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException(Func<Exception> exceptionFactory) => throw exceptionFactory();

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="parameter" /> is passed to <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException<T>(Func<T, Exception> exceptionFactory, T parameter) => throw exceptionFactory(parameter);

        /// <summary>
        /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="parameter" /> and <paramref name="other" /> are passed to <paramref name="exceptionFactory" />.
        /// </summary>
        public static void CustomException<T1, T2>(Func<T1, T2, Exception> exceptionFactory, T1 parameter, T2 other) => throw exceptionFactory(parameter, other);
    }
}