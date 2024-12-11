using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

/// <summary>
/// The <see cref="Check" /> class provides access to all assertions of Light.GuardClauses.
/// </summary>
public static partial class Check
{
    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this double value, double other, double tolerance) =>
        value > other || value.IsApproximately(other, tolerance);

    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the default tolerance of 0.0001.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this double value, double other) =>
        value > other || value.IsApproximately(other);
    
    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this float value, float other, float tolerance) =>
        value > other || value.IsApproximately(other, tolerance);

    /// <summary>
    /// Checks if the specified value is greater than or approximately the same as the other value, using the default tolerance of 0.0001f.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is greater than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this float value, float other) =>
        value > other || value.IsApproximately(other);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this double value, double other, double tolerance) =>
        value < other || value.IsApproximately(other, tolerance);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the default tolerance of 0.0001.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this double value, double other) =>
        value < other || value.IsApproximately(other);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this float value, float other, float tolerance) =>
        value < other || value.IsApproximately(other, tolerance);
    
    /// <summary>
    /// Checks if the specified value is less than or approximately the same as the other value, using the default tolerance of 0.0001f.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns>
    /// True if <paramref name="value" /> is less than <paramref name="other" /> or if their absolute difference
    /// is smaller than 0.0001f, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this float value, float other) =>
        value < other || value.IsApproximately(other);
}