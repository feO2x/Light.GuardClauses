using System;
using System.Reflection;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Provides assertion extension methods for the <see cref="Type" /> and <see cref="TypeInfo" /> types.
    /// </summary>
    public static class TypeAssertions
    {
        /// <summary>
        ///     Checks if the two specified types are equivalent. This is true when 1) both types are non-generic types and are equal
        ///     or when 2) one type is a constructed generic type and the other type is the corresponding generic type definition.
        /// </summary>
        /// <param name="type">The first type to be checked.</param>
        /// <param name="other">The other type to be checked.</param>
        /// <returns>
        ///     True if both types are null, or if both are non-generic types that are equal, or if one type
        ///     is a constructed generic type and the other one is the corresponding generic type definition, else false.
        /// </returns>
        public static bool IsEquivalentTo(this Type type, Type other)
        {
            if (ReferenceEquals(type, other)) return true;
            if (ReferenceEquals(type, null) || ReferenceEquals(other, null)) return false;

            if (type == other) return true;

            if (type.IsConstructedGenericType == other.IsConstructedGenericType)
                return false;

            if (type.IsConstructedGenericType)
                return type.GetGenericTypeDefinition() == other;
            return other.GetGenericTypeDefinition() == type;
        }

        /// <summary>
        ///     Checks if the specified type is a class. This is true when the <see cref="TypeInfo.IsClass" />
        ///     property returns true and the type is no delegate (i.e. its <see cref="TypeInfo.BaseType" /> is no
        ///     <see cref="MulticastDelegate" />).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsClass(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass && typeInfo.BaseType != typeof(MulticastDelegate);
        }

        /// <summary>
        ///     Checks if the specified type is an interface. This is true when the <see cref="TypeInfo.IsInterface" />
        ///     property returns true.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        /// <summary>
        ///     Checks if the specified type is a delegate. This is true when the <see cref="TypeInfo.IsClass" />
        ///     property returns true and the <see cref="TypeInfo.BaseType" /> is <see cref="MulticastDelegate" />).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsDelegate(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass && typeInfo.BaseType == typeof(MulticastDelegate);
        }

        /// <summary>
        ///     Checks if the specified type is a struct. This is true when the <see cref="TypeInfo.IsValueType" />
        ///     property returns true and the <see cref="TypeInfo.BaseType" /> is <see cref="ValueType" />).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsStruct(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsValueType && typeInfo.BaseType == typeof(ValueType);
        }

        /// <summary>
        ///     Checks if the specified type derives from the other type. Internally, this method uses <see cref="IsEquivalentTo" />
        ///     so that bound generic types and their corresponding generic type definitions are regarded as equal.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="baseClass">The base class that <paramref name="type" /> should derive from.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <returns>True if <paramref name="type" /> derives directly or indirectly from <paramref name="baseClass" />, else false.</returns>
        public static bool IsDerivingFrom(this Type type, Type baseClass)
        {
            type.MustNotBeNull(nameof(type));
            baseClass.MustNotBeNull(nameof(baseClass));

            var currentBaseType = type.GetTypeInfo().BaseType;
            while (currentBaseType != null)
            {
                if (currentBaseType.IsEquivalentTo(baseClass))
                    return true;

                currentBaseType = currentBaseType.GetTypeInfo().BaseType;
            }
            return false;
        }
    }
}