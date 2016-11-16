using System;
using System.Collections.Generic;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    ///     The Equality class contains static methods that help create hash codes and check the equality of values with calls to GetHashCode and Equals.
    /// </summary>
    public static class Equality
    {
        /// <summary>
        ///     One of the two prime numbers that are used to create hash codes. It defaults to 17.
        /// </summary>
        public static int FirstPrime = 17;

        /// <summary>
        ///     The second prime number used to create hash codes. It defaults to 31.
        /// </summary>
        public static int SecondPrime = 31;

        /// <summary>
        ///     Creates a hash code from the two specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        /// <typeparam name="T1">The type of <paramref name="value1" />.</typeparam>
        /// <typeparam name="T2">The type of <paramref name="value2" />.</typeparam>
        /// <param name="value1">The first value to compute the hash code from.</param>
        /// <param name="value2">The second value to compute the hash code from.</param>
        /// <returns>The computed hash code.</returns>
        public static int CreateHashCode<T1, T2>(T1 value1, T2 value2)
        {
            var hash = FirstPrime;
            if (value1 != null) hash = hash * SecondPrime + value1.GetHashCode();
            if (value2 != null) hash = hash * SecondPrime + value2.GetHashCode();
            return hash;
        }

        /// <summary>
        ///     Creates a hash code from the three specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        /// <typeparam name="T1">The type of <paramref name="value1" />.</typeparam>
        /// <typeparam name="T2">The type of <paramref name="value2" />.</typeparam>
        /// <typeparam name="T3">The type of <paramref name="value3" />.</typeparam>
        /// <param name="value1">The first value to compute the hash code from.</param>
        /// <param name="value2">The second value to compute the hash code from.</param>
        /// <param name="value3">The third value to compute the hash code from.</param>
        /// <returns>The computed hash code.</returns>
        public static int CreateHashCode<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            var hash = FirstPrime;
            if (value1 != null) hash = hash * SecondPrime + value1.GetHashCode();
            if (value2 != null) hash = hash * SecondPrime + value2.GetHashCode();
            if (value3 != null) hash = hash * SecondPrime + value3.GetHashCode();
            return hash;
        }

        /// <summary>
        ///     Creates a hash code from the four specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        /// <typeparam name="T1">The type of <paramref name="value1" />.</typeparam>
        /// <typeparam name="T2">The type of <paramref name="value2" />.</typeparam>
        /// <typeparam name="T3">The type of <paramref name="value3" />.</typeparam>
        /// <typeparam name="T4">The type of <paramref name="value4" />.</typeparam>
        /// <param name="value1">The first value to compute the hash code from.</param>
        /// <param name="value2">The second value to compute the hash code from.</param>
        /// <param name="value3">The third value to compute the hash code from.</param>
        /// <param name="value4">The forth value to compute the hash code from.</param>
        /// <returns>The computed hash code.</returns>
        public static int CreateHashCode<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var hash = FirstPrime;
            if (value1 != null) hash = hash * SecondPrime + value1.GetHashCode();
            if (value2 != null) hash = hash * SecondPrime + value2.GetHashCode();
            if (value3 != null) hash = hash * SecondPrime + value3.GetHashCode();
            if (value4 != null) hash = hash * SecondPrime + value4.GetHashCode();
            return hash;
        }

        /// <summary>
        ///     Creates a hash code from the specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        /// <typeparam name="T">The type of values used to create the hash code.</typeparam>
        /// <param name="values">The values used to create the hash code.</param>
        /// <returns>The computed hash code.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values" /> is null.</exception>
        public static int CreateHashCode<T>(params T[] values)
        {
            return CreateHashCode((IEnumerable<T>) values);
        }

        /// <summary>
        ///     Creates a hash code from the specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        /// <typeparam name="T">The type of values used to create the hash code.</typeparam>
        /// <param name="values">The values used to create the hash code.</param>
        /// <returns>The computed hash code.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values" /> is null.</exception>
        public static int CreateHashCode<T>(IEnumerable<T> values)
        {
            var hash = FirstPrime;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var @object in values)
            {
                if (@object != null)
                    hash = hash * SecondPrime + @object.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        ///     Checks if the specified object refences are equal. Calls GetHashCode if both values are not null. Use <see cref="EqualsValueWithHashCode{T}(T,T)" /> instead if you want to compare Value Types with each other.
        /// </summary>
        /// <typeparam name="T">The type of the values to be compared.</typeparam>
        /// <param name="object">The first value.</param>
        /// <param name="other">The value used for comparison.</param>
        /// <returns>True if both objects are considered equal, else false.</returns>
        public static bool EqualsWithHashCode<T>(this T @object, T other)
        {
            if (@object == null)
                return other == null || other.Equals(null);

            if (other == null)
                return @object.Equals(null);

            return @object.GetHashCode() == other.GetHashCode() &&
                   @object.Equals(other);
        }

        /// <summary>
        ///     Checks if the specified values are equal. Calls GetHashCode before calling Equals.
        /// </summary>
        /// <typeparam name="T">The type of the values to be compared. This must be a Value Type.</typeparam>
        /// <param name="value">The first value.</param>
        /// <param name="other">The value used for comparison.</param>
        /// <returns>True if both values are considered equal, else false.</returns>
        public static bool EqualsValueWithHashCode<T>(this T value, T other) where T : struct
        {
            return value.GetHashCode() == other.GetHashCode() &&
                   value.Equals(other);
        }

        /// <summary>
        ///     Checks that the specified equatables are equal. Calls GetHashCode if both values are not null. Use
        ///     <see cref="EqualsValueWithHashCode{T}(IEquatable{T},IEquatable{T})" /> instead if you want to compare Value Types
        ///     with each other.
        /// </summary>
        /// <typeparam name="T">The type of the values to be compared.</typeparam>
        /// <param name="first">The first value.</param>
        /// <param name="second">The value used for comparison.</param>
        /// <returns>True if both values are considered equal, else false.</returns>
        public static bool EqualsWithHashCode<T>(this IEquatable<T> first, IEquatable<T> second)
        {
            if (first == null)
                return second == null || second.Equals(null);

            if (second == null)
                return first.Equals(null);

            return first.GetHashCode() == second.GetHashCode() &&
                   first.Equals(second);
        }

        /// <summary>
        ///     Checks that the specified equatables are equal. Calls GetHashCode before calling Equals.
        /// </summary>
        /// <typeparam name="T">The type of the values to be compared. This must be a Value Type.</typeparam>
        /// <param name="first">The first value.</param>
        /// <param name="second">The value used for comparison.</param>
        /// <returns>True if both values are considered equal, else false.</returns>
        public static bool EqualsValueWithHashCode<T>(this IEquatable<T> first, IEquatable<T> second) where T : struct
        {
            return first.GetHashCode() == second.GetHashCode() &&
                   first.Equals(second);
        }

        /// <summary>
        ///     Checks that the specified values are equal. Uses the specified equality comparer calling GetHashCode and Equals.
        /// </summary>
        /// <typeparam name="T">The type of the values to be compared.</typeparam>
        /// <param name="equalityComparer">The equality comparer used for comparison.</param>
        /// <param name="first">The first value.</param>
        /// <param name="second">The second value.</param>
        /// <returns>True if both values are considered equal, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
        public static bool EqualsWithHashCode<T>(this IEqualityComparer<T> equalityComparer, T first, T second)
        {
            equalityComparer.MustNotBeNull(nameof(equalityComparer));

            return equalityComparer.GetHashCode(first) == equalityComparer.GetHashCode(second) &&
                   equalityComparer.Equals(first, second);
        }
    }
}