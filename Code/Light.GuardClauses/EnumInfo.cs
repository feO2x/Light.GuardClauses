using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
#if NET40 || NET35 || NET35_CF || SILVERLIGHT
using System.Linq;
#endif
#if NETSTANDARD2_0 || NETSTANDARD1_0 || NET45
using System.Reflection;
#endif
#if NETSTANDARD1_0
using Light.GuardClauses.FrameworkExtensions;
#endif
#if NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT
using System.Runtime.CompilerServices;
#endif

// ReSharper disable StaticMemberInGenericType

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
#if NET45 || NETSTANDARD2_0
            typeof(T).GetCustomAttribute(Types.FlagsAttributeType) != null;
#elif NETSTANDARD1_0
            typeof(T).GetTypeInfo().GetCustomAttribute(Types.FlagsAttributeType) != null;
#else
            typeof(T).GetCustomAttributes(Types.FlagsAttributeType, false).FirstOrDefault() != null;
#endif
        /// <summary>
        /// Gets the flags pattern when <see cref="IsFlagsEnum"/> is true. If the enum is not a flags enum, then 0UL is returned.
        /// </summary>
        public static readonly ulong FlagsPattern;
        private static readonly bool IsUInt64Enum;
        private static readonly T[] EnumConstantsArray;

        /// <summary>
        /// Gets the underlying type that is used for the enum (<see cref="int" /> for default enums).
        /// </summary>
        public static readonly Type UnderlyingType;

        /// <summary>
        /// Gets the values of the enum as a read-only collection.
        /// </summary>
        public static ReadOnlyCollection<T> EnumConstants { get; }

        static EnumInfo()
        {
#if !NET35_CF
            EnumConstantsArray = (T[]) Enum.GetValues(typeof(T));
#endif
#if !NETSTANDARD1_0
            var fields = typeof(T).GetFields();
#else
            var fields = typeof(T).GetTypeInfo().DeclaredFields.AsArray();
#endif

            UnderlyingType = fields[0].FieldType;
#if NET35_CF
            EnumConstantsArray = new T[fields.Length - 1];
            for (var i = 1; i < fields.Length; ++i)
            {
                EnumConstantsArray[i - 1] = (T) fields[i].GetValue(null);
            }
#endif
            EnumConstants = new ReadOnlyCollection<T>(EnumConstantsArray);
            if (!IsFlagsEnum)
                return;

            IsUInt64Enum = UnderlyingType == Types.UInt64Type;
            for (var i = 0; i < EnumConstantsArray.Length; ++i)
            {
                if (IsUInt64Enum)
                {
#if !NETSTANDARD1_0
                    FlagsPattern |= EnumConstantsArray[i].ToUInt64(null);
#else
                    FlagsPattern |= Convert.ToUInt64(EnumConstantsArray[i]);
#endif
                }
                else
                {
                    unchecked
                    {
#if !NETSTANDARD1_0
                        FlagsPattern |= (ulong) EnumConstantsArray[i].ToInt64(null);
#else
                        FlagsPattern |= (ulong) Convert.ToInt64(EnumConstantsArray[i]);
#endif
                    }
                }
            }
        }

#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static bool IsValidFlagsValue(T enumValue)
        {
            if (IsUInt64Enum)
            {
#if !NETSTANDARD1_0
                var convertedUInt64Value = enumValue.ToUInt64(null);
#else
                var convertedUInt64Value = Convert.ToUInt64(enumValue);
#endif
                return (FlagsPattern & convertedUInt64Value) == convertedUInt64Value;
            }

            unchecked
            {
#if !NETSTANDARD1_0
            
                var convertedInt64Value = (ulong) enumValue.ToInt64(null);
#else
                var convertedInt64Value = (ulong) Convert.ToInt64(enumValue);
#endif
                return (FlagsPattern & convertedInt64Value) == convertedInt64Value;
            }
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
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsValidEnumValue(T enumValue) =>
            IsFlagsEnum ? IsValidFlagsValue(enumValue) : IsValidValue(enumValue);
    }
}