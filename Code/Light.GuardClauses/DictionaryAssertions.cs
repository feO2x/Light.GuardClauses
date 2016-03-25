using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The DictionaryAssertions class contains extension methods providing assertions for <see cref="IDictionary{TKey,TValue}" /> instances.
    /// </summary>
    public static class DictionaryAssertions
    {
        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is a key of the specified <paramref name="dictionary" />, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The value that should be key of the <paramref name="dictionary" />.</param>
        /// <param name="dictionary">The dictionary whose keys are used for checking.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is no key of <paramref name="dictionary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="parameter" /> is no key of <paramref name="dictionary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeKeyOf<TKey, TValue>(this TKey parameter, IDictionary<TKey, TValue> dictionary, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            dictionary.MustNotBeNull(nameof(dictionary));

            if (dictionary.ContainsKey(parameter))
                return;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be one of the dictionary keys{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(dictionary.Keys)}{Environment.NewLine}but you specified {parameter}.");
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is not a key of the specified <paramref name="dictionary" />, or otherwise throws a <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The value that should not be key of <paramref name="dictionary" />.</param>
        /// <param name="dictionary">The dictionary whose keys are used for checking.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is a key of <paramref name="dictionary" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="parameter" /> is a key of <paramref name="dictionary" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeKeyOf<TKey, TValue>(this TKey parameter, IDictionary<TKey, TValue> dictionary, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            dictionary.MustNotBeNull(nameof(dictionary));

            if (dictionary.ContainsKey(parameter))
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be one of the dictionary keys{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(dictionary.Keys)}{Environment.NewLine}but you specified {parameter}");
        }

        /// <summary>
        ///     Ensures that the dictionary contains the specified <paramref name="key" />, or otherwise throws a <see cref="KeyNotFoundException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="key">The key that should be part of the Keys collection of <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="KeyNotFoundException" /> or <see cref="ArgumentNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> has does not have the specified key (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        ///     Thrown when <paramref name="parameter" /> does not contain the specified <paramref name="key" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveKey<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TKey key, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);

            if (parameter.ContainsKey(key) == false)
                throw exception != null ? exception() : new KeyNotFoundException(message ?? $"{parameterName ?? "The dictionary"} must contain key \"{key}\".{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}");
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified <paramref name="key" />, or otherwise throws a <see cref="DictionaryException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="key">The key that must not be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="DictionaryException" /> or <see cref="ArgumentNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> has contains the specified key (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">
        ///     Thrown when <paramref name="parameter" /> contains <paramref name="key" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotHaveKey<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TKey key, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);

            if (parameter.ContainsKey(key))
                throw exception != null ? exception() : new DictionaryException(message ?? $"{parameterName ?? "The dictionary"} must not contain key \"{key}\".{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
        }

        /// <summary>
        ///     Ensures that the dictionary contains all the specified keys, or otherwise throws a <see cref="KeyNotFoundException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="keys">The collection of keys that must be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="KeyNotFoundException" /> or <see cref="ArgumentNullException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> does not contain any of the specified keys (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="KeyNotFoundException">Thrown when <paramref name="parameter" /> does not contain any of the specified <paramref name="keys" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified or Thrown when <paramref name="keys" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, IEnumerable<TKey> keys, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);
            // ReSharper disable PossibleMultipleEnumeration
            keys.MustNotBeNull(nameof(keys));

            if (keys.Any(k => parameter.ContainsKey(k) == false))
                throw exception != null ? exception() : new KeyNotFoundException(message ?? $"{parameterName ?? "The dictionary"} must contain all of the following keys:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(keys)}{Environment.NewLine}but does not.{Environment.NewLine}{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}");
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the dictionary contains all the specified keys, or otherwise throws a <see cref="KeyNotFoundException" />. This method uses the default exception.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="keys">The collection of keys that must be part of the dictionary.</param>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not contain any of the specified <paramref name="keys" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, params TKey[] keys)
        {
            MustHaveKeys(parameter, (IEnumerable<TKey>) keys);
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified keys, or otherwise throws a <see cref="DictionaryException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="keys">The collection of keys that must not be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="DictionaryException" /> or <see cref="ArgumentNullException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> does not contain any of the specified keys (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">Thrown when <paramref name="parameter" /> does contain any of the specified <paramref name="keys" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified or Thrown when <paramref name="keys" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotHaveKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, IEnumerable<TKey> keys, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName, message, exception);
            keys.MustNotBeNull(nameof(keys));

            if (keys.Any(parameter.ContainsKey))
                throw exception != null ? exception() : new DictionaryException(message ?? $"{parameterName ?? "The dictionary"} must not contain any of the following keys:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(keys)}{Environment.NewLine}but it does.{Environment.NewLine}{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified keys, or otherwise throws a <see cref="DictionaryException" />. This method uses the default exception.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="keys">The collection of keys that must not be part of the dictionary.</param>
        /// ///
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does contain any of the specified <paramref name="keys" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotHaveKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, params TKey[] keys)
        {
            MustNotHaveKeys(parameter, (IEnumerable<TKey>) keys);
        }
    }
}