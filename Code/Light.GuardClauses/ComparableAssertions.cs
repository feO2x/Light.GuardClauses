using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The <see cref="ComparableAssertions" /> class contains extension methods that check assertions for the <see cref="IComparable{T}" /> interface.
    /// </summary>
    public static class ComparableAssertions
    {
        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must not exceed.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is less than <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is less than <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeLessThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) < 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be less than {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must be less than.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not less than <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is not less than <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeLessThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) >= 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is not less than or equal to the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must not exceed.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeLessThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) <= 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is less than or equal to the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must be less than or equal to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not less than or equal to <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is not less than or equal to <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeLessThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) > 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be less than or equal to {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is not greaten than the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must not exceed.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is greater than <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is greater than <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeGreaterThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) > 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be greater than {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must be greater than.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not greater than <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is not greater than <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeGreaterThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) <= 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be greater than {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is not greaten than or equal to the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must not exceed.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is greater than or equal to <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is greater than or equal to <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeGreaterThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) >= 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="parameter" /> is greaten than or equal to the given <paramref name="boundary" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="boundary">The boundary value that <paramref name="parameter" /> must be greatern than or equal to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not greater than or equal to <paramref name="boundary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is not greater than or equal to <paramref name="boundary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeGreaterThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) < 0)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be greater than or equal to {boundary}, but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is within the specified <paramref name="range" />, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="range">The range that <paramref name="parameter" /> must be in between.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not within <paramref name="range" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is not within <paramref name="range" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeIn<T>(this T parameter, Range<T> range, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";

            if (range.IsValueWithinRange(parameter) == false)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }

        /// <summary>
        ///     Checks if the parameter value is within the specified range.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="range">The range that <paramref name="parameter" /> must be in between.</param>
        /// <returns>True if the parameter is within the specified range, else false.</returns>
        public static bool IsIn<T>(this T parameter, Range<T> range) where T : IComparable<T>
        {
            return range.IsValueWithinRange(parameter);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is not within the specified <paramref name="range" />, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="range">The range that <paramref name="parameter" /> must not be in between.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is within <paramref name="range" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the specified <paramref name="parameter" /> is within <paramref name="range" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeIn<T>(this T parameter, Range<T> range, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";

            if (range.IsValueWithinRange(parameter))
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }
    }
}