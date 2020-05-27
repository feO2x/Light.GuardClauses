using System;
using System.Collections.Generic;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Represents an <see cref="IEqualityComparer{T}"/> that compares strings using the
    /// ordinal sort rules, ignoring the case and the white space characters.
    /// </summary>
    public sealed class OrdinalIgnoreCaseIgnoreWhiteSpaceComparer : IEqualityComparer<string>
    {
        /// <summary>
        /// Checks if the two strings are equal using ordinal sorting rules as well as ignoring the case and
        /// the white space of the provided strings.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="x"/> or <paramref name="y"/> are null.</exception>
        public bool Equals(string? x, string? y)
        {
            x.MustNotBeNull(nameof(x));
            y.MustNotBeNull(nameof(y));

            return x.EqualsOrdinalIgnoreCaseIgnoreWhiteSpace(y);
        }

        /// <summary>
        /// Gets the hash code for the specified string. The hash code is created only from the non-white space characters
        /// which are interpreted as case-insensitive.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="string"/> is null.</exception>
        public int GetHashCode(string @string)
        {
            @string.MustNotBeNull(nameof(@string));

            var hashBuilder = MultiplyAddHashBuilder.Create();

            foreach (var character in @string)
            {
                if (!character.IsWhiteSpace())
                    hashBuilder.CombineIntoHash(char.ToLowerInvariant(character));
            }

            return hashBuilder.BuildHash();
        }
    }
}