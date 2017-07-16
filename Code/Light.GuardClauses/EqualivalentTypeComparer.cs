using System;
using System.Collections.Generic;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Represents an <see cref="IEqualityComparer{T}" /> that uses <see cref="TypeAssertions.IsEquivalentTo" />
    ///     to compare types. This check works like the normal type equality comparison, but when two
    ///     generic types are compared, they are regarded as equal when one of them is a bound generic type
    ///     and the other one is the corresponding generic type definition.
    /// </summary>
    public sealed class EqualivalentTypeComparer : IEqualityComparer<Type>
    {
        /// <summary>
        ///     Checks if the two types are equivalent (using <see cref="TypeAssertions.IsEquivalentTo" />).
        ///     This check works like the normal type equality comparison, but when two
        ///     generic types are compared, they are regarded as equal when one of them is a bound generic type
        ///     and the other one is the corresponding generic type definition.
        /// </summary>
        /// <param name="x">The first type.</param>
        /// <param name="y">The second type.</param>
        public bool Equals(Type x, Type y)
        {
            return x.IsEquivalentTo(y);
        }

        /// <summary>
        ///     Returns the hash code of the given type. When the specified type is a constructed generic type,
        ///     the hash code of the generic type definition is returned instead.
        /// </summary>
        /// <param name="type">The type whose hash code is requested.</param>
        public int GetHashCode(Type type)
        {
            return type == null ? 0 : type.IsConstructedGenericType ? type.GetGenericTypeDefinition().GetHashCode() : type.GetHashCode();
        }
    }
}