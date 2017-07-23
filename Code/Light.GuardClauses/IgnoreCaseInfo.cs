using System;
using System.Globalization;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Represents information about how substrings are searched case-sensitive or not.
    /// </summary>
    public struct IgnoreCaseInfo
    {
        /// <summary>
        ///     Gets the culture info whose <see cref="CompareInfo" /> is used for searching.
        /// </summary>
        public readonly CultureInfo CultureInfo;

        /// <summary>
        ///     Gets the value indicating how strings are compared.
        /// </summary>
        public readonly CompareOptions CompareOptions;

        /// <summary>
        ///     Initializes a new instance of <see cref="IgnoreCaseInfo" />.
        /// </summary>
        /// <param name="cultureInfo">The culture info whose <see cref="CompareInfo" /> is used for searching.</param>
        /// <param name="compareOptions">The value indicating how strings are compared.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cultureInfo" /> is null.</exception>
        /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="compareOptions" /> is no valid enum value.</exception>
        public IgnoreCaseInfo(CultureInfo cultureInfo, CompareOptions compareOptions)
        {
            CultureInfo = cultureInfo.MustNotBeNull(nameof(cultureInfo));
            CompareOptions = compareOptions.MustBeValidEnumValue(nameof(compareOptions));
        }

        public static implicit operator IgnoreCaseInfo(bool ignoreCase)
        {
            return ignoreCase ? new IgnoreCaseInfo(CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase) : new IgnoreCaseInfo();
        }

        public static implicit operator IgnoreCaseInfo(CultureInfo cultureInfo)
        {
            return new IgnoreCaseInfo(cultureInfo, CompareOptions.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Checks if the first string contains the second string.
        /// </summary>
        public bool StringContains(string first, string second)
        {
            first.MustNotBeNull();
            second.MustNotBeNull();

            if (CultureInfo == null)
                return first.Contains(second);

            return CultureInfo.CompareInfo.IndexOf(first, second, CompareOptions) != -1;
        }
    }
}