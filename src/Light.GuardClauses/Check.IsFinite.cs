using System.Runtime.CompilerServices;
#if NET8_0_OR_GREATER
using System.Numerics;
#endif

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified single-precision floating-point value is finite.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFinite(this float parameter) => parameter >= float.MinValue && parameter <= float.MaxValue;

    /// <summary>
    /// Checks if the specified double-precision floating-point value is finite.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFinite(this double parameter) => parameter >= double.MinValue && parameter <= double.MaxValue;

#if NET8_0_OR_GREATER
    /// <summary>
    /// Checks if the specified IEEE 754 floating-point value is finite.
    /// </summary>
    /// <typeparam name="T">The IEEE 754 floating-point type.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFinite<T>(this T parameter) where T : IFloatingPointIeee754<T> => T.IsFinite(parameter);
#endif
}
