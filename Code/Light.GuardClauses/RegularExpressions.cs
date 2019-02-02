using System.Text.RegularExpressions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Provides regular expressions that are used in string assertions.
    /// </summary>
    public static class RegularExpressions
    {
        /// <summary>
        /// Gets the default regular expression for email validation.
        /// For more information about mail address patterns see https://emailregex.com/".
        /// </summary>
        public static readonly Regex EmailRegex = new Regex(
            @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((((\w+\-?)+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
            RegexOptions.CultureInvariant | RegexOptions.ECMAScript);
    }
}