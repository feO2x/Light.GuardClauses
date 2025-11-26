#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

// ReSharper disable RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the type implements the specified interface type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
    /// so that constructed generic types and their corresponding generic type definitions are regarded as equal.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="interfaceType">The interface type that <paramref name="type" /> should implement.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="interfaceType" /> is null.</exception>
    [ContractAnnotation("type:null => halt; interfaceType:null => halt")]
    public static bool Implements(
#if NET8_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type interfaceType
    )
    {
        type.MustNotBeNull();
        interfaceType.MustNotBeNull();

        var implementedInterfaces = type.GetInterfaces();
        for (var i = 0; i < implementedInterfaces.Length; ++i)
        {
            if (interfaceType.IsEquivalentTypeTo(implementedInterfaces[i]))
            {
                return true;
            }
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
#if NET8_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type interfaceType,
        [NotNull] [ValidatedNotNull] IEqualityComparer<Type> typeComparer
    )
    {
        type.MustNotBeNull();
        interfaceType.MustNotBeNull();
        typeComparer.MustNotBeNull();

        var implementedInterfaces = type.GetInterfaces();
        for (var i = 0; i < implementedInterfaces.Length; ++i)
        {
            if (typeComparer.Equals(implementedInterfaces[i], interfaceType))
            {
                return true;
            }
        }

        return false;
    }
}
