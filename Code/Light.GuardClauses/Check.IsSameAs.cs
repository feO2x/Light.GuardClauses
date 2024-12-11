using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if <paramref name="parameter" /> and <paramref name="other" /> point to the same object.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable StringLiteralTypo
    [ContractAnnotation("parameter:notNull => true, other:notnull; parameter:notNull => false, other:canbenull; other:notnull => true, parameter:notnull; other:notnull => false, parameter:canbenull")]
    // ReSharper restore StringLiteralTypo
    public static bool IsSameAs<T>([NoEnumeration] this T? parameter, [NoEnumeration] T? other) where T : class =>
        ReferenceEquals(parameter, other);
}
