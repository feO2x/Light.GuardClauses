using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

/// <summary>
/// Provides meta-information about enum values and the flag bitmask if the enum is marked with the <see cref="FlagsAttribute" />.
/// Can be used to validate that an enum value is valid.
/// </summary>
/// <typeparam name="T">The type of the enum.</typeparam>
public static class EnumInfo<T> where T : struct, Enum
{
    // ReSharper disable StaticMemberInGenericType

    /// <summary>
    /// Gets the value indicating whether the enum type is marked with the flags attribute.
    /// </summary>
    public static readonly bool IsFlagsEnum =
        typeof(T).GetCustomAttribute(Types.FlagsAttributeType) != null;

    /// <summary>
    /// Gets the flags pattern when <see cref="IsFlagsEnum" /> is true. If the enum is not a flags enum, then 0UL is returned.
    /// </summary>
    public static readonly ulong FlagsPattern;

    private static readonly int EnumSize = Unsafe.SizeOf<T>();
    private static readonly T[] EnumConstantsArray;

    /// <summary>
    /// Gets the values of the enum as a read-only collection.
    /// </summary>
    public static ReadOnlyMemory<T> EnumConstants { get; }

    static EnumInfo()
    {
#if NET8_0
        EnumConstantsArray = Enum.GetValues<T>();
#else
        EnumConstantsArray = (T[]) Enum.GetValues(typeof(T));
#endif
        EnumConstants = new ReadOnlyMemory<T>(EnumConstantsArray);

        if (!IsFlagsEnum)
        {
            return;
        }

        for (var i = 0; i < EnumConstantsArray.Length; ++i)
        {
            var convertedValue = ConvertToUInt64(EnumConstantsArray[i]);
            FlagsPattern |= convertedValue;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsValidFlagsValue(T enumValue)
    {
        var convertedValue = ConvertToUInt64(enumValue);
        return (FlagsPattern & convertedValue) == convertedValue;
    }

    private static bool IsValidValue(T parameter)
    {
        var comparer = EqualityComparer<T>.Default;
        for (var i = 0; i < EnumConstantsArray.Length; ++i)
        {
            if (comparer.Equals(EnumConstantsArray[i], parameter))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the specified enum value is valid. This is true if either the enum is a standard enum and the enum value corresponds
    /// to one of the enum constant values or if the enum type is marked with the <see cref="FlagsAttribute" /> and the given value
    /// is a valid combination of bits for this type.
    /// </summary>
    /// <param name="enumValue">The enum value to be checked.</param>
    /// <returns>True if either the enum value is </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidEnumValue(T enumValue) =>
        IsFlagsEnum ? IsValidFlagsValue(enumValue) : IsValidValue(enumValue);

    private static ulong ConvertToUInt64(T value)
    {
        switch (EnumSize)
        {
            case 1: return Unsafe.As<T, byte>(ref value);
            case 2: return Unsafe.As<T, ushort>(ref value);
            case 4: return Unsafe.As<T, uint>(ref value);
            case 8: return Unsafe.As<T, ulong>(ref value);
            default:
                ThrowUnknownEnumSize();
                return 0UL;
        }
    }

    private static void ThrowUnknownEnumSize() => throw new InvalidOperationException(
        $"The enum type \"{typeof(T)}\" has an unknown size of {EnumSize}. This means that the underlying enum type is not one of the supported ones."
    );
}
