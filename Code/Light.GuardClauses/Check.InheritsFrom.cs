#if NET8_0
using System.Diagnostics.CodeAnalysis;
#endif
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

// ReSharper disable RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests

namespace Light.GuardClauses;

public static partial class Check
{
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
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type baseClassOrInterfaceType
    ) =>
        baseClassOrInterfaceType
           .MustNotBeNull(nameof(baseClassOrInterfaceType))
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
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type baseClassOrInterfaceType,
        [NotNull] [ValidatedNotNull] IEqualityComparer<Type> typeComparer
    ) =>
        baseClassOrInterfaceType
           .MustNotBeNull(nameof(baseClassOrInterfaceType))
           .IsInterface ?
            type.Implements(baseClassOrInterfaceType, typeComparer) :
            type.DerivesFrom(baseClassOrInterfaceType, typeComparer);
}
