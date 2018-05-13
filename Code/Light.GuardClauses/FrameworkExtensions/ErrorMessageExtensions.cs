using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// Provides extension methods for <see cref="string" /> and <see cref="StringBuilder" /> to easily assembly error messages.
    /// </summary>
    public static class ErrorMessageExtensions
    {
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
        /// If the type of <paramref name="value"/> is not one of <see cref="UnquotedTypes"/>, then quotation marks will be put around the string representation.
        /// </summary>
        /// <param name="value">The item whose string representation should be returned.</param>
        /// <param name="nullText">The text that is returned when <paramref name="value" /> is null (defaults to "null").</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string ToStringOrNull<T>(this T value, string nullText = "null") => value == null ? nullText : value.ToStringRepresentation();

        /// <summary>
        /// Returns the string representation of <paramref name="value"/>. This is done by calling <see cref="object.ToString"/>. If the type of
        /// <paramref name="value"/> is not one of <see cref="UnquotedTypes"/>, then the resulting string will be wrapped in quotation marks.
        /// </summary>
        /// <param name="value">The value whose string representation is requested.</param>
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
    }
}