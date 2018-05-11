#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// Represents a builder for the <see cref="MultiplyAddHash"/> algorithm that does not allocate.
    /// Should only be used in cases where the overload for sixteen values is not enough or a dedicated
    /// initial hash must be provided (e.g. for test reasons).
    /// Instantiate the builder with the <see cref="Create"/> method. You have to instantiate a builder
    /// for each hash code that you want to calculate.
    /// </summary>
    public struct MultiplyAddHashBuilder
    {
        private int _hash;

#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private MultiplyAddHashBuilder(int initialHash) => _hash = initialHash;

        /// <summary>
        /// Combines the given value into the hash using the <see cref="MultiplyAddHash.CombineIntoHash{T}(ref int, T)"/> method.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public MultiplyAddHashBuilder CombineIntoHash<T>(T value)
        {
            MultiplyAddHash.CombineIntoHash(ref _hash, value);
            return this;
        }

        /// <summary>
        /// Returns the calculated hash code.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int BuildHash() => _hash;

        /// <summary>
        /// Initializes a new instance of <see cref="MultiplyAddHashBuilder"/> with the specified initial hash.
        /// </summary>
        /// <param name="initialHash"></param>
        /// <returns></returns>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static MultiplyAddHashBuilder Create(int initialHash = MultiplyAddHash.FirstPrime) => new MultiplyAddHashBuilder(initialHash);

    }
}
