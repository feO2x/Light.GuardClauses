using System.Text.RegularExpressions;

namespace Light.GuardClauses;

/// <summary>
/// Provides regular expressions that are used in string assertions.
/// </summary>
public static class RegularExpressions
{
    /// <summary>
    /// Gets the default regular expression for email validation.
    /// This pattern is based on https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/ and
    /// was modified to satisfy all tests of https://blogs.msdn.microsoft.com/testing123/2009/02/06/email-address-test-cases/.
    /// </summary>
    public static readonly Regex EmailRegex = new Regex(
        @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((((\w+\-?)+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
        RegexOptions.CultureInvariant | RegexOptions.ECMAScript);
}