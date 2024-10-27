using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// Provides static factory methods that throw default exceptions.
/// </summary>
public static class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentNullException" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ArgumentNull(string? parameterName = null, string? message = null) =>
        throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

    /// <summary>
    /// Throws the default <see cref="ArgumentDefaultException" /> indicating that a value is the default value of its type, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ArgumentDefault(string? parameterName = null, string? message = null) =>
        throw new ArgumentDefaultException(parameterName, message ?? $"{parameterName ?? "The value"} must not be the default value.");

    /// <summary>
    /// Throws the default <see cref="TypeCastException" /> indicating that a reference cannot be downcast, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidTypeCast(object? parameter, Type targetType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new TypeCastException(parameterName, message ?? $"{parameterName ?? "The value"} {parameter.ToStringOrNull()} cannot be cast to \"{targetType}\".");

    /// <summary>
    /// Throws the default <see cref="EnumValueNotDefinedException" /> indicating that a value is not one of the constants defined in an enum, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void EnumValueNotDefined<T>(T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : Enum =>
        throw new EnumValueNotDefinedException(parameterName, message ?? $"{parameterName ?? "The value"} \"{parameter}\" must be one of the defined constants of enum \"{parameter.GetType()}\", but it actually is not.");

    /// <summary>
    /// Throws the default <see cref="EmptyGuidException" /> indicating that a GUID is empty, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void EmptyGuid(string? parameterName = null, string? message = null) =>
        throw new EmptyGuidException(parameterName, message ?? $"{parameterName ?? "The value"} must be a valid GUID, but it actually is an empty one.");

    /// <summary>
    /// Throws an <see cref="InvalidOperationException" /> using the optional message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidOperation(string? message = null) => throw new InvalidOperationException(message);

    /// <summary>
    /// Throws an <see cref="InvalidStateException" /> using the optional message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidState(string? message = null) => throw new InvalidStateException(message);

    /// <summary>
    /// Throws an <see cref="ArgumentException" /> using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void Argument(string? parameterName = null, string? message = null) =>
        throw new ArgumentException(message ?? $"{parameterName ?? "The value"} is invalid.", parameterName);

    /// <summary>
    /// Throws an <see cref="InvalidEmailAddressException"/> using the optional message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidEmailAddress(string parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidEmailAddressException(parameterName, message ?? $"{parameterName ?? "The string"} must be a valid email address, but it actually is \"{parameter}\".");

    /// <summary>
    /// Throws the default <see cref="NullableHasNoValueException" /> indicating that a <see cref="Nullable{T}" /> has no value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NullableHasNoValue(string? parameterName = null, string? message = null) =>
        throw new NullableHasNoValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have a value, but it actually is null.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be less than the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeLessThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be less than {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be less than the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeLessThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be less than or equal to the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeLessThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be less than or equal to {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be greater than or equal to the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeGreaterThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be greater than or equal to {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be greater than or equal to the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeGreaterThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be greater than or equal to {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be greater than the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeGreaterThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be greater than {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be greater than the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeGreaterThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be greater than {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must be less than or equal to the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeLessThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than or equal to {boundary}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a value is not within a specified range, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeInRange<T>(T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be between {range.CreateRangeDescriptionText("and")}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a value is within a specified range, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeInRange<T>(T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be between {range.CreateRangeDescriptionText("and")}, but it actually is {parameter}.");

    /// <summary>
    /// Throws the default <see cref="SameObjectReferenceException" /> indicating that two references point to the same object, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SameObjectReference<T>(T? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : class =>
        throw new SameObjectReferenceException(parameterName, message ?? $"{parameterName ?? "The reference"} must not point to object \"{parameter}\", but it actually does.");

    /// <summary>
    /// Throws the default <see cref="EmptyStringException" /> indicating that a string is empty, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void EmptyString(string? parameterName = null, string? message = null) =>
        throw new EmptyStringException(parameterName, message ?? $"{parameterName ?? "The string"} must not be an empty string, but it actually is.");

    /// <summary>
    /// Throws the default <see cref="WhiteSpaceStringException" /> indicating that a string contains only white space, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void WhiteSpaceString(string parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new WhiteSpaceStringException(parameterName, message ?? $"{parameterName ?? "The string"} must not contain only white space, but it actually is \"{parameter}\".");

    /// <summary>
    /// Throws the default <see cref="StringDoesNotMatchException" /> indicating that a string does not match a regular expression, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotMatch(string parameter, Regex regex, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new StringDoesNotMatchException(parameterName, message ?? $"{parameterName ?? "The string"} must match the regular expression \"{regex}\", but it actually is \"{parameter}\".");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string does not contain another string as a substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotContain(string parameter, string substring, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must contain {substring.ToStringOrNull()}, but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string does not contain another string as a substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotContain(string parameter, string substring, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must contain {substring.ToStringOrNull()} ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string does contain another string as a substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringContains(string parameter, string substring, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not contain {substring.ToStringOrNull()} as a substring, but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string does contain another string as a substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringContains(string parameter, string substring, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not contain {substring.ToStringOrNull()} as a substring ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string is not a substring of another one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotSubstring(string parameter, string other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must be a substring of \"{other}\", but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string is not a substring of another one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotSubstring(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must be a substring of \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string is a substring of another one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void Substring(string parameter, string other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not be a substring of \"{other}\", but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string is a substring of another one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void Substring(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not be a substring of \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
    
    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string does not start with another one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotStartWith(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must start with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
    
    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string does start with another one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringStartsWith(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not start with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
    
    /// <summary>
    /// Throws the default <see cref="SubstringException"/> indicating that a string does not end with another one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotEndWith(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must end with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="StringLengthException"/> indicating that a string is not shorter than the given length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotShorterThan(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be shorter than {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="StringLengthException"/> indicating that a string is not shorter or equal to the given length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotShorterThanOrEqualTo(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be shorter or equal to {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="StringLengthException"/> indicating that a string has a different length than the specified one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringLengthNotEqualTo(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must have length {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="StringLengthException"/> indicating that a string is not longer than the given length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotLongerThan(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be longer than {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="StringLengthException"/> indicating that a string is not longer or equal to the given length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotLongerThanOrEqualTo(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be longer than or equal to {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="StringLengthException"/> indicating that a string's length is not in between the given range, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringLengthNotInRange(string parameter, Range<int> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must have its length in between {range.CreateRangeDescriptionText("and")}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="StringException"/> indicating that a string is not equal to "\n" or "\r\n".
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotNewLine(string? parameter, string? parameterName, string? message) =>
        throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be either \"\\n\" or \"\\r\\n\", but it actually is {parameter.ToStringOrNull()}.");
    
    /// <summary>
    /// Throws the default <see cref="StringException"/> indicating that a string is not trimmed.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotTrimmed(string? parameter, string? parameterName, string? message) =>
        throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be trimmed, but it actually is {parameter.ToStringOrNull()}.");
    
    /// <summary>
    /// Throws the default <see cref="StringException"/> indicating that a string is not trimmed at the start.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotTrimmedAtStart(string? parameter, string? parameterName, string? message) =>
        throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be trimmed at the start, but it actually is {parameter.ToStringOrNull()}.");
    
    /// <summary>
    /// Throws the default <see cref="StringException"/> indicating that a string is not trimmed at the end.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotTrimmedAtEnd(string? parameter, string? parameterName, string? message) =>
        throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be trimmed at the end, but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="ValuesNotEqualException" /> indicating that two values are not equal, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ValuesNotEqual<T>(T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new ValuesNotEqualException(parameterName, message ?? $"{parameterName ?? "The value"} must be equal to {other.ToStringOrNull()}, but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="ValuesEqualException" /> indicating that two values are equal, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ValuesEqual<T>(T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new ValuesEqualException(parameterName, message ?? $"{parameterName ?? "The value"} must not be equal to {other.ToStringOrNull()}, but it actually is {parameter.ToStringOrNull()}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a collection has an invalid number of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidCollectionCount(IEnumerable parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The collection"} must have count {count}, but it actually has count {parameter.Count()}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span has an invalid length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidSpanLength<T>(in Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must have length {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span has an invalid length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidSpanLength<T>(in ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The read-only span"} must have length {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a collection has less than a minimum number of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidMinimumCollectionCount(IEnumerable parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The collection"} must have at least count {count}, but it actually has count {parameter.Count()}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not longer than the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeLongerThan<T>(in Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be longer than {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not longer than the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeLongerThan<T>(in ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be longer than {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not longer than and not equal to the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeLongerThanOrEqualTo<T>(in Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be longer than or equal to {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not longer than and not equal to the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeLongerThanOrEqualTo<T>(in ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be longer than or equal to {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not shorter than the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeShorterThan<T>(in Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be shorter than {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not shorter than the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeShorterThanOrEqualTo<T>(in Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be shorter than or equal to {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not shorter than the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeShorterThanOrEqualTo<T>(in ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be shorter than or equal to {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not shorter than the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeShorterThan<T>(in ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be shorter than {length}, but it actually has length {parameter.Length}.");

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a collection has more than a maximum number of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidMaximumCollectionCount(IEnumerable parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The collection"} must have at most count {count}, but it actually has count {parameter.Count()}.");

    /// <summary>
    /// Throws the default <see cref="EmptyCollectionException" /> indicating that a collection has no items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void EmptyCollection(string? parameterName = null, string? message = null) =>
        throw new EmptyCollectionException(parameterName, message ?? $"{parameterName ?? "The collection"} must not be an empty collection, but it actually is.");

    /// <summary>
    /// Throws the default <see cref="MissingItemException" /> indicating that a collection is not containing the specified item, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MissingItem<TItem>(IEnumerable<TItem> parameter, TItem item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new MissingItemException(parameterName,
                                       message ??
                                       new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must contain {item.ToStringOrNull()}, but it actually does not.")
                                                          .AppendCollectionContent(parameter)
                                                          .ToString());

    /// <summary>
    /// Throws the default <see cref="ExistingItemException" /> indicating that a collection contains the specified item that should not be part of it, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ExistingItem<TItem>(IEnumerable<TItem> parameter, TItem item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new ExistingItemException(parameterName,
                                        message ??
                                        new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must not contain {item.ToStringOrNull()}, but it actually does.")
                                                           .AppendCollectionContent(parameter)
                                                           .ToString());

    /// <summary>
    /// Throws the default <see cref="ValueIsNotOneOfException" /> indicating that a value is not one of a specified collection of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ValueNotOneOf<TItem>(TItem parameter, IEnumerable<TItem> items, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new ValueIsNotOneOfException(parameterName,
                                           message ??
                                           new StringBuilder().AppendLine($"{parameterName ?? "The value"} must be one of the following items")
                                                              .AppendItemsWithNewLine(items)
                                                              .AppendLine($"but it actually is {parameter.ToStringOrNull()}.")
                                                              .ToString());

    /// <summary>
    /// Throws the default <see cref="ValueIsOneOfException" /> indicating that a value is one of a specified collection of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ValueIsOneOf<TItem>(TItem parameter, IEnumerable<TItem> items, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new ValueIsOneOfException(parameterName,
                                        message ??
                                        new StringBuilder().AppendLine($"{parameterName ?? "The value"} must not be one of the following items")
                                                           .AppendItemsWithNewLine(items)
                                                           .AppendLine($"but it actually is {parameter.ToStringOrNull()}.")
                                                           .ToString());

    /// <summary>
    /// Throws the default <see cref="RelativeUriException" /> indicating that a URI is relative instead of absolute, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeAbsoluteUri(Uri parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new RelativeUriException(parameterName, message ?? $"{parameterName ?? "The URI"} must be an absolute URI, but it actually is \"{parameter}\".");

    /// <summary>
    /// Throws the default <see cref="AbsoluteUriException" /> indicating that a URI is absolute instead of relative, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeRelativeUri(Uri parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new AbsoluteUriException(parameterName, message ?? $"{parameterName ?? "The URI"} must be a relative URI, but it actually is \"{parameter}\".");

    /// <summary>
    /// Throws the default <see cref="InvalidUriSchemeException" /> indicating that a URI has an unexpected scheme, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void UriMustHaveScheme(Uri parameter, string scheme, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidUriSchemeException(parameterName, message ?? $"{parameterName ?? "The URI"} must use the scheme \"{scheme}\", but it actually is \"{parameter}\".");

    /// <summary>
    /// Throws the default <see cref="InvalidUriSchemeException" /> indicating that a URI does not use one of a set of expected schemes, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void UriMustHaveOneSchemeOf(Uri parameter, IEnumerable<string> schemes, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidUriSchemeException(parameterName,
                                            message ??
                                            new StringBuilder().AppendLine($"{parameterName ?? "The URI"} must use one of the following schemes")
                                                               .AppendItemsWithNewLine(schemes)
                                                               .AppendLine($"but it actually is \"{parameter}\".")
                                                               .ToString());

    /// <summary>
    /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using <see cref="DateTimeKind.Utc" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeUtcDateTime(DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Utc}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\".");

    /// <summary>
    /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using <see cref="DateTimeKind.Local" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeLocalDateTime(DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Local}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\".");

    /// <summary>
    /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using <see cref="DateTimeKind.Unspecified" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeUnspecifiedDateTime(DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) =>
        throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Unspecified}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\".");

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException(Func<Exception> exceptionFactory) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))();

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="parameter" /> is passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException<T>(Func<T, Exception> exceptionFactory, T parameter) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(parameter);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="first" /> and <paramref name="second" /> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException<T1, T2>(Func<T1, T2, Exception> exceptionFactory, T1 first, T2 second) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="first" />, <paramref name="second" />, and <paramref name="third"/> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException<T1, T2, T3>(Func<T1, T2, T3, Exception> exceptionFactory, T1 first, T2 second, T3 third) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second, third);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory"/>. <paramref name="span"/> and <paramref name="value"/> are passed to <paramref name="exceptionFactory"/>.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomSpanException<TItem, T>(SpanExceptionFactory<TItem, T> exceptionFactory, in Span<TItem> span, T value) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(span, value);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory"/>. <paramref name="span"/> and <paramref name="value"/> are passed to <paramref name="exceptionFactory"/>.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomSpanException<TItem, T>(ReadOnlySpanExceptionFactory<TItem, T> exceptionFactory, in ReadOnlySpan<TItem> span, T value) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(span, value);


}