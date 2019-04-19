using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Light.GuardClauses.Exceptions;
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

namespace Light.GuardClauses
{
    /// <summary>
    /// Provides meta-information about enum values and the flag bitmask if the enum is marked with the <see cref="FlagsAttribute" />.
    /// Can be used to validate that an enum value is valid.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    public static class EnumInfo<T> where T : Enum
    {
        // ReSharper disable StaticMemberInGenericType

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
#if (NET45 || NETSTANDARD1_0 || NETSTANDARD2_0)
        private static readonly int EnumSize = Unsafe.SizeOf<T>();
#else
        private static readonly bool IsUInt64Enum;
#endif
        private static readonly T[] EnumConstantsArray;

        /// <summary>
        /// Gets the underlying type that is used for the enum (<see cref="int" /> for default enums).
        /// </summary>
        public static readonly Type UnderlyingType;

        /// <summary>
        /// Gets the values of the enum as a read-only collection.
        /// </summary>
#if NETSTANDARD2_0 || NET45
        public static ReadOnlyMemory<T> EnumConstants { get; }
#else
        public static ReadOnlyCollection<T> EnumConstants { get; }
#endif

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
#if NET45 || NETSTANDARD2_0
            EnumConstants = new ReadOnlyMemory<T>(EnumConstantsArray);
#else
            EnumConstants = new ReadOnlyCollection<T>(EnumConstantsArray);
#endif

            if (!IsFlagsEnum)
                return;

#if !(NET45 || NETSTANDARD1_0 || NETSTANDARD2_0)
            IsUInt64Enum = UnderlyingType == Types.UInt64Type;
#endif
            for (var i = 0; i < EnumConstantsArray.Length; ++i)
            {
                var convertedValue = ConvertToUInt64(EnumConstantsArray[i]);
                FlagsPattern |= convertedValue;
            }
        }

#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static bool IsValidFlagsValue(T enumValue)
        {
            var convertedValue = ConvertToUInt64(enumValue);
            return (FlagsPattern & convertedValue) == convertedValue;
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

        private static ulong ConvertToUInt64(T value)
        {
#if NETSTANDARD2_0 || NETSTANDARD1_0 || NET45

            switch (EnumSize)
            {
                case 1: return Unsafe.As<T, byte>(ref value);
                case 2: return Unsafe.As<T, ushort>(ref value);
                case 4: return Unsafe.As<T, uint>(ref value);
                case 8: return Unsafe.As<T, ulong>(ref value);
                default:
                    ThrowUnknownEnumSize();
                    return default;
            }
#else
            if (IsUInt64Enum)
                return Convert.ToUInt64(value);
            return (ulong) Convert.ToInt64(value);
#endif
        }

#if NETSTANDARD2_0 || NETSTANDARD1_0 || NET45
        private static void ThrowUnknownEnumSize()
        {
            throw new InvalidOperationException($"The enum type \"{typeof(T)}\" has an unknown size of {EnumSize}. This means that the underlying enum type is not one of the supported ones.");
        }
#endif
    }
}