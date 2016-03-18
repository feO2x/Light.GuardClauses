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
    ///     The CollectionAssertions class contains extension methods that apply assertions to collections.
    /// </summary>
    public static class CollectionAssertions
    {
        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is one of the specified <paramref name="items" />, or otherwise throws a
        ///     <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="items">The items where <paramref name="parameter" /> must be part of.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not part of
        ///     <paramref name="items" /> (optional). Please note that <paramref name="message" /> and
        ///     <paramref name="parameterName" /> are
        ///     both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="parameter" /> is not part of
        ///     <paramref name="items" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeOneOf<T>(this T parameter, IReadOnlyList<T> items, string parameterName = null, string message = null, Exception exception = null)
        {
            items.MustNotBeNull(nameof(items), "You called MustBeOneOf wrongly by specifying items as null.");

            if (items.Contains(parameter))
                return;

            var stringBuilder = new StringBuilder().AppendItems(items);
            throw exception ?? new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be one of the items ({stringBuilder}), but you specified {parameter}.");
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeOneOf<T>(this T parameter, IReadOnlyList<T> items, string parameterName)
        {
            if (items.Contains(parameter) == false)
                return;

            var stringBuilder = new StringBuilder().AppendItems(items);
            throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be none of the items ({stringBuilder}), but you specified {parameter}.");
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrEmpty<T>(this IReadOnlyCollection<T> collection, string parameterName)
        {
            if (collection == null)
                throw new ArgumentNullException(parameterName);

            if (collection.Count == 0)
                throw new EmptyCollectionException(parameterName);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveUniqueItems<T>(this IReadOnlyList<T> parameter, string parameterName)
        {
            for (var i = 0; i < parameter.Count; i++)
            {
                var itemToCompare = parameter[i];
                for (var j = i + 1; j < parameter.Count; j++)
                {
                    if (!itemToCompare.EqualsWithHashCode(parameter[j]))
                        continue;

                    var stringBuilder = new StringBuilder().AppendItems(parameter);
                    throw new CollectionException($"{parameterName} must be a collection with unique items, but you specified {stringBuilder}.", parameterName);
                }
            }
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeKeyOf<TKey, TValue>(this TKey parameter, IDictionary<TKey, TValue> dictionary, string parameterName)
        {
            if (dictionary.ContainsKey(parameter))
                return;

            var stringBuilder = new StringBuilder().AppendItems(dictionary.Keys.ToList());
            throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be one of the dictionary keys ({stringBuilder}), but you specified {parameter}.");
        }
    }
}