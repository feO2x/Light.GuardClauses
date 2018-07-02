using System;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif

namespace Light.GuardClauses
{
    public static partial class Guard
    {
        /// <summary>
        /// Checks if the two specified types are equivalent. This is true when both types are equal or
        /// when one type is a constructed generic type and the other type is the corresponding generic type definition.
        /// </summary>
        /// <param name="type">The first type to be checked.</param>
        /// <param name="other">The other type to be checked.</param>
        /// <returns>
        /// True if both types are null, or if both are equal, or if one type
        /// is a constructed generic type and the other one is the corresponding generic type definition, else false.
        /// </returns>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsEquivalentTypeTo(this Type type, Type other) =>
            ReferenceEquals(type, other) ||
            !(type is null) &&
            other != null &&
            (type == other ||
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45)
             type.IsConstructedGenericType != other.IsConstructedGenericType &&
#else
             type.IsConstructedGenericType() != other.IsConstructedGenericType() &&
#endif
             CheckTypeEquivalency(type, other));

        private static bool CheckTypeEquivalency(Type type, Type other)
        {
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45)
            if (type.IsConstructedGenericType)
#else
			if (type.IsConstructedGenericType())
#endif
                return type.GetGenericTypeDefinition() == other;
            return other.GetGenericTypeDefinition() == type;
        }

#if (NET35 || NET40 || SILVERLIGHT)
/// <summary>
/// Gets a value that indicates whether this object represents a constructed generic type. You can create instances of a constructed generic type.
/// </summary>
/// <param name="type">The type to be checked.</param>
/// <returns>True if this object represents a constructed generic type, else false.</returns>
        public static bool IsConstructedGenericType(this Type type)
        {
            if (type == null)
                return false;

            return type.IsGenericType && !type.ContainsGenericParameters;
        }
#endif
    }
}