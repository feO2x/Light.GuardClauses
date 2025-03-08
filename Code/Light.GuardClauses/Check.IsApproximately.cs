using System;
using System.Runtime.CompilerServices;
#if NET8_0
using System.Numerics;
#endif

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this double value, double other, double tolerance) =>
        Math.Abs(value - other) < tolerance;

    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than 0.0001, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this double value, double other) =>
        Math.Abs(value - other) < 0.0001;

    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this float value, float other, float tolerance) =>
        Math.Abs(value - other) < tolerance;

    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001f.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than 0.0001f, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this float value, float other) =>
        Math.Abs(value - other) < 0.0001f;

#if NET8_0
    /// <summary>
    /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
    /// </summary>
    /// <param name="value">The first value to be compared.</param>
    /// <param name="other">The second value to be compared.</param>
    /// <param name="tolerance">The tolerance indicating how much the two values may differ from each other.</param>
    /// <typeparam name="T">The type that implements the <see cref="INumber{T}" /> interface.</typeparam>
    /// <returns>
    /// True if <paramref name="value" /> and <paramref name="other" /> are equal or if their absolute difference
    /// is smaller than the given <paramref name="tolerance" />, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately<T>(this T value, T other, T tolerance) where T : INumber<T> =>
        T.Abs(value - other) < tolerance;
#endif
}
