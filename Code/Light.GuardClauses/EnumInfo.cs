using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Provides metainformation about enum values and the flag bitmask if the enum is marked with the <see cref="FlagsAttribute" />.
    /// Can be used to validate that an enum value is valid.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    public static class EnumInfo<T> where T : struct, IComparable, IFormattable
#if !NETSTANDARD1_0
        , IConvertible
#endif
    {
        /// <summary>
        /// Gets the value indicating whether the enum type is marked with the flags attribute.
        /// </summary>
        public static readonly bool IsFlagsEnum =
#if !NETSTANDARD1_0
            typeof(T).GetCustomAttribute(Types.FlagsAttributeType) != null;
#else
            typeof(T).GetTypeInfo().GetCustomAttribute(Types.FlagsAttributeType) != null;
#endif

        // ReSharper disable StaticMemberInGenericType
        private static readonly long Int64FlagsPattern;
        private static readonly ulong UInt64FlagsPattern;
        private static readonly bool IsUInt64FlagsPattern;

        private static readonly T[] EnumConstantsArray;

        /// <summary>
        /// Gets the underlying type that is used for the enum (<see cref="int" /> for default enums).
        /// </summary>
        public static readonly Type UnderlyingType;
        // ReSharper restore StaticMemberInGenericType

        /// <summary>
        /// Gets the values of the enum as an read-only list.
        /// </summary>
        public static IReadOnlyList<T> EnumConstants => EnumConstantsArray;

        static EnumInfo()
        {
            EnumConstantsArray = (T[]) Enum.GetValues(typeof(T));
#if !NETSTANDARD1_0
            var fields = typeof(T).GetFields();
#else
            var fields = typeof(T).GetTypeInfo().DeclaredFields.AsArray();
#endif

            UnderlyingType = fields[0].FieldType;
            if (!IsFlagsEnum)
                return;

            IsUInt64FlagsPattern = UnderlyingType == Types.UInt64Type;
            for (var i = 0; i < EnumConstantsArray.Length; ++i)
            {
                if (IsUInt64FlagsPattern)
#if !NETSTANDARD1_0
                    UInt64FlagsPattern |= EnumConstantsArray[i].ToUInt64(null);
#else
                    UInt64FlagsPattern |= Convert.ToUInt64(EnumConstantsArray[i]);
#endif
                else
#if !NETSTANDARD1_0
                    Int64FlagsPattern |= EnumConstantsArray[i].ToInt64(null);
#else
                    Int64FlagsPattern |= Convert.ToInt64(EnumConstantsArray[i]);
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidFlagsValue(T enumValue)
        {
            if (IsUInt64FlagsPattern)
            {
#if !NETSTANDARD1_0
                var convertedUInt64Value = enumValue.ToUInt64(null);
#else
                var convertedUInt64Value = Convert.ToUInt64(enumValue);
#endif
                return (UInt64FlagsPattern & convertedUInt64Value) == convertedUInt64Value;
            }

#if !NETSTANDARD1_0
            var convertedInt64Value = enumValue.ToInt64(null);
#else
            var convertedInt64Value = Convert.ToInt64(enumValue);
#endif
            return (Int64FlagsPattern & convertedInt64Value) == convertedInt64Value;
        }

        private static bool IsValidValue(T parameter)
        {
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < EnumConstantsArray.Length; ++i)
                if (comparer.Equals(EnumConstantsArray[i], parameter))
                    return true;

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
        public static bool IsValidEnumValue(T enumValue)
        {
            return IsFlagsEnum ? IsValidFlagsValue(enumValue) : IsValidValue(enumValue);
        }
    }
}