using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEquivalentTypeTo(this Type? type, Type? other) =>
        ReferenceEquals(type, other) ||
        (type is not null &&
         other is not null &&
         (type == other ||
          (type.IsConstructedGenericType != other.IsConstructedGenericType &&
           CheckTypeEquivalency(type, other))));

    private static bool CheckTypeEquivalency(Type type, Type other)
    {
        if (type.IsConstructedGenericType)
        {
            return type.GetGenericTypeDefinition() == other;
        }

        return other.GetGenericTypeDefinition() == type;
    }
}
