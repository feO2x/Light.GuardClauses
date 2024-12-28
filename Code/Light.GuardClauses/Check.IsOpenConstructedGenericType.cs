#if NET8_0
using System.Diagnostics.CodeAnalysis;
#endif
using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the given <paramref name="type" /> is a generic type that has open generic parameters,
    /// but is no generic type definition.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("type:null => halt")]
    // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static bool IsOpenConstructedGenericType([NotNull] [ValidatedNotNull] this Type type) =>
        type.MustNotBeNull(nameof(type)).IsGenericType &&
        type.ContainsGenericParameters &&
        type.IsGenericTypeDefinition == false;
}
