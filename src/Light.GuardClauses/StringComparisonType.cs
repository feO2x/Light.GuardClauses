namespace Light.GuardClauses;

/// <summary>
/// Specifies the culture, case , and sort rules when comparing strings.
/// </summary>
/// <remarks>
/// This enum is en extension of <see cref="System.StringComparison" />, adding
/// capabilities to ignore white space when making string equality comparisons.
/// See the <see cref="Check.Equals(string,string,StringComparisonType)" /> when
/// you want to compare in such a way.
/// </remarks>
public enum StringComparisonType
{
    /// <summary>
    /// Compare strings using culture-sensitive sort rules and the current culture.
    /// </summary>
    CurrentCulture = 0,

    /// <summary>
    /// Compare strings using culture-sensitive sort rules, the current culture, and
    /// ignoring the case of the strings being compared.
    /// </summary>
    CurrentCultureIgnoreCase = 1,

    /// <summary>
    /// Compare strings using culture-sensitive sort rules and the invariant culture.
    /// </summary>
    InvariantCulture = 2,

    /// <summary>
    /// Compare strings using culture-sensitive sort rules, the invariant culture, and
    /// ignoring the case of the strings being compared.
    /// </summary>
    InvariantCultureIgnoreCase = 3,

    /// <summary>
    /// Compare strings using ordinal sort rules.
    /// </summary>
    Ordinal = 4,

    /// <summary>
    /// Compare strings using ordinal sort rules and ignoring the case of the strings
    /// being compared.
    /// </summary>
    OrdinalIgnoreCase = 5,

    /// <summary>
    /// Compare strings using ordinal sort rules and ignoring the white space characters
    /// of the strings being compared.
    /// </summary>
    OrdinalIgnoreWhiteSpace = 6,

    /// <summary>
    /// Compare strings using ordinal sort rules, ignoring the case and ignoring the
    /// white space characters of the strings being compared.
    /// </summary>
    OrdinalIgnoreCaseIgnoreWhiteSpace = 7,
}
