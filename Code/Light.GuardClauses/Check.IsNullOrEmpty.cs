using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified collection is null or empty.
    /// </summary>
    /// <param name="collection">The collection to be checked.</param>
    /// <returns>True if the collection is null or empty, else false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("=> true, collection:canbenull; => false, collection:notnull")]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this IEnumerable? collection) =>
        collection is null || collection.Count() == 0;
}
