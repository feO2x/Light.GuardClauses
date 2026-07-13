#if NET8_0_OR_GREATER
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
#if NET8_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type otherType
    ) =>
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
#if NET8_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type otherType,
        [NotNull] [ValidatedNotNull] IEqualityComparer<Type> typeComparer
    ) =>
        typeComparer.MustNotBeNull(nameof(typeComparer)).Equals(type, otherType.MustNotBeNull(nameof(otherType))) ||
        type.InheritsFrom(otherType, typeComparer);
}
