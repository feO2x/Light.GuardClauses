using System;
using System.Reflection;
using Light.GuardClauses.FrameworkExtensions;

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
        ///     Checks if the specified type is a class. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and the type is no delegate (i.e. its <see cref="TypeInfo.BaseType" /> is no
        ///     <see cref="MulticastDelegate" />).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsClass(this Type type)
        {
            return type.GetTypeInfo().IsClass();
        }

        /// <summary>
        ///     Checks if the specified type info describes a class. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and the type is no delegate (i.e. its <see cref="TypeInfo.BaseType" /> is no
        ///     <see cref="MulticastDelegate" />).
        /// </summary>
        /// <param name="typeInfo">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsClass(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull().IsClass && typeInfo.BaseType != typeof(MulticastDelegate);
        }

        /// <summary>
        ///     Checks if the specified type is an interface. This is true when <see cref="TypeInfo.IsInterface" />
        ///     returns true.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        /// <summary>
        ///     Checks if the specified type is a delegate. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and <see cref="TypeInfo.BaseType" /> is the <see cref="MulticastDelegate" />) type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsDelegate(this Type type)
        {
            return type.GetTypeInfo().IsDelegate();
        }

        /// <summary>
        ///     Checks if the specified type info describes a delegate. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and <see cref="TypeInfo.BaseType" /> is the <see cref="MulticastDelegate" />) type.
        /// </summary>
        /// <param name="typeInfo">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsDelegate(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull().IsClass && typeInfo.BaseType == typeof(MulticastDelegate);
        }

        /// <summary>
        ///     Checks if the specified type is a struct. This is true when <see cref="TypeInfo.IsValueType" />
        ///     returns true and <see cref="TypeInfo.IsEnum" /> returns false.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsStruct(this Type type)
        {
            return type.GetTypeInfo().IsStruct();
        }

        /// <summary>
        ///     Checks if the specified type info describes a struct. This is true when <see cref="TypeInfo.IsValueType" />
        ///     returns true and <see cref="TypeInfo.IsEnum" /> returns false.
        /// </summary>
        /// <param name="typeInfo">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsStruct(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull().IsValueType && typeInfo.IsEnum == false;
        }

        /// <summary>
        ///     Checks if the specified type is an enum. This is true when <see cref="TypeInfo.IsEnum" />
        ///     returns true.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        /// <summary>
        ///     Checks if the specified type is a reference type. This is true when <see cref="TypeInfo.IsValueType" />
        ///     return false.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        /// <returns>True if the specified type is a class, delegate, or interface, else false.</returns>
        public static bool IsReferenceType(this Type type)
        {
            return type.GetTypeInfo().IsValueType == false;
        }

        /// <summary>
        ///     Checks if the specified type info describes a reference type. This is true when <see cref="TypeInfo.IsValueType" />
        ///     return false.
        /// </summary>
        /// <param name="typeInfo">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        /// <returns>True if the specified type is a class, delegate, or interface, else false.</returns>
        public static bool IsReferenceType(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull().IsValueType == false;
        }

        /// <summary>
        ///     Checks if the specified type is a value type. This is true when the <see cref="TypeInfo.IsValueType" />
        ///     property returns true.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
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
            baseClass.MustNotBeNull(nameof(baseClass));

            if (baseClass.IsClass() == false) return false;

            var currentBaseType = type.GetTypeInfo().BaseType;
            while (currentBaseType != null)
            {
                if (currentBaseType.IsEquivalentTo(baseClass))
                    return true;

                currentBaseType = currentBaseType.GetTypeInfo().BaseType;
            }
            return false;
        }

        /// <summary>
        ///     Checks if the specified type implements the given interface type. Internally, this method uses <see cref="IsEquivalentTo" />
        ///     so that bound generic types and their corresponding generic type defintions are regarded as equal.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="interfaceType">The interface type that <paramref name="type" /> should implement.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <returns>True if <paramref name="type" /> implements <paramref name="interfaceType" /> directly or indirectly, else false.</returns>
        public static bool IsImplementing(this Type type, Type interfaceType)
        {
            interfaceType.MustNotBeNull(nameof(interfaceType));
            if (interfaceType.IsInterface() == false) return false;

            var implementedInterfaces = type.GetTypeInfo().ImplementedInterfaces.AsReadOnlyList();
            for (var i = 0; i < implementedInterfaces.Count; i++)
            {
                if (implementedInterfaces[i].IsEquivalentTo(interfaceType))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks if the given type derives from the specified base class or interface type. Internally, this method uses <see cref="IsEquivalentTo" />
        ///     so that bound generic types and their corresponding generic type defintions are regarded as equal.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="baseClassOrInterfaceType">The type describing an interface or base class that <paramref name="type" /> should derive from or implement.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public static bool IsDerivingFromOrImplementing(this Type type, Type baseClassOrInterfaceType)
        {
            return baseClassOrInterfaceType.MustNotBeNull(nameof(baseClassOrInterfaceType))
                                           .IsInterface()
                       ? type.IsImplementing(baseClassOrInterfaceType)
                       : baseClassOrInterfaceType.IsClass() && type.IsDerivingFrom(baseClassOrInterfaceType);
        }
    }
}