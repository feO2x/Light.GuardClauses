using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Defines a range that can be used to check if a specified <see cref="IComparable{T}" /> is in between it or not.
    /// </summary>
    /// <typeparam name="T">The type that the range should be applied to.</typeparam>
    public readonly struct Range<T> where T : IComparable<T>
    {
        /// <summary>
        ///     Gets the lower boundary of the range.
        /// </summary>
        public readonly T From;

        /// <summary>
        ///     Gets the upper boundary of the range.
        /// </summary>
        public readonly T To;

        /// <summary>
        ///     Gets the value indicating whether the From value is included in the range.
        /// </summary>
        public readonly bool IsFromInclusive;

        /// <summary>
        ///     Gets the value indicating whether the To value is included in the range.
        /// </summary>
        public readonly bool IsToInclusive;

        private readonly int _expectedLowerBoundaryResult;
        private readonly int _expectedUpperBoundaryResult;

        /// <summary>
        ///     Creates a new instance of <see cref="Range{T}" />.
        /// </summary>
        /// <param name="from">The lower boundary of the range.</param>
        /// <param name="to">The upper boundary of the range.</param>
        /// <param name="isFromInclusive">The value indicating whether <paramref name="from" /> is part of the range.</param>
        /// <param name="isToInclusive">The value indicating whether <paramref name="to" /> is part of the range.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="to" /> is less than <paramref name="from" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Range(T from, T to, bool isFromInclusive, bool isToInclusive)
        {
            to.MustNotBeLessThan(from, nameof(to));

            From = from;
            To = to;
            IsFromInclusive = isFromInclusive;
            IsToInclusive = isToInclusive;

            _expectedLowerBoundaryResult = isFromInclusive ? 0 : 1;
            _expectedUpperBoundaryResult = isToInclusive ? 0 : -1;
        }

        /// <summary>
        ///     Checks if the specified <paramref name="value" /> is within range.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <returns>True if value is within range, otherwise false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValueWithinRange(T value) => value.CompareTo(From) >= _expectedLowerBoundaryResult && value.CompareTo(To) <= _expectedUpperBoundaryResult;

        /// <summary>
        ///     Use this method to create a range in a fluent style using method chaining.
        ///     Defines the lower boundary as an inclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the inclusive lower boundary of the resulting Range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new Range.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFromInfo FromInclusive(T value)
        {
            return new RangeFromInfo(value, true);
        }

        /// <summary>
        ///     Use this method to create a range in a fluent style using method chaining.
        ///     Defines the lower boundary as an exclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the exclusive lower boundary of the resulting Range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new Range.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFromInfo FromExclusive(T value)
        {
            return new RangeFromInfo(value, false);
        }

        /// <summary>
        ///     The nested <see cref="RangeFromInfo" /> can be used to fluently create a <see cref="Range{T}" />.
        /// </summary>
        public struct RangeFromInfo
        {
            private readonly T _from;
            private readonly bool _isFromInclusive;

            /// <summary>
            ///     Creates a new RangeFromInfo.
            /// </summary>
            /// <param name="from">The lower boundary of the range.</param>
            /// <param name="isFromInclusive">The value indicating whether <paramref name="from" /> is part of the range.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public RangeFromInfo(T from, bool isFromInclusive)
            {
                _from = from;
                _isFromInclusive = isFromInclusive;
            }

            /// <summary>
            ///     Use this method to create a Range in a fluent style using method chaining.
            ///     Defines the upper boundary as an exclusive value.
            /// </summary>
            /// <param name="value">The value that indicates the exclusive upper boundary of the resulting Range.</param>
            /// <returns>A new range with the specified upper and lower boundaries.</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     Thrown when <paramref name="value" /> is less than the lower boundary value.
            /// </exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Range<T> ToExclusive(T value)
            {
                return new Range<T>(_from, value, _isFromInclusive, false);
            }

            /// <summary>
            ///     Use this method to create a Range in a fluent style using method chaining.
            ///     Defines the upper boundary as an inclusive value.
            /// </summary>
            /// <param name="value">The value that indicates the inclusive upper boundary of the resulting Range.</param>
            /// <returns>A new range with the specified upper and lower boundaries.</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     Thrown when <paramref name="value" /> is less than the lower boundary value.
            /// </exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Range<T> ToInclusive(T value)
            {
                return new Range<T>(_from, value, _isFromInclusive, true);
            }
        }
    }
}