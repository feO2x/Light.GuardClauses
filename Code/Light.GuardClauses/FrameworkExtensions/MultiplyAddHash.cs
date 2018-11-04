#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// The <see cref="MultiplyAddHash" /> class represents a simple non-cryptographic hash function that uses a prime number
    /// as seed and then manipulates this value by constantly performing <c>hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);</c>
    /// for each given value. It is implemented according to the guidelines of Jon Skeet as stated in this Stack Overflow
    /// answer: http://stackoverflow.com/a/263416/1560623. IMPORTANT: do not persist any hash codes and rely on them
    /// to stay the same. Hash codes should only be used in memory within a single process session, usually for the use
    /// in dictionaries (hash tables) and sets. This algorithm, especially the prime numbers can change even during minor
    /// releases of Light.GuardClauses.
    /// </summary>
    public static class MultiplyAddHash
    {
        /// <summary>
        /// This prime number is used as an initial (seed) value when calculating hash codes. Its value is 1322837333.
        /// </summary>
        public const int FirstPrime = 1322837333;

        /// <summary>
        /// The second prime number (397) used for hash code generation. It is applied using the following statement:
        /// <c>hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);</c>.
        /// It is the same value that ReSharper (2018.1) uses for hash code generation.
        /// </summary>
        public const int SecondPrime = 397;

        /// <summary>
        /// Creates a hash code from the two specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2>(T1 value1, T2 value2)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the three specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the four specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the five specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the six specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the seven specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the eight specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the nine specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the ten specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the eleven specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the eleven specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the thirteen specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the fourteen specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            CombineIntoHash(ref hash, value14);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the fifteen specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            CombineIntoHash(ref hash, value14);
            CombineIntoHash(ref hash, value15);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the sixteen specified values.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15, T16 value16)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            CombineIntoHash(ref hash, value14);
            CombineIntoHash(ref hash, value15);
            CombineIntoHash(ref hash, value16);
            return hash;
        }

        /// <summary>
        /// Mutates the given hash with the specified value using the following statement:
        /// <c>hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);</c>.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void CombineIntoHash<T>(ref int hash, T value) => hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);
    }
}