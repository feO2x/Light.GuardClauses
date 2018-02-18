using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Represents information indicating if substrings are searched case-sensitive or not.
    /// </summary>
    public readonly struct IgnoreCaseInfo
    {
        /// <summary>
        /// Gets the culture info whose <see cref="CompareInfo" /> is used for searching.
        /// </summary>
        public readonly CultureInfo CultureInfo;

        /// <summary>
        /// Gets the value indicating how strings are compared.
        /// </summary>
        public readonly CompareOptions CompareOptions;

        /// <summary>
        /// Initializes a new instance of <see cref="IgnoreCaseInfo" />.
        /// </summary>
        /// <param name="cultureInfo">The culture info whose <see cref="CompareInfo" /> is used for searching.</param>
        /// <param name="compareOptions">The value indicating how strings are compared.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cultureInfo" /> is null.</exception>
        /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="compareOptions" /> is no valid enum value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IgnoreCaseInfo(CultureInfo cultureInfo, CompareOptions compareOptions)
        {
            CultureInfo = cultureInfo.MustNotBeNull(nameof(cultureInfo));
            CompareOptions = compareOptions.MustBeValidEnumValue(nameof(compareOptions));
        }

        /// <summary>
        /// Creates an <see cref="IgnoreCaseInfo" /> with the invariant culture and CompareOptions.OrdinalIgnoreCase when the
        /// specified boolean is true.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IgnoreCaseInfo(bool ignoreCase)
        {
            return ignoreCase ? new IgnoreCaseInfo(CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase) : new IgnoreCaseInfo();
        }

        /// <summary>
        /// Creates an <see cref="IgnoreCaseInfo" /> with the specified culture info and CompareOptions.OrdinalIgnoreCase.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cultureInfo" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IgnoreCaseInfo(CultureInfo cultureInfo)
        {
            return new IgnoreCaseInfo(cultureInfo, CompareOptions.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the <paramref name="string" /> contains the <paramref name="substring" />.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
        /// <param name="substring">The string to seek.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool StringContains(string @string, string substring)
        {
            if (CultureInfo == null)
                return @string.MustNotBeNull(nameof(@string)).Contains(substring);

            return CultureInfo.CompareInfo.IndexOf(@string, substring, CompareOptions) != -1;
        }
    }
}