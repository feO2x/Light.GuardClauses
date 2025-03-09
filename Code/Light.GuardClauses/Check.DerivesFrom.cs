#if NET8_0
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
    /// Checks if the specified type derives from the other type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
    /// by default so that constructed generic types and their corresponding generic type definitions are regarded as equal.
    /// </summary>
    /// <param name="type">The type info to be checked.</param>
    /// <param name="baseClass">The base class that <paramref name="type" /> should derive from.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="baseClass" /> is null.</exception>
    [ContractAnnotation("type:null => halt; baseClass:null => halt")]
    public static bool DerivesFrom(
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type baseClass
    )
    {
        baseClass.MustNotBeNull(nameof(baseClass));

        var currentBaseType = type.MustNotBeNull(nameof(type)).BaseType;
        while (currentBaseType != null)
        {
            if (currentBaseType.IsEquivalentTypeTo(baseClass))
            {
                return true;
            }

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
    public static bool DerivesFrom(
        [NotNull] [ValidatedNotNull] this Type type,
        [NotNull] [ValidatedNotNull] Type baseClass,
        [NotNull] [ValidatedNotNull] IEqualityComparer<Type> typeComparer
    )
    {
        baseClass.MustNotBeNull(nameof(baseClass));
        typeComparer.MustNotBeNull(nameof(typeComparer));

        var currentBaseType = type.MustNotBeNull(nameof(type)).BaseType;
        while (currentBaseType != null)
        {
            if (typeComparer.Equals(currentBaseType, baseClass))
            {
                return true;
            }

            currentBaseType = currentBaseType.BaseType;
        }

        return false;
    }
}
