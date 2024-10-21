using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
#if NET8_0
using System.Diagnostics.CodeAnalysis;
#endif
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

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
        !(type is null) &&
        !(other is null) &&
        (type == other ||
         type.IsConstructedGenericType != other.IsConstructedGenericType &&
         CheckTypeEquivalency(type, other));

    private static bool CheckTypeEquivalency(Type type, Type other)
    {
        if (type.IsConstructedGenericType)
            return type.GetGenericTypeDefinition() == other;
        return other.GetGenericTypeDefinition() == type;
    }

    /// <summary>
    /// Checks if the type implements the specified interface type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
    /// so that constructed generic types and their corresponding generic type definitions are regarded as equal.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="interfaceType">The interface type that <paramref name="type" /> should implement.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="interfaceType" /> is null.</exception>
    [ContractAnnotation("type:null => halt; interfaceType:null => halt")]
    public static bool Implements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        // ReSharper disable RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        [NotNull, ValidatedNotNull] this Type type,
        [NotNull, ValidatedNotNull] Type interfaceType
        // ReSharper restore RedundantNullableFlowAttribute
    )
    {
        type.MustNotBeNull();
        interfaceType.MustNotBeNull();

        var implementedInterfaces = type.GetInterfaces();
        for (var i = 0; i < implementedInterfaces.Length; ++i)
        {
            if (interfaceType.IsEquivalentTypeTo(implementedInterfaces[i]))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the type implements the specified interface type. This overload uses the specified <paramref name="typeComparer" />
    /// to compare the interface types.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="interfaceType">The interface type that <paramref name="type" /> should implement.</param>
    /// <param name="typeComparer">The equality comparer used to compare the interface types.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" />, or <paramref name="interfaceType" />, or <paramref name="typeComparer" /> is null.</exception>
    [ContractAnnotation("type:null => halt; interfaceType:null => halt; typeComparer:null => halt")]
    public static bool Implements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        // ReSharper disable RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        [NotNull, ValidatedNotNull] this Type type,
        [NotNull, ValidatedNotNull] Type interfaceType,
        [NotNull, ValidatedNotNull] IEqualityComparer<Type> typeComparer
        // ReSharper restore RedundantNullableFlowAttribute
    )
    {
        type.MustNotBeNull();
        interfaceType.MustNotBeNull();
        typeComparer.MustNotBeNull();

        var implementedInterfaces = type.GetInterfaces();
        for (var i = 0; i < implementedInterfaces.Length; ++i)
        {
            if (typeComparer.Equals(implementedInterfaces[i], interfaceType))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it implements it. Internally, this
    /// method uses <see cref="IsEquivalentTypeTo" /> so that constructed generic types and their corresponding generic type definitions are regarded as equal.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the interface type that <paramref name="type" /> implements.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; otherType:null => halt")]
    public static bool IsOrImplements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [ValidatedNotNull] this Type type,
        [ValidatedNotNull] Type otherType) =>
        type.IsEquivalentTypeTo(otherType.MustNotBeNull(nameof(otherType))) || type.Implements(otherType);

    /// <summary>
    /// Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it implements it. This overload uses the specified <paramref name="typeComparer" />
    /// to compare the types.
    /// </summary>
    /// ,
    /// <param name="type">The type to be checked.</param>
    /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the interface type that <paramref name="type" /> implements.</param>
    /// <param name="typeComparer">The equality comparer used to compare the interface types.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; otherType:null => halt")]
    public static bool IsOrImplements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [ValidatedNotNull] this Type type,
        [ValidatedNotNull] Type otherType,
        [ValidatedNotNull] IEqualityComparer<Type> typeComparer) =>
        typeComparer.MustNotBeNull(nameof(typeComparer)).Equals(type.MustNotBeNull(nameof(type)), otherType.MustNotBeNull(nameof(otherType))) || type.Implements(otherType, typeComparer);

    /// <summary>
    /// Checks if the specified type derives from the other type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
    /// by default so that constructed generic types and their corresponding generic type definitions are regarded as equal.
    /// </summary>
    /// <param name="type">The type info to be checked.</param>
    /// <param name="baseClass">The base class that <paramref name="type" /> should derive from.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="baseClass" /> is null.</exception>
    [ContractAnnotation("type:null => halt; baseClass:null => halt")]
    public static bool DerivesFrom([ValidatedNotNull] this Type type, [ValidatedNotNull] Type baseClass)
    {
        baseClass.MustNotBeNull(nameof(baseClass));

        var currentBaseType = type.MustNotBeNull(nameof(type)).BaseType;
        while (currentBaseType != null)
        {
            if (currentBaseType.IsEquivalentTypeTo(baseClass))
                return true;

            currentBaseType = currentBaseType.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Checks if the specified type derives from the other type. This overload uses the specified <paramref name="typeComparer" />
    /// to compare the types.
    /// </summary>
    /// <param name="type">The type info to be checked.</param>
    /// <param name="baseClass">The base class that <paramref name="type" /> should derive from.</param>
    /// <param name="typeComparer">The equality comparer used to compare the types.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" />, or <paramref name="baseClass" />, or <paramref name="typeComparer" /> is null.</exception>
    [ContractAnnotation("type:null => halt; baseClass:null => halt; typeComparer:null => halt")]
    public static bool DerivesFrom([ValidatedNotNull] this Type type, [ValidatedNotNull] Type baseClass, [ValidatedNotNull] IEqualityComparer<Type> typeComparer)
    {
        baseClass.MustNotBeNull(nameof(baseClass));
        typeComparer.MustNotBeNull(nameof(typeComparer));

        var currentBaseType = type.MustNotBeNull(nameof(type)).BaseType;
        while (currentBaseType != null)
        {
            if (typeComparer.Equals(currentBaseType, baseClass))
                return true;

            currentBaseType = currentBaseType.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it derives from it. Internally, this
    /// method uses <see cref="IsEquivalentTypeTo" /> so that constructed generic types and their corresponding generic type definitions are regarded as equal.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the base class type where <paramref name="type" /> derives from.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; otherType:null => halt")]
    public static bool IsOrDerivesFrom([ValidatedNotNull] this Type type, [ValidatedNotNull] Type otherType) =>
        type.IsEquivalentTypeTo(otherType.MustNotBeNull(nameof(otherType))) || type.DerivesFrom(otherType);

    /// <summary>
    /// Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it derives from it. This overload uses the specified <paramref name="typeComparer" />
    /// to compare the types.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the base class type where <paramref name="type" /> derives from.</param>
    /// <param name="typeComparer">The equality comparer used to compare the types.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" />, or <paramref name="otherType" />, or <paramref name="typeComparer" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; otherType:null => halt; typeComparer:null => halt")]
    public static bool IsOrDerivesFrom([ValidatedNotNull] this Type type, [ValidatedNotNull] Type otherType, [ValidatedNotNull] IEqualityComparer<Type> typeComparer) =>
        typeComparer.MustNotBeNull(nameof(typeComparer)).Equals(type, otherType.MustNotBeNull(nameof(otherType))) || type.DerivesFrom(otherType, typeComparer);


    /// <summary>
    /// Checks if the given type derives from the specified base class or interface type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
    /// so that constructed generic types and their corresponding generic type definitions are regarded as equal.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="baseClassOrInterfaceType">The type describing an interface or base class that <paramref name="type" /> should derive from or implement.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="baseClassOrInterfaceType" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; baseClassOrInterfaceType:null => halt")]
    public static bool InheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif        
        [ValidatedNotNull] this Type type,
        [ValidatedNotNull] Type baseClassOrInterfaceType) =>
        baseClassOrInterfaceType.MustNotBeNull(nameof(baseClassOrInterfaceType))
                                .IsInterface ?
            type.Implements(baseClassOrInterfaceType) :
            type.DerivesFrom(baseClassOrInterfaceType);

    /// <summary>
    /// Checks if the given type derives from the specified base class or interface type. This overload uses the specified <paramref name="typeComparer" />
    /// to compare the types.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="baseClassOrInterfaceType">The type describing an interface or base class that <paramref name="type" /> should derive from or implement.</param>
    /// <param name="typeComparer">The equality comparer used to compare the types.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" />, or <paramref name="baseClassOrInterfaceType" />, or <paramref name="typeComparer" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; baseClassOrInterfaceType:null => halt; typeComparer:null => halt")]
    public static bool InheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif        
        [ValidatedNotNull] this Type type,
        [ValidatedNotNull] Type baseClassOrInterfaceType,
        [ValidatedNotNull] IEqualityComparer<Type> typeComparer) =>
        baseClassOrInterfaceType.MustNotBeNull(nameof(baseClassOrInterfaceType))
                                .IsInterface ?
            type.Implements(baseClassOrInterfaceType, typeComparer) :
            type.DerivesFrom(baseClassOrInterfaceType, typeComparer);

    /// <summary>
    /// Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it derives from it or implements it.
    /// Internally, this method uses <see cref="IsEquivalentTypeTo" /> so that constructed generic types and their corresponding generic type definitions
    /// are regarded as equal.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the base class type where <paramref name="type" /> derives from.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; otherType:null => halt")]
    public static bool IsOrInheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [ValidatedNotNull] this Type type,
        [ValidatedNotNull] Type otherType) =>
        type.IsEquivalentTypeTo(otherType.MustNotBeNull(nameof(otherType))) || type.InheritsFrom(otherType);


    /// <summary>
    /// Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it derives from it or implements it.
    /// This overload uses the specified <paramref name="typeComparer" /> to compare the types.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the base class type where <paramref name="type" /> derives from.</param>
    /// <param name="typeComparer">The equality comparer used to compare the types.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt; otherType:null => halt; typeComparer:null => halt")]
    public static bool IsOrInheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif        
        [ValidatedNotNull] this Type type,
        [ValidatedNotNull] Type otherType,
        [ValidatedNotNull] IEqualityComparer<Type> typeComparer) =>
        typeComparer.MustNotBeNull(nameof(typeComparer)).Equals(type, otherType.MustNotBeNull(nameof(otherType))) || type.InheritsFrom(otherType, typeComparer);


    /// <summary>
    /// Checks if the given <paramref name="type" /> is a generic type that has open generic parameters,
    /// but is no generic type definition.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt")]
    public static bool IsOpenConstructedGenericType([ValidatedNotNull] this Type type) =>
        type.MustNotBeNull(nameof(type)).IsGenericType &&
        type.ContainsGenericParameters &&
        type.IsGenericTypeDefinition == false;
}