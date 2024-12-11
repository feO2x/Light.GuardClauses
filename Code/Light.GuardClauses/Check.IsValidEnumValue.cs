using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified value is a valid enum value of its type. This is true when the specified value
    /// is one of the constants defined in the enum, or a valid flags combination when the enum type is marked
    /// with the <see cref="FlagsAttribute" />.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="parameter">The enum value to be checked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidEnumValue<T>(this T parameter) where T : struct, Enum =>
        EnumInfo<T>.IsValidEnumValue(parameter);
}
