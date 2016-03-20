using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The DictionaryAssertions class contains extension methods providing assertions for
    ///     <see cref="Dictionary{TKey,TValue}" /> instances.
    /// </summary>
    public static class DictionaryAssertions
    {
        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is a key of the specified <paramref name="dictionary" />, or otherwise
        ///     throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="TKey">The key type of the <paramref name="dictionary" />.</typeparam>
        /// <typeparam name="TValue">The value type of the <paramref name="dictionary" />.</typeparam>
        /// <param name="parameter">The value that should be key of the <paramref name="dictionary" />.</param>
        /// <param name="dictionary">The dictionary whose keys are used for checking.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is no key of
        ///     <paramref name="dictionary" /> (optional). Please note that <paramref name="message" /> and
        ///     <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="parameter" /> is no key of
        ///     <paramref name="dictionary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeKeyOf<TKey, TValue>(this TKey parameter, IDictionary<TKey, TValue> dictionary, string parameterName = null, string message = null, Exception exception = null)
        {
            dictionary.MustNotBeNull(nameof(dictionary), "You called MustBeKeyOf wrongly by specifying a dictionary that is null.");

            if (dictionary.ContainsKey(parameter))
                return;

            throw exception ?? new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be one of the dictionary keys ({new StringBuilder().AppendItems(dictionary.Keys.ToList())}), but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that the dictionary contains the specified <paramref name="key" />, or otherwise throws a
        ///     <see cref="KeyNotFoundException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="key">The key that should be part of the Keys collection of <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="KeyNotFoundException" /> or
        ///     <see cref="ArgumentNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> has does not have
        ///     the specified key (optional). Please note that <paramref name="message" /> and <paramref name="parameterName" />
        ///     are both ignored when you specify exception.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        ///     Thrown when <paramref name="parameter" /> does not contain the specified
        ///     <paramref name="key" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveKey<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TKey key, string parameterName = null, string message = null, Exception exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);

            if (parameter.ContainsKey(key) == false)
                throw exception ?? new KeyNotFoundException(message ?? $"{parameterName ?? "The dictionary"} must have key \"{key}\".{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairs(parameter, "," + Environment.NewLine)}");
        }
    }
}