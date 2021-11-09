using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// The <see cref="Check" /> class provides access to all assertions of Light.GuardClauses.
    /// </summary>
    public static partial class Check
    {
        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <param name="parameter">The object reference to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNull<T>([ValidatedNotNull] this T? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : class
        {
            if (parameter is null)
                Throw.ArgumentNull(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The reference to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeNull<T>([ValidatedNotNull] this T? parameter, Func<Exception> exceptionFactory) where T : class
        {
            if (parameter is null)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeDefault<T>([ValidatedNotNull] this T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (default(T) is null)
            {
                if (parameter is null)
                    Throw.ArgumentNull(parameterName, message);
                return parameter;
            }

            if (EqualityComparer<T>.Default.Equals(parameter, default!))
                Throw.ArgumentDefault(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified parameter is not the default value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is the default value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeDefault<T>([ValidatedNotNull] this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T) is null)
            {
                if (parameter is null)
                    Throw.CustomException(exceptionFactory);
                return parameter;
            }

            if (EqualityComparer<T>.Default.Equals(parameter, default!))
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
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNullReference<T>([ValidatedNotNull] this T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (default(T) != null)
                return parameter;

            if (parameter is null)
                Throw.ArgumentNull(parameterName, message);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeNullReference<T>([ValidatedNotNull] this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T) != null)
                return parameter;

            if (parameter is null)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> can be cast to <typeparamref name="T" /> and returns the cast value, or otherwise throws a <see cref="TypeCastException" />.
        /// </summary>
        /// <param name="parameter">The value to be cast.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="TypeCastException">Thrown when <paramref name="parameter" /> cannot be cast to <typeparamref name="T" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeOfType<T>([ValidatedNotNull] this object? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message) is T castValue)
                return castValue;

            Throw.InvalidTypeCast(parameter, typeof(T), parameterName, message);
            return default;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> can be cast to <typeparamref name="T" /> and returns the cast value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be cast.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. The <paramref name="parameter" /> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> cannot be cast to <typeparamref name="T" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeOfType<T>([ValidatedNotNull] this object? parameter, Func<object?, Exception> exceptionFactory)
        {
            if (parameter is T castValue)
                return castValue;

            Throw.CustomException(exceptionFactory, parameter);
            return default;
        }

        /// <summary>
        /// Checks if the specified value is a valid enum value of its type. This is true when the specified value
        /// is one of the constants defined in the enum, or a valid flags combination when the enum type is marked
        /// with the <see cref="FlagsAttribute" />.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="parameter">The enum value to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidEnumValue<T>(this T parameter) where T : Enum
            => EnumInfo<T>.IsValidEnumValue(parameter);

        /// <summary>
        /// Ensures that the specified enum value is valid, or otherwise throws an <see cref="EnumValueNotDefinedException" />. An enum value
        /// is valid when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type
        /// is marked with the <see cref="FlagsAttribute" />.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="parameter" /> is no valid enum value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeValidEnumValue<T>(this T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : Enum
        {
            if (!EnumInfo<T>.IsValidEnumValue(parameter))
                Throw.EnumValueNotDefined(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified enum value is valid, or otherwise throws your custom exception. An enum value
        /// is valid when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type
        /// is marked with the <see cref="FlagsAttribute" />.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. The <paramref name="parameter" /> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is no valid enum value, or when <typeparamref name="T" /> is no enum type.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static T MustBeValidEnumValue<T>(this T parameter, Func<T, Exception> exceptionFactory) where T : Enum
        {
            if (!EnumInfo<T>.IsValidEnumValue(parameter))
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified GUID is an empty one.
        /// </summary>
        /// <param name="parameter">The GUID to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Guid parameter) => parameter == Guid.Empty;

        /// <summary>
        /// Ensures that the specified GUID is not empty, or otherwise throws an <see cref="EmptyGuidException" />.
        /// </summary>
        /// <param name="parameter">The GUID to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="EmptyGuidException">Thrown when <paramref name="parameter" /> is an empty GUID.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid MustNotBeEmpty(this Guid parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter == Guid.Empty)
                Throw.EmptyGuid(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified GUID is not empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The GUID to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is an empty GUID.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static Guid MustNotBeEmpty(this Guid parameter, Func<Exception> exceptionFactory)
        {
            if (parameter == Guid.Empty)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified <paramref name="condition" /> is true and throws an <see cref="InvalidOperationException" /> in this case.
        /// </summary>
        /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidOperationException" /> (optional).</param>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="condition" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidOperation(bool condition, string? message = null)
        {
            if (condition)
                Throw.InvalidOperation(message);
        }

        /// <summary>
        /// Checks if the specified <paramref name="condition" /> is true and throws an <see cref="InvalidStateException" /> in this case.
        /// </summary>
        /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidStateException" />.</param>
        /// <exception cref="InvalidStateException">Thrown when <paramref name="condition" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidState(bool condition, string? message = null)
        {
            if (condition)
                Throw.InvalidState(message);
        }

        /// <summary>
        /// Checks if the specified <paramref name="condition" /> is true and throws an <see cref="ArgumentException" /> in this case.
        /// </summary>
        /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentException" /> (optional).</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="condition" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidArgument(bool condition, [CallerArgumentExpression("condition")] string? parameterName = null, string? message = null)
        {
            if (condition)
                Throw.Argument(parameterName, message);
        }

        /// <summary>
        /// Checks if the specified <paramref name="condition" /> is true and throws your custom exception in this case.
        /// </summary>
        /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="condition" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static void InvalidArgument(bool condition, Func<Exception> exceptionFactory)
        {
            if (condition)
                Throw.CustomException(exceptionFactory);
        }

        /// <summary>
        /// Checks if the specified <paramref name="condition" /> is true and throws your custom exception in this case.
        /// </summary>
        /// <param name="condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name="parameter">The value that is checked in the <paramref name="condition" />. This value is passed to the <paramref name="exceptionFactory" />.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. The <paramref name="parameter" /> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="condition" /> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static void InvalidArgument<T>(bool condition, T parameter, Func<T, Exception> exceptionFactory)
        {
            if (condition)
                Throw.CustomException(exceptionFactory, parameter);
        }

        /// <summary>
        /// Ensures that the specified nullable has a value and returns it, or otherwise throws a <see cref="NullableHasNoValueException" />.
        /// </summary>
        /// <param name="parameter">The nullable to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="NullableHasNoValueException">Thrown when <paramref name="parameter" /> has no value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustHaveValue<T>(this T? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : struct
        {
            if (!parameter.HasValue)
                Throw.NullableHasNoValue(parameterName, message);

            return parameter.Value;
        }

        /// <summary>
        /// Ensures that the specified nullable has a value and returns it, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The nullable to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="NullableHasNoValueException">Thrown when <paramref name="parameter" /> has no value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static T MustHaveValue<T>(this T? parameter, Func<Exception> exceptionFactory) where T : struct
        {
            if (!parameter.HasValue)
                Throw.CustomException(exceptionFactory);

            return parameter.Value;
        }

        /// <summary>
        /// Checks if <paramref name="parameter" /> and <paramref name="other" /> point to the same object.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable StringLiteralTypo
        [ContractAnnotation("parameter:notNull => true, other:notnull; parameter:notNull => false, other:canbenull; other:notnull => true, parameter:notnull; other:notnull => false, parameter:canbenull")]
        // ReSharper restore StringLiteralTypo
        public static bool IsSameAs<T>(this T? parameter, T? other) where T : class =>
            ReferenceEquals(parameter, other);

        /// <summary>
        /// Ensures that <paramref name="parameter" /> and <paramref name="other" /> do not point to the same object instance, or otherwise
        /// throws a <see cref="SameObjectReferenceException" />.
        /// </summary>
        /// <param name="parameter">The first reference to be checked.</param>
        /// <param name="other">The second reference to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SameObjectReferenceException">Thrown when both <paramref name="parameter" /> and <paramref name="other" /> point to the same object.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustNotBeSameAs<T>(this T? parameter, T? other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) where T : class
        {
            if (ReferenceEquals(parameter, other))
                Throw.SameObjectReference(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> and <paramref name="other" /> do not point to the same object instance, or otherwise
        /// throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first reference to be checked.</param>
        /// <param name="other">The second reference to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
        /// <exception cref="SameObjectReferenceException">Thrown when both <paramref name="parameter" /> and <paramref name="other" /> point to the same object.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustNotBeSameAs<T>(this T? parameter, T? other, Func<T?, Exception> exceptionFactory) where T : class
        {
            if (ReferenceEquals(parameter, other))
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the default equality comparer, or otherwise throws a <see cref="ValuesNotEqualException" />.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBe<T>(this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!EqualityComparer<T>.Default.Equals(parameter, other))
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the default equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBe<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory)
        {
            if (!EqualityComparer<T>.Default.Equals(parameter, other))
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws a <see cref="ValuesNotEqualException" />.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!equalityComparer.MustNotBeNull(nameof(equalityComparer), message).Equals(parameter, other))
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="other" />, and <paramref name="equalityComparer" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are not equal, or when <paramref name="equalityComparer" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, Func<T, T, IEqualityComparer<T>, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
            if (equalityComparer is null || !equalityComparer.Equals(parameter, other))
                Throw.CustomException(exceptionFactory, parameter, other, equalityComparer!);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the default equality comparer, or otherwise throws a <see cref="ValuesEqualException" />.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBe<T>(this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (EqualityComparer<T>.Default.Equals(parameter, other))
                Throw.ValuesEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the default equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBe<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory)
        {
            if (EqualityComparer<T>.Default.Equals(parameter, other))
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws a <see cref="ValuesEqualException" />.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesEqualException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (equalityComparer.MustNotBeNull(nameof(equalityComparer), message).Equals(parameter, other))
                Throw.ValuesEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is not equal to <paramref name="other" /> using the specified equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first value to be compared.</param>
        /// <param name="other">The other value to be compared.</param>
        /// <param name="equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="other" />, and <paramref name="equalityComparer" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> and <paramref name="other" /> are equal, or when <paramref name="equalityComparer" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, Func<T, T, IEqualityComparer<T>, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
            if (equalityComparer is null || equalityComparer.Equals(parameter, other))
                Throw.CustomException(exceptionFactory, parameter, other, equalityComparer!);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name="value">The first value to be compared.</param>
        /// <param name="other">The second value to be compared.</param>
        /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name="value" /> <paramref name="other" /> are equal or if their absolute difference
        /// is smaller than the given <paramref name="tolerance" />, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this double value, double other, double tolerance) =>
            Math.Abs(value - other) < tolerance;

        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001.
        /// </summary>
        /// <param name="value">The first value to be compared.</param>
        /// <param name="other">The second value to be compared.</param>
        /// <returns>
        /// True if <paramref name="value" /> <paramref name="other" /> are equal or if their absolute difference
        /// is smaller than 0.0001, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this double value, double other) =>
            Math.Abs(value - other) < 0.0001;

        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name="value">The first value to compare.</param>
        /// <param name="other">The second value to compare.</param>
        /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name="value" /> <paramref name="other" /> are equal or if their absolute difference
        /// is smaller than the given <paramref name="tolerance" />, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this float value, float other, float tolerance) =>
            Math.Abs(value - other) < tolerance;

        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001f.
        /// </summary>
        /// <param name="value">The first value to be compared.</param>
        /// <param name="other">The second value to be compared.</param>
        /// <returns>
        /// True if <paramref name="value" /> <paramref name="other" /> are equal or if their absolute difference
        /// is smaller than 0.0001f, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this float value, float other) =>
            Math.Abs(value - other) < 0.0001f;
    }
}