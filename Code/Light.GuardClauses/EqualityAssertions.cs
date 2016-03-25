using System;
using System.Collections.Generic;
using System.Diagnostics;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The EqualityAssertions class contains assertions that can be used to ensure that two values are equal or different.
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBe<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other) == false)
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (equalityComparer.EqualsWithHashCode(parameter, other) == false)
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeEqualTo<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other) == false)
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeEqualToValue<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : struct
        {
            if (parameter.EqualsValueWithHashCode(other) == false)
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBe<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other))
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (equalityComparer.EqualsWithHashCode(parameter, other))
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeEqualTo<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.EqualsWithHashCode(other))
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeEqualToValue<T>(this IEquatable<T> parameter, IEquatable<T> other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : struct
        {
            if (parameter.EqualsValueWithHashCode(other))
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeSameAs<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (ReferenceEquals(parameter, other) == false)
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
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeSameAs<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (ReferenceEquals(parameter, other))
                throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The reference"} must not point to the object instance \"{other}\", but it does.", parameterName);
        }
    }
}