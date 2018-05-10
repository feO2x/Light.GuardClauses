using System;
using System.Collections.Generic;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// The Equality class contains static methods that help create hash codes and check the equality of values with calls to GetHashCode and Equals.
    /// </summary>
    public static class Equality
    {
        /// <summary>
        /// This prime number is used as an initial hash code value when calculating hash codes. Its value is 1322837333.
        /// </summary>
        public const int FirstPrime = 1322837333;

        /// <summary>
        /// The second prime number (397) used for hash code generation. It is applied using the following calculation:
        /// <c>hash = hash * SecondPrime + value.GetHashCode();</c> when the corresponding value is not null.
        /// It is the same value that ReSharper (2017.2.1) uses for hash code generation.
        /// </summary>
        public const int SecondPrime = 397;

        /// <summary>
        /// Creates a hash code from the two specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        public static int CreateHashCode<T1, T2>(T1 value1, T2 value2)
        {
            unchecked
            {
                var hash = FirstPrime;
                if (value1 != null) hash = hash * SecondPrime + value1.GetHashCode();
                if (value2 != null) hash = hash * SecondPrime + value2.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Creates a hash code from the three specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        public static int CreateHashCode<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            unchecked
            {
                var hash = FirstPrime;
                if (value1 != null) hash = hash * SecondPrime + value1.GetHashCode();
                if (value2 != null) hash = hash * SecondPrime + value2.GetHashCode();
                if (value3 != null) hash = hash * SecondPrime + value3.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Creates a hash code from the four specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        public static int CreateHashCode<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            unchecked
            {
                var hash = FirstPrime;
                if (value1 != null) hash = hash * SecondPrime + value1.GetHashCode();
                if (value2 != null) hash = hash * SecondPrime + value2.GetHashCode();
                if (value3 != null) hash = hash * SecondPrime + value3.GetHashCode();
                if (value4 != null) hash = hash * SecondPrime + value4.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Creates a hash code from the specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values" /> is null.</exception>
        public static int CreateHashCode<T>(params T[] values)
        {
            values.MustNotBeNull(nameof(values));
            unchecked
            {
                var hash = FirstPrime;
                for (var i = 0; i < values.Length; ++i)
                {
                    var currentValue = values[i];
                    if (currentValue != null)
                        hash = hash * SecondPrime + currentValue.GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// Creates a hash code from the specified values. It is implemented according to the guidelines of this Stack Overflow answer: http://stackoverflow.com/a/263416/1560623.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values" /> is null.</exception>
        public static int CreateHashCode<T>(IEnumerable<T> values)
        {
            values.MustNotBeNull(nameof(values));
            unchecked
            {
                var hash = FirstPrime;
                if (values is IList<T> list)
                {
                    for (var i = 0; i < list.Count; ++i)
                    {
                        var currentValue = list[i];
                        if (currentValue != null)
                            hash = hash * SecondPrime + currentValue.GetHashCode();
                    }

                    return hash;
                }

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var @object in values)
                    if (@object != null)
                        hash = hash * SecondPrime + @object.GetHashCode();
                return hash;
            }
        }
    }
}