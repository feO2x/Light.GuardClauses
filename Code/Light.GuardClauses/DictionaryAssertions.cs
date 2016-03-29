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
        ///     The message that will be injected into the <see cref="KeyNotFoundException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> has does not have the specified key (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        ///     Thrown when <paramref name="parameter" /> does not contain the specified <paramref name="key" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContainKey<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TKey key, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

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
        ///     The message that will be injected into the <see cref="DictionaryException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> has contains the specified key (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">
        ///     Thrown when <paramref name="parameter" /> contains <paramref name="key" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainKey<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TKey key, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

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
        /// <param name="message">The message that will be injected into the <see cref="KeyNotFoundException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> does not contain any of the specified keys (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="KeyNotFoundException">Thrown when <paramref name="parameter" /> does not contain any of the specified <paramref name="keys" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="keys" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContainKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, IEnumerable<TKey> keys, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
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
        public static void MustContainKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, params TKey[] keys)
        {
            MustContainKeys(parameter, (IEnumerable<TKey>) keys);
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified keys, or otherwise throws a <see cref="DictionaryException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="keys">The collection of keys that must not be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="DictionaryException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> does not contain any of the specified keys (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">Thrown when <paramref name="parameter" /> does contain any of the specified <paramref name="keys" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="keys" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, IEnumerable<TKey> keys, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
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
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does contain any of the specified <paramref name="keys" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainKeys<TKey, TValue>(this IDictionary<TKey, TValue> parameter, params TKey[] keys)
        {
            MustNotContainKeys(parameter, (IEnumerable<TKey>) keys);
        }

        /// <summary>
        ///     Ensures that the dictionary contains the specified value, or otherwise throws a <see cref="ValueNotFoundException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="value">The value that should be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ValueNotFoundException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> does not contain the specified <paramref name="value" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ValueNotFoundException">Thrown when <paramref name="parameter" /> does not contain the specified <paramref name="value" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContainValue<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TValue value, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.Values.Contains(value) == false)
                throw exception != null ? exception() : new ValueNotFoundException(message ?? $"{parameterName ?? "The dictionary"} must contain value \"{value.ToStringOrNull()}\", but it does not.{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
        }

        /// <summary>
        ///     Ensures that the dictionary contains the specified values, or otherwise throws a <see cref="ValueNotFoundException" />. This method is not aware of duplicates.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="values">The values that should be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ValueNotFoundException" />.</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> does not contain all of the specified <paramref name="values" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ValueNotFoundException">Thrown when <paramref name="parameter" /> does not contain the specified <paramref name="values" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="values" /> has no entries.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="values" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContainValues<TKey, TValue>(this IDictionary<TKey, TValue> parameter, IEnumerable<TValue> values, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            values.MustNotBeNullOrEmpty(nameof(values));

            if (values.Any(v => parameter.Values.Contains(v) == false))
                throw exception != null ? exception() : new ValueNotFoundException(message ?? $"{parameterName ?? "The dictionary"} must contain all the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(values)}{Environment.NewLine}{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the dictionary contains the specified values, or otherwise throws a <see cref="ValueNotFoundException" />. This method is not aware of duplicates and throws the default exception.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="values">The values that should be part of the dictionary.</param>
        /// <exception cref="ValueNotFoundException">Thrown when <paramref name="parameter" /> does not contain the specified <paramref name="values" />.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="values" /> has no entries.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContainValues<TKey, TValue>(this IDictionary<TKey, TValue> parameter, params TValue[] values)
        {
            MustContainValues(parameter, (IEnumerable<TValue>) values);
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified value, or otherwise throws a <see cref="DictionaryException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="value">The values that should not be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ValueNotFoundException" />.</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> does contain the specified <paramref name="value" />.
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">Thrown when <paramref name="parameter" /> contains the specified <paramref name="value" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainValue<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TValue value, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.Values.Contains(value))
                throw exception != null ? exception() : new DictionaryException(message ?? $"{parameterName ?? "The dictionary"} must not contain value \"{value.ToStringOrNull()}\", but it does.{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified values, or otherwise throws a <see cref="DictionaryException" />. This method is not aware of duplicates.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="values">The values that should not be part of the dictionary.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="DictionaryException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> contains any of the specified <paramref name="values" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">Thrown when <paramref name="parameter" /> contains any of the specified <paramref name="values" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="values" /> has no entries.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="values" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainValues<TKey, TValue>(this IDictionary<TKey, TValue> parameter, IEnumerable<TValue> values, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            values.MustNotBeNullOrEmpty(nameof(values));

            if (values.Any(parameter.Values.Contains))
                throw exception != null ? exception() : new DictionaryException(message ?? $"{parameterName ?? "The dictionary"} must not contain any of the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(values)}{Environment.NewLine}{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified values, or otherwise throws a <see cref="DictionaryException" />. This method is not aware of duplicates and throws the default exception.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="values">The values that should not be part of the dictionary.</param>
        /// <exception cref="ValueNotFoundException">Thrown when <paramref name="parameter" /> does contain any of the specified <paramref name="values" />.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="values" /> has no entries.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainValues<TKey, TValue>(this IDictionary<TKey, TValue> parameter, params TValue[] values)
        {
            MustNotContainValues(parameter, (IEnumerable<TValue>) values);
        }

        /// <summary>
        ///     Ensures that the dictionary contains the specified key-value-pair, or otherwise throws a <see cref="DictionaryException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="key">The key of the pair.</param>
        /// <param name="value">The value of the pair.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="DictionaryException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> does not contain the specified key-value-pair (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">Thrown when <paramref name="parameter" /> does not contain the specified key-value-pair and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContainPair<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TKey key, TValue value, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(new KeyValuePair<TKey, TValue>(key, value)) == false)
                throw exception != null ? exception() : new DictionaryException(message ?? $"{parameterName ?? "The dictionary"} must contain the key-value-pair \"{new StringBuilder().AppendKeyValuePair(key, value)}\", but it does not.{Environment.NewLine}{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
        }

        /// <summary>
        ///     Ensures that the dictionary does not contain the specified key-value-pair, or otherwise throws a <see cref="DictionaryException" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
        /// <param name="parameter">The dictionary to be checked.</param>
        /// <param name="key">The key of the pair.</param>
        /// <param name="value">The value of the pair.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="DictionaryException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when <paramref name="parameter" /> does contain the specified key-value-pair (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="DictionaryException">Thrown when <paramref name="parameter" /> does contain the specified key-value-pair and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainPair<TKey, TValue>(this IDictionary<TKey, TValue> parameter, TKey key, TValue value, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(new KeyValuePair<TKey, TValue>(key, value)))
                throw exception != null ? exception() : new DictionaryException(message ?? $"{parameterName ?? "The dictionary"} must not contain the key-value-pair \"{new StringBuilder().AppendKeyValuePair(key, value)}\", but it does.{Environment.NewLine}{Environment.NewLine}Actual content of the dictionary:{Environment.NewLine}{new StringBuilder().AppendKeyValuePairsWithNewLine(parameter)}", parameterName);
        }
    }
}