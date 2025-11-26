using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string is an email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />.
    /// </summary>
    /// <param name="emailAddress">The string to be checked if it is an email address.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("emailAddress:null => false")]
    public static bool IsEmailAddress([NotNullWhen(true)] this string? emailAddress) =>
        emailAddress != null && RegularExpressions.EmailRegex.IsMatch(emailAddress);

    /// <summary>
    /// Checks if the specified string is an email address using the provided regular expression for validation.
    /// </summary>
    /// <param name="emailAddress">The string to be checked.</param>
    /// <param name="emailAddressPattern">The regular expression that determines whether the input string is an email address.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("emailAddress:null => false; emailAddressPattern:null => halt")]
    public static bool IsEmailAddress([NotNullWhen(true)] this string? emailAddress, Regex emailAddressPattern) =>
        emailAddress != null && emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern)).IsMatch(emailAddress);

#if NET8_0_OR_GREATER
    /// <summary>
    /// Checks if the specified span is an email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />.
    /// </summary>
    /// <param name="emailAddress">The span to be checked.</param>
    public static bool IsEmailAddress(this Span<char> emailAddress) =>
        RegularExpressions.EmailRegex.IsMatch(emailAddress);

    /// <summary>
    /// Checks if the specified span is an email address using the provided regular expression for validation.
    /// </summary>
    /// <param name="emailAddress">The span to be checked.</param>
    /// <param name="emailAddressPattern">The regular expression that determines whether the input string is an email address.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    public static bool IsEmailAddress(this Span<char> emailAddress, Regex emailAddressPattern) =>
        emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern)).IsMatch(emailAddress);

    /// <summary>
    /// Checks if the specified span is an email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />.
    /// </summary>
    /// <param name="emailAddress">The span to be checked.</param>
    public static bool IsEmailAddress(this ReadOnlySpan<char> emailAddress) =>
        RegularExpressions.EmailRegex.IsMatch(emailAddress);

    /// <summary>
    /// Checks if the specified span is an email address using the provided regular expression for validation.
    /// </summary>
    /// <param name="emailAddress">The span to be checked.</param>
    /// <param name="emailAddressPattern">The regular expression that determines whether the input string is an email address.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    public static bool IsEmailAddress(this ReadOnlySpan<char> emailAddress, Regex emailAddressPattern) =>
        emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern)).IsMatch(emailAddress);
    
    /// <summary>
    /// Checks if the specified character memory is an email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />.
    /// </summary>
    /// <param name="emailAddress">The character memory to be checked.</param>
    public static bool IsEmailAddress(this Memory<char> emailAddress) =>
        RegularExpressions.EmailRegex.IsMatch(emailAddress.Span);
    
    /// <summary>
    /// Checks if the specified character memory is an email address using the provided regular expression for validation.
    /// </summary>
    /// <param name="emailAddress">The character memory to be checked.</param>
    /// <param name="emailAddressPattern">The regular expression that determines whether the input string is an email address.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    public static bool IsEmailAddress(this Memory<char> emailAddress, Regex emailAddressPattern) =>
        emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern)).IsMatch(emailAddress.Span);
    
    /// <summary>
    /// Checks if the specified character memory is an email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex" />.
    /// </summary>
    /// <param name="emailAddress">The character memory to be checked.</param>
    public static bool IsEmailAddress(this ReadOnlyMemory<char> emailAddress) =>
        RegularExpressions.EmailRegex.IsMatch(emailAddress.Span);
    
    /// <summary>
    /// Checks if the specified character memory is an email address using the provided regular expression for validation.
    /// </summary>
    /// <param name="emailAddress">The character memory to be checked.</param>
    /// <param name="emailAddressPattern">The regular expression that determines whether the input string is an email address.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern" /> is null.</exception>
    public static bool IsEmailAddress(this ReadOnlyMemory<char> emailAddress, Regex emailAddressPattern) =>
        emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern)).IsMatch(emailAddress.Span);
#endif
}
