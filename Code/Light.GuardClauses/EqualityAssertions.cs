using System;
using System.Collections.Generic;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    // TODO: I should check if it is useful have this many overloads of MustBe and MustNotBe
    /// <summary>
    ///     The <see cref="EnumerableAssertions" /> class contains assertions that can be used to ensure that two values are equal or different.
    /// </summary>
    public static class EqualityAssertions
    {
        /// <summary>
        ///     Ensures that the specified parameter is equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     GetHashCode and Equals are both used for comparison.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static T MustBe<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other))
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must be {other}, but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified parameter is equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     The specified <paramref name="equalityComparer" /> is used for comparison (both GetHashCode and Equals).
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="equalityComparer">The equality comparer that is used for comparing.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
        public static T MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (equalityComparer.EqualsWithHashCode(parameter, other))
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must be {other}, but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified parameter is equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     GetHashCode and Equals are both used for comparison.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static IEquatable<T> MustBeEqualTo<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other))
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must be {other}, but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified parameter is equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     GetHashCode and Equals are both used for comparison.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static IEquatable<T> MustBeEqualToValue<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : struct
        {
            if (parameter.EqualsValueWithHashCode(other))
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must be {other}, but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified parameter is not equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     GetHashCode and Equals are both used for comparison.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static T MustNotBe<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other) == false)
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be {other}, but you specified this very value.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified parameter is not equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     The specified <paramref name="equalityComparer" /> is used for comparison (both GetHashCode and Equals).
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="equalityComparer">The equality comparer that is used for comparing.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
        public static T MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (equalityComparer.EqualsWithHashCode(parameter, other) == false)
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be {other}, but you specified this very value.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified parameter is not equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     GetHashCode and Equals are both used for comparison.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static IEquatable<T> MustNotBeEqualTo<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other) == false)
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be {other}, but you specified this very value.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified parameter is not equal to the other specified value, or otherwise throws an <see cref="ArgumentException" />.
        ///     GetHashCode and Equals are both used for comparison.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The value that <paramref name="parameter" /> is checked against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentException" /> (optional). Please
        ///     note that <paramref name="parameterName" /> is ignored when you use message.
        /// </param>
        /// <param name="exception">
        ///     The exception that will be thrown when the comparison fails (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the specified parameter is different from the other value and no <paramref name="exception" /> is specified.
        /// </exception>
        public static IEquatable<T> MustNotBeEqualToValue<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : struct
        {
            if (parameter.EqualsValueWithHashCode(other) == false)
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be {other}, but you specified this very value.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified reference points to the same object instance as <paramref name="other" />, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter. This must be a reference type.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The other instance that parameter must be compared to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> and <paramref name="other" /> do not point to the same instance.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> do not point to the same instance and no <paramref name="exception" /> is specified.</exception>
        public static T MustBeSameAs<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (ReferenceEquals(parameter, other))
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The reference"} must point to the object instance \"{other}\", but it does not.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified reference does not point to the same object instance as <paramref name="other" />, or otherwise throws an <see cref="ArgumentException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter. This must be a reference type.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="other">The other instance that parameter must be compared to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> and <paramref name="other" /> point to the same instance.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameter" /> and <paramref name="other" /> point to the same instance and no <paramref name="exception" /> is specified.</exception>
        public static T MustNotBeSameAs<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (ReferenceEquals(parameter, other) == false)
                return parameter;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The reference"} must not point to the object instance \"{other}\", but it does.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified parameter points to the same object as the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="reference">The reference that parameter should be checked against.</param>
        /// <returns>True if both references point to the same object, else false.</returns>
        public static bool IsSameAs<T>(this T parameter, T reference) where T : class
        {
            return ReferenceEquals(parameter, reference);
        }
    }
}