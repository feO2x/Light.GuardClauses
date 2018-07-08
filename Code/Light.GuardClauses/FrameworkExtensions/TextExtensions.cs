using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// Provides extension methods for <see cref="string" /> and <see cref="StringBuilder" /> to easily assembly error messages.
    /// </summary>
    public static class TextExtensions
    {
        /// <summary>
        /// Gets the default NewLineSeparator. This value is $",{Environment.NewLine}".
        /// </summary>
        public static readonly string DefaultNewLineSeparator = ',' + Environment.NewLine;

        /// <summary>
        /// Gets the list of types that will not be surrounded by quotation marks in error messages.
        /// </summary>
        public static readonly ReadOnlyCollection<Type> UnquotedTypes =
            new ReadOnlyCollection<Type>(new[]
            {
                typeof(int),
                typeof(long),
                typeof(short),
                typeof(sbyte),
                typeof(uint),
                typeof(ulong),
                typeof(ushort),
                typeof(byte),
                typeof(bool),
                typeof(double),
                typeof(decimal),
                typeof(float)
            });

        /// <summary>
        /// Returns the string reprensentation of <paramref name="value" />, or <paramref name="nullText" /> if <paramref name="value" /> is null.
        /// If the type of <paramref name="value" /> is not one of <see cref="UnquotedTypes" />, then quotation marks will be put around the string representation.
        /// </summary>
        /// <param name="value">The item whose string representation should be returned.</param>
        /// <param name="nullText">The text that is returned when <paramref name="value" /> is null (defaults to "null").</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("=> notnull")]
        public static string ToStringOrNull<T>(this T value, string nullText = "null") => value == null ? nullText : value.ToStringRepresentation();

        /// <summary>
        /// Returns the string representation of <paramref name="value" />. This is done by calling <see cref="object.ToString" />. If the type of
        /// <paramref name="value" /> is not one of <see cref="UnquotedTypes" />, then the resulting string will be wrapped in quotation marks.
        /// </summary>
        /// <param name="value">The value whose string representation is requested.</param>
        [ContractAnnotation("value:null => halt; value:notnull => notnull")]
        public static string ToStringRepresentation<T>(this T value)
        {
            value.MustNotBeNullReference(nameof(value));

            if (UnquotedTypes.Contains(value.GetType()))
                return value.ToString();

            var content = value.ToString();
            var contentWithQuotationMarks = new char[content.Length + 2];
            contentWithQuotationMarks[0] = contentWithQuotationMarks[contentWithQuotationMarks.Length - 1] = '"';
            content.CopyTo(0, contentWithQuotationMarks, 1, content.Length);
            return new string(contentWithQuotationMarks);
        }

        /// <summary>
        /// Appends the content of the collection with the specified header line to the string builder.
        /// Each item is on a new line.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="stringBuilder">The string builder that the content is appended to.</param>
        /// <param name="items">The collection whose items will be appended to the string builder.</param>
        /// <param name="headerLine">The string that will be placed before the actual items as a header.</param>
        /// <param name="finishWithNewLine">The value indicating if a new line is added after the last item. This value defaults to true.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> or <paramref name="items" />is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("stringBuilder:null => halt; items:null => halt; stringBuilder:notnull => notnull")]
        public static StringBuilder AppendCollectionContent<T>(this StringBuilder stringBuilder, IEnumerable<T> items, string headerLine = "Content of the collection:", bool finishWithNewLine = true) =>
            stringBuilder.MustNotBeNull(nameof(stringBuilder))
                         .AppendLine(headerLine)
                         .AppendItemsWithNewLine(items, finishWithNewLine: finishWithNewLine);

        /// <summary>
        /// Appends the string representations of the specified items to the string builder.
        /// </summary>
        /// <param name="stringBuilder">The string builder where the items will be appended to.</param>
        /// <param name="items">The items to be appended.</param>
        /// <param name="itemSeparator">The characters used to separate the items. Defaults to ", " and is not appended after the last item.</param>
        /// <param name="emptyCollectionText">The text that is appended to the string builder when <paramref name="items" /> is empty. Defaults to "empty collection".</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> or <paramref name="items" /> is null.</exception>
        [ContractAnnotation("stringBuilder:null => halt; items:null => halt; stringBuilder:notnull => notnull")]
        public static StringBuilder AppendItems<T>(this StringBuilder stringBuilder, IEnumerable<T> items, string itemSeparator = ", ", string emptyCollectionText = "empty collection")
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            var list = items.MustNotBeNull(nameof(items)).AsList();

            var currentIndex = 0;
            var itemsCount = list.Count;
            if (itemsCount == 0)
                return stringBuilder.Append(emptyCollectionText);

            while (true)
            {
                stringBuilder.Append(list[currentIndex].ToStringOrNull());
                if (currentIndex < itemsCount - 1)
                    stringBuilder.Append(itemSeparator);
                else
                    return stringBuilder;
                ++currentIndex;
            }
        }

        /// <summary>
        /// Appends the string representations of the specified items to the string builder. Each item is on its own line.
        /// </summary>
        /// <param name="stringBuilder">The string builder where the items will be appended to.</param>
        /// <param name="items">The items to be appended.</param>
        /// <param name="emptyCollectionText">The text that is appended to the string builder when <paramref name="items" /> is empty. Defaults to "empty collection".</param>
        /// <param name="finishWithNewLine">The value indicating if a new line is added after the last item. This value defaults to true.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> or <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("stringBuilder:null => halt; items:null => halt; stringBuilder:notnull => notnull")]
        public static StringBuilder AppendItemsWithNewLine<T>(this StringBuilder stringBuilder, IEnumerable<T> items, string emptyCollectionText = "empty collection", bool finishWithNewLine = true) =>
            stringBuilder.AppendItems(items, DefaultNewLineSeparator, emptyCollectionText)
                         .AppendLineIf(finishWithNewLine);

        /// <summary>
        /// Appends the value to the specified string builder if the condition is true.
        /// </summary>
        /// <param name="stringBuilder">The string builder where <paramref name="value" /> will be appended to.</param>
        /// <param name="condition">The boolean value indicating whether the append will be performed or not.</param>
        /// <param name="value">The value to be appended to the string builder.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("stringBuilder:null => halt; stringBuilder:notnull => notnull")]
        public static StringBuilder AppendIf(this StringBuilder stringBuilder, bool condition, string value)
        {
            if (condition)
                stringBuilder.MustNotBeNull(nameof(stringBuilder)).Append(value);
            return stringBuilder;
        }

        /// <summary>
        /// Appends the value followed by a new line separator to the specified string builder if the condition is true.
        /// </summary>
        /// <param name="stringBuilder">The string builder where <paramref name="value" /> will be appended to.</param>
        /// <param name="condition">The boolean value indicating whether the append will be performed or not.</param>
        /// <param name="value">The value to be appended to the string builder (optional). This value defaults to an empty string.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("stringBuilder:null => halt; stringBuilder:notnull => notnull")]
        public static StringBuilder AppendLineIf(this StringBuilder stringBuilder, bool condition, string value = "")
        {
            if (condition)
                stringBuilder.MustNotBeNull(nameof(stringBuilder)).AppendLine(value);
            return stringBuilder;
        }

        /// <summary>
        /// Appends the messages of the <paramref name="exception" /> and its nested exceptions to the
        /// specified <paramref name="stringBuilder" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public static StringBuilder AppendExceptionMessages(this StringBuilder stringBuilder, Exception exception)
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            var currentException = exception.MustNotBeNull(nameof(exception));
            while (true)
            {
                stringBuilder.AppendLine(currentException.Message);
                if (currentException.InnerException == null)
                    return stringBuilder;

                stringBuilder.AppendLine();
                currentException = exception.InnerException;
            }
        }

        /// <summary>
        /// Formats all messages of the <paramref name="exception" /> and its nested exceptions into
        /// a single string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception" /> is null.</exception>
        public static string GetAllExceptionMessages(this Exception exception) => 
            new StringBuilder().AppendExceptionMessages(exception).ToString();
    }
}