using System;
using System.Collections;
using System.Collections.Generic;
using Light.GuardClauses.FrameworkExtensions;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

/// <summary>
/// Defines a range that can be used to check if a specified <see cref="IComparable{T}" /> is in between it or not.
/// </summary>
/// <typeparam name="T">The type that the range should be applied to.</typeparam>
public readonly struct Range<T> : IEquatable<Range<T>> where T : IComparable<T>
{
    /// <summary>
    /// Gets the lower boundary of the range.
    /// </summary>
    public readonly T From;

    /// <summary>
    /// Gets the upper boundary of the range.
    /// </summary>
    public readonly T To;

    /// <summary>
    /// Gets the value indicating whether the From value is included in the range.
    /// </summary>
    public readonly bool IsFromInclusive;

    /// <summary>
    /// Gets the value indicating whether the To value is included in the range.
    /// </summary>
    public readonly bool IsToInclusive;

    private readonly int _expectedLowerBoundaryResult;
    private readonly int _expectedUpperBoundaryResult;

    /// <summary>
    /// Creates a new instance of <see cref="Range{T}" />.
    /// </summary>
    /// <param name="from">The lower boundary of the range.</param>
    /// <param name="to">The upper boundary of the range.</param>
    /// <param name="isFromInclusive">The value indicating whether <paramref name="from" /> is part of the range.</param>
    /// <param name="isToInclusive">The value indicating whether <paramref name="to" /> is part of the range.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="to" /> is less than <paramref name="from" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Range(T from, T to, bool isFromInclusive = true, bool isToInclusive = true)
    {
        From = from.MustNotBeNullReference(nameof(from));
        To = to.MustNotBeLessThan(from, nameof(to));
        IsFromInclusive = isFromInclusive;
        IsToInclusive = isToInclusive;

        _expectedLowerBoundaryResult = isFromInclusive ? 0 : 1;
        _expectedUpperBoundaryResult = isToInclusive ? 0 : -1;
    }

    /// <summary>
    /// Checks if the specified <paramref name="value" /> is within range.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <returns>True if value is within range, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsValueWithinRange(T value) =>
        value.MustNotBeNullReference(nameof(value)).CompareTo(From) >= _expectedLowerBoundaryResult && 
        value.CompareTo(To) <= _expectedUpperBoundaryResult;

    /// <summary>
    /// Use this method to create a range in a fluent style using method chaining.
    /// Defines the lower boundary as an inclusive value.
    /// </summary>
    /// <param name="value">The value that indicates the inclusive lower boundary of the resulting range.</param>
    /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeFromInfo FromInclusive(T value) => new RangeFromInfo(value, true);

    /// <summary>
    /// Use this method to create a range in a fluent style using method chaining.
    /// Defines the lower boundary as an exclusive value.
    /// </summary>
    /// <param name="value">The value that indicates the exclusive lower boundary of the resulting range.</param>
    /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeFromInfo FromExclusive(T value) => new RangeFromInfo(value, false);

    /// <summary>
    /// The nested <see cref="RangeFromInfo" /> can be used to fluently create a <see cref="Range{T}" />.
    /// </summary>
    public readonly struct RangeFromInfo
    {
        private readonly T _from;
        private readonly bool _isFromInclusive;

        /// <summary>
        /// Creates a new RangeFromInfo.
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
        /// Use this method to create a range in a fluent style using method chaining.
        /// Defines the upper boundary as an exclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the exclusive upper boundary of the resulting range.</param>
        /// <returns>A new range with the specified upper and lower boundaries.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="value" /> is less than the lower boundary value.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Range<T> ToExclusive(T value) => new Range<T>(_from, value, _isFromInclusive, false);

        /// <summary>
        /// Use this method to create a range in a fluent style using method chaining.
        /// Defines the upper boundary as an inclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the inclusive upper boundary of the resulting range.</param>
        /// <returns>A new range with the specified upper and lower boundaries.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="value" /> is less than the lower boundary value.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Range<T> ToInclusive(T value) => new Range<T>(_from, value, _isFromInclusive);
    }

    /// <inheritdoc />
    public override string ToString() => 
        $"Range from {CreateRangeDescriptionText()}";

    /// <summary>
    /// Returns either "inclusive" or "exclusive", depending on whether <see cref="IsFromInclusive"/> is true or false.
    /// </summary>
    public string LowerBoundaryText
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetBoundaryText(IsFromInclusive);
    }

    /// <summary>
    /// Returns either "inclusive" or "exclusive", depending on whether <see cref="IsToInclusive"/> is true or false.
    /// </summary>
    public string UpperBoundaryText
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetBoundaryText(IsToInclusive);
    }

    /// <summary>
    /// Returns a text description of this range with the following pattern: From (inclusive | exclusive) to To (inclusive | exclusive).
    /// </summary>
    public string CreateRangeDescriptionText(string fromToConnectionWord = "to") =>
        From + " (" + LowerBoundaryText + ") " + fromToConnectionWord + ' ' + To + " (" + UpperBoundaryText + ")";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBoundaryText(bool isInclusive) => isInclusive ? "inclusive" : "exclusive";

    /// <inheritdoc />
    public bool Equals(Range<T> other)
    {
        if (IsFromInclusive != other.IsFromInclusive ||
            IsToInclusive != other.IsToInclusive)
            return false;
        var comparer = EqualityComparer<T>.Default;
        return comparer.Equals(From, other.From) &&
               comparer.Equals(To, other.To);
    }

    /// <inheritdoc />
    public override bool Equals(object? other)
    {
        if (other is null) return false;
        return other is Range<T> range && Equals(range);
    }

    /// <inheritdoc />
    public override int GetHashCode() => MultiplyAddHash.CreateHashCode(From, To, IsFromInclusive, IsToInclusive);

    /// <summary>
    /// Checks if two ranges are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Range<T> first, Range<T> second) => first.Equals(second);

    /// <summary>
    /// Checks if two ranges are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Range<T> first, Range<T> second) => first.Equals(second) == false;
}

/// <summary>
/// Provides methods to simplify the creation of <see cref="Range{T}"/> instances.
/// </summary>
public static class Range
{
    /// <summary>
    /// Use this method to create a range in a fluent style using method chaining.
    /// Defines the lower boundary as an inclusive value.
    /// </summary>
    /// <param name="value">The value that indicates the inclusive lower boundary of the resulting range.</param>
    /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<T>.RangeFromInfo FromInclusive<T>(T value) where T : IComparable<T> => new (value, true);

    /// <summary>
    /// Use this method to create a range in a fluent style using method chaining.
    /// Defines the lower boundary as an exclusive value.
    /// </summary>
    /// <param name="value">The value that indicates the exclusive lower boundary of the resulting range.</param>
    /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<T>.RangeFromInfo FromExclusive<T>(T value) where T : IComparable<T> => new (value, false);

    /// <summary>
    /// Creates a range with both boundaries inclusive.
    /// </summary>
    /// <param name="from">The lower boundary of the range.</param>
    /// <param name="to">The upper boundary of the range.</param>
    /// <returns>A new range with both boundaries inclusive.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="to" /> is less than <paramref name="from" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<T> InclusiveBetween<T>(T from, T to) where T : IComparable<T> => new (from, to);

    /// <summary>
    /// Creates a range with both boundaries exclusive.
    /// </summary>
    /// <param name="from">The lower boundary of the range.</param>
    /// <param name="to">The upper boundary of the range.</param>
    /// <returns>A new range with both boundaries exclusive.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="to" /> is less than <paramref name="from" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<T> ExclusiveBetween<T>(T from, T to) where T : IComparable<T> => new (from, to, false, false);

    /// <summary>
    /// Creates a range for the specified enumerable that encompasses all valid indexes.
    /// </summary>
    /// <param name="enumerable">
    /// The count of this enumerable will be used to create the index range. Please ensure that this enumerable
    /// is actually a collection, not a lazy enumerable.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<int> For(IEnumerable enumerable) =>
        new (0, enumerable.Count(), isFromInclusive: true, isToInclusive: false);
    
    /// <summary>
    /// Creates a range for the specified enumerable that encompasses all valid indexes.
    /// </summary>
    /// <param name="enumerable">
    /// The count of this enumerable will be used to create the index range. Please ensure that this enumerable
    /// is actually a collection, not a lazy enumerable.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable"/> is null.</exception>
    public static Range<int> For<T>(IEnumerable<T> enumerable) =>
        new (0, enumerable.GetCount(), isFromInclusive: true, isToInclusive: false);
    
    /// <summary>
    /// Creates a range for the specified span that encompasses all valid indexes.
    /// </summary>
    /// <param name="span">
    /// The length of the span is used to create a valid index range.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<int> For<T>(ReadOnlySpan<T> span) =>
        new (0, span.Length, isFromInclusive: true, isToInclusive: false);
    
    /// <summary>
    /// Creates a range for the specified span that encompasses all valid indexes.
    /// </summary>
    /// <param name="span">
    /// The length of the span is used to create a valid index range.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<int> For<T>(Span<T> span) =>
        new (0 , span.Length, isFromInclusive: true, isToInclusive: false);

    /// <summary>
    /// Creates a range for the specified memory that encompasses all valid indexes.
    /// </summary>
    /// <param name="memory">
    /// The length of the memory is used to create a valid index range.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<int> For<T>(Memory<T> memory) =>
        new (0, memory.Length, isFromInclusive: true, isToInclusive: false);
    
    /// <summary>
    /// Creates a range for the specified memory that encompasses all valid indexes.
    /// </summary>
    /// <param name="memory">
    /// The length of the memory is used to create a valid index range.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<int> For<T>(ReadOnlyMemory<T> memory) =>
        new (0, memory.Length, isFromInclusive: true, isToInclusive: false);
    
    /// <summary>
    /// Creates a range for the specified memory that encompasses all valid indexes.
    /// </summary>
    /// <param name="segment">
    /// The count of the segment is used to create a valid index range.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<int> For<T>(ArraySegment<T> segment) =>
        new (0, segment.Count, isFromInclusive: true, isToInclusive: false);
}
