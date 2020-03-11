using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MultiplyAddHashBuilder(int initialHash) => _hash = initialHash;

        /// <summary>
        /// Combines the given value into the hash using the <see cref="MultiplyAddHash.CombineIntoHash{T}(ref int, T)"/> method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MultiplyAddHashBuilder CombineIntoHash<T>(T value)
        {
            MultiplyAddHash.CombineIntoHash(ref _hash, value);
            return this;
        }

        /// <summary>
        /// Returns the calculated hash code.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BuildHash() => _hash;

        /// <summary>
        /// Initializes a new instance of <see cref="MultiplyAddHashBuilder"/> with the specified initial hash.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MultiplyAddHashBuilder Create(int initialHash = MultiplyAddHash.FirstPrime) => new MultiplyAddHashBuilder(initialHash);

    }
}
