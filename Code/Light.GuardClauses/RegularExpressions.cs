using System.Text.RegularExpressions;

namespace Light.GuardClauses;

/// <summary>
/// Provides regular expressions that are used in string assertions.
/// </summary>
#if NET8_0_OR_GREATER
public static partial class RegularExpressions
#else
public static class RegularExpressions
#endif
{
    /// <summary>
    /// Gets the string that represents the <see cref="EmailRegex" />.
    /// </summary>
    // This is an AI-generated regex. I don't have any clue and I find it way to complex to ever understand it.
    public const string EmailRegexText =
        @"^(?:(?:""(?:(?:[^""\\]|\\.)*)""|[\p{L}\p{N}!#$%&'*+\-/=?^_`{|}~-]+(?:\.[\p{L}\p{N}!#$%&'*+\-/=?^_`{|}~-]+)*)@(?:(?:[A-Za-z0-9](?:[A-Za-z0-9\-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}|(?:\[(?:IPv6:[0-9A-Fa-f:.]+)\])|(?:25[0-5]|2[0-4]\d|[01]?\d?\d)(?:\.(?:25[0-5]|2[0-4]\d|[01]?\d?\d)){3}))$";

    /// <summary>
    /// Gets the default regular expression for email validation.
    /// This pattern is based on https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/ and
    /// was modified to satisfy all tests of https://blogs.msdn.microsoft.com/testing123/2009/02/06/email-address-test-cases/.
    /// </summary>
    public static readonly Regex EmailRegex =
#if NET8_0_OR_GREATER
        GenerateEmailRegex();

    [GeneratedRegex(EmailRegexText, RegexOptions.ECMAScript | RegexOptions.CultureInvariant)]
    private static partial Regex GenerateEmailRegex();
#else
        new (EmailRegexText, RegexOptions.ECMAScript | RegexOptions.CultureInvariant | RegexOptions.Compiled);
#endif
}
