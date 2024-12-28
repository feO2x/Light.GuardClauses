using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using System.Runtime.CompilerServices;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="value" /> contains <paramref name="parameter" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf([NotNull, ValidatedNotNull] this string? parameter, string value, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (value.MustNotBeNull(nameof(value), message).Contains(parameter.MustNotBeNull(parameterName, message)))
            Throw.Substring(parameter, value, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="value" /> contains <paramref name="parameter" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf([NotNull, ValidatedNotNull] this string? parameter, string value, Func<string?, string, Exception> exceptionFactory)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || value is null || value.Contains(parameter))
            Throw.CustomException(exceptionFactory, parameter, value!);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="value" /> contains <paramref name="parameter" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf([NotNull, ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (value.MustNotBeNull(nameof(value), message).IndexOf(parameter.MustNotBeNull(parameterName, message), comparisonType) != -1)
            Throw.Substring(parameter, value, comparisonType, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="value" /> contains <paramref name="parameter" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustNotBeSubstringOf([NotNull, ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, Func<string?, string, StringComparison, Exception> exceptionFactory)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (parameter is null || value is null || value.IndexOf(parameter, comparisonType) != -1)
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string starts with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must start with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref="StringComparison.CurrentCulture"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> does not start with <paramref name="value" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
    public static string MustStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType = StringComparison.CurrentCulture,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).StartsWith(value, comparisonType))
        {
            Throw.StringDoesNotStartWith(parameter, value, comparisonType, parameterName, message);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string starts with the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must start with.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not start with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || !parameter.StartsWith(value))
        {
            Throw.CustomException(exceptionFactory, parameter, value!);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string starts with the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must start with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not start with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType,
        [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || !comparisonType.IsValidEnumValue() || !parameter.StartsWith(value, comparisonType))
        {
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        }
        return parameter;
    }

    /// <summary>
    /// Ensures that the string does not start with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that <paramref name="parameter" /> must not start with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref="StringComparison.CurrentCulture" />.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> starts with <paramref name="value" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    public static string MustNotStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType = StringComparison.CurrentCulture,
        [CallerArgumentExpression("parameter")]
        string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).StartsWith(value, comparisonType))
        {
            Throw.StringStartsWith(parameter, value, comparisonType, parameterName, message);
        }

        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string does not start with the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that <paramref name="parameter" /> must not start with.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not start with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustNotStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || parameter.StartsWith(value))
        {
            Throw.CustomException(exceptionFactory, parameter, value!);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string does not start with the specified value, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string that <paramref name="parameter" /> must not start with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not start with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustNotStartWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType,
        [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || parameter.StartsWith(value, comparisonType))
        {
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string ends with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must end with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref="StringComparison.CurrentCulture"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> does not end with <paramref name="value" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    public static string MustEndWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType = StringComparison.CurrentCulture,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).EndsWith(value, comparisonType))
        {
            Throw.StringDoesNotEndWith(parameter, value, comparisonType, parameterName, message);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string ends with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must end with.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not end with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustEndWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || !parameter.EndsWith(value))
        {
            Throw.CustomException(exceptionFactory, parameter, value!);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string ends with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must end with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> does not end with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustEndWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType,
        [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || !parameter.EndsWith(value, comparisonType))
        {
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string does not end with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must not end with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref="StringComparison.CurrentCulture"/>.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> ends with <paramref name="value" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    public static string MustNotEndWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType = StringComparison.CurrentCulture,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.MustNotBeNull(parameterName, message).EndsWith(value, comparisonType))
        {
            Throw.StringEndsWith(parameter, value, comparisonType, parameterName, message);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string does not end with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must not end with.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> ends with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustNotEndWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || parameter.EndsWith(value))
        {
            Throw.CustomException(exceptionFactory, parameter, value!);
        }
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string does not end with the specified value, or otherwise throws a <see cref="SubstringException" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="value">The other string <paramref name="parameter"/> must not end with.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType"/> are passed to this delegate.</param>
    /// <exception cref="Exception">
    /// Your custom exception thrown when <paramref name="parameter" /> ends with <paramref name="value" />,
    /// or when <paramref name="parameter" /> is null,
    /// or when <paramref name="value" /> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
    public static string MustNotEndWith(
        [NotNull, ValidatedNotNull] this string? parameter,
        [NotNull, ValidatedNotNull] string value,
        StringComparison comparisonType,
        [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory
    )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
        if (parameter is null || value is null || parameter.EndsWith(value, comparisonType))
        {
            Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
        }
        return parameter;
    }

    /// <summary>
    /// Checks if the specified string is an email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex"/>.
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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailAddressPattern"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("emailAddress:null => false; emailAddressPattern:null => halt")]
    public static bool IsEmailAddress([NotNullWhen(true)] this string? emailAddress, Regex emailAddressPattern) =>
        emailAddress != null && emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern)).IsMatch(emailAddress);

    /// <summary>
    /// Ensures that the string is a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex"/>, or otherwise throws an <see cref="InvalidEmailAddressException"/>.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeEmailAddress([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsEmailAddress())
            Throw.InvalidEmailAddress(parameter, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is a valid email address using the default email regular expression
    /// defined in <see cref="RegularExpressions.EmailRegex"/>, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeEmailAddress([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (!parameter.IsEmailAddress())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is a valid email address using the provided regular expression,
    /// or otherwise throws an <see cref="InvalidEmailAddressException"/>.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the input string is a valid email.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidEmailAddressException">Thrown when <paramref name="parameter" /> is no valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; emailAddressPattern:null => halt")]
    public static string MustBeEmailAddress([NotNull, ValidatedNotNull] this string? parameter, Regex emailAddressPattern, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsEmailAddress(emailAddressPattern))
            Throw.InvalidEmailAddress(parameter, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is a valid email address using the provided regular expression,
    /// or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The email address that will be validated.</param>
    /// <param name="emailAddressPattern">The regular expression that determines if the input string is a valid email.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="emailAddressPattern"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or no valid email address.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; emailAddressPattern:null => halt")]
    public static string MustBeEmailAddress([NotNull, ValidatedNotNull] this string? parameter, Regex emailAddressPattern, Func<string?, Regex, Exception> exceptionFactory)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
        if (emailAddressPattern is null || !parameter.IsEmailAddress(emailAddressPattern))
            Throw.CustomException(exceptionFactory, parameter, emailAddressPattern!);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is shorter than the specified length, or otherwise throws a <see cref="StringLengthException"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be shorter than.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when <paramref name="parameter"/> has a length greater than or equal to <paramref name="length"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeShorterThan([NotNull, ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.MustNotBeNull(parameterName, message).Length >= length)
            Throw.StringNotShorterThan(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is shorter than the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be shorter than.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or when it has a length greater than or equal to <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeShorterThan([NotNull, ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
    {
        if (parameter is null || parameter.Length >= length)
            Throw.CustomException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is shorter than or equal to the specified length, or otherwise throws a <see cref="StringLengthException"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be shorter than or equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when <paramref name="parameter"/> has a length greater than <paramref name="length"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeShorterThanOrEqualTo([NotNull, ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.MustNotBeNull(parameterName, message).Length > length)
            Throw.StringNotShorterThanOrEqualTo(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is shorter than or equal to the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be shorter than or equal to.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or when it has a length greater than <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeShorterThanOrEqualTo([NotNull, ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
    {
        if (parameter is null || parameter.Length > length)
            Throw.CustomException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string has the specified length, or otherwise throws a <see cref="StringLengthException"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The asserted length of the string.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when <paramref name="parameter"/> has a length other than <paramref name="length"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLength([NotNull, ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.MustNotBeNull(parameterName, message).Length != length)
            Throw.StringLengthNotEqualTo(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string has the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The asserted length of the string.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or when it has a length other than <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLength([NotNull, ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
    {
        if (parameter is null || parameter.Length != length)
            Throw.CustomException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is longer than the specified length, or otherwise throws a <see cref="StringLengthException"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be longer than.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when <paramref name="parameter"/> has a length shorter than or equal to <paramref name="length"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeLongerThan([NotNull, ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.MustNotBeNull(parameterName, message).Length <= length)
            Throw.StringNotLongerThan(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is longer than the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be longer than.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or when it has a length shorter than or equal to <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeLongerThan([NotNull, ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
    {
        if (parameter is null || parameter.Length <= length)
            Throw.CustomException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is longer than or equal to the specified length, or otherwise throws a <see cref="StringLengthException"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be longer than or equal to.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when <paramref name="parameter"/> has a length shorter than <paramref name="length"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeLongerThanOrEqualTo([NotNull, ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (parameter.MustNotBeNull(parameterName, message).Length < length)
            Throw.StringNotLongerThanOrEqualTo(parameter, length, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is longer than or equal to the specified length, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="length">The length that the string must be longer than or equal to.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or when it has a length shorter than <paramref name="length"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeLongerThanOrEqualTo([NotNull, ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
    {
        if (parameter is null || parameter.Length < length)
            Throw.CustomException(exceptionFactory, parameter, length);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string's length is within the specified range, or otherwise throws a <see cref="StringLengthException"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="range">The range where the string's length must be in-between.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringLengthException">Thrown when the length of <paramref name="parameter"/> is not with the specified <paramref name="range"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLengthIn([NotNull, ValidatedNotNull] this string? parameter, Range<int> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!range.IsValueWithinRange(parameter.MustNotBeNull(parameterName, message).Length))
            Throw.StringLengthNotInRange(parameter, range, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string's length is within the specified range, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="range">The range where the string's length must be in-between.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="range"/> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or its length is not within the specified range.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustHaveLengthIn([NotNull, ValidatedNotNull] this string? parameter, Range<int> range, Func<string?, Range<int>, Exception> exceptionFactory)
    {
        if (parameter is null || !range.IsValueWithinRange(parameter.Length))
            Throw.CustomException(exceptionFactory, parameter, range);
        return parameter;
    }

    /// <summary>
    /// Checks if the string is either "\n" or "\r\n". This is done independently of the current value of <see cref="Environment.NewLine"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("=> false, parameter:canbenull; => true, parameter:notnull")]
    public static bool IsNewLine([NotNullWhen(true)] this string? parameter) => parameter == "\n" || parameter == "\r\n";

    /// <summary>
    /// Ensures that the string is either "\n" or "\r\n", or otherwise throws a <see cref="StringException"/>. This is done independently of the current value of <see cref="Environment.NewLine"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">Thrown when <paramref name="parameter"/> is not equal to "\n" or "\r\n".</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeNewLine([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsNewLine())
            Throw.NotNewLine(parameter, parameterName, message);
        return parameter;
    }

    /// <summary>
    /// Ensures that the string is either "\n" or "\r\n", or otherwise throws your custom exception. This is done independently of the current value of <see cref="Environment.NewLine"/>.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is not equal to "\n" or "\r\n".</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeNewLine([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (!parameter.IsNewLine())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }

    /// <summary>
    /// Checks if the specified string is trimmed, i.e. it does not start or end with
    /// white space characters. Inputting an empty string will return true. When null is passed,
    /// you can control the return value with <paramref name="regardNullAsTrimmed" /> which will
    /// return true by default.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regardNullAsTrimmed">
    /// The value indicating whether true or false should be returned from this method when the
    /// <paramref name="parameter" /> is null. The default value is true.
    /// </param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed, else false. An empty string will result in true.
    /// You can control the return value with <paramref name="regardNullAsTrimmed" /> when the
    /// <paramref name="parameter" /> is null.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmed(this string? parameter, bool regardNullAsTrimmed = true) =>
        parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmed();

    /// <summary>
    /// Checks if the specified character span is trimmed, i.e. it does not start or end with
    /// white space characters. Inputting an empty span will return true.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>True if the <paramref name="parameter"/> is trimmed, else false. An empty span will result in true.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmed(this ReadOnlySpan<char> parameter) =>
        parameter.Length == 0 ||
        !parameter[0].IsWhiteSpace() &&
        !parameter[parameter.Length - 1].IsWhiteSpace();


    /// <summary>
    /// Ensures that the string is not null and trimmed, or otherwise throws a <see cref="StringException"/>.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">
    /// Thrown when <paramref name="parameter"/> is not trimmed, i.e. they start or end with white space characters.
    /// Empty strings are regarded as trimmed.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmed([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsTrimmed())
            Throw.NotTrimmed(parameter, parameterName, message);
        return parameter;

    }

    /// <summary>
    /// Ensures that the string is not null and trimmed, or otherwise throws your custom exception.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or not trimmed. Empty strings are regarded as trimmed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmed([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (parameter is null || !parameter.AsSpan().IsTrimmed())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }

    /// <summary>
    /// Checks if the specified string is trimmed at the start, i.e. it does not start with
    /// white space characters. Inputting an empty string will return true.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regardNullAsTrimmed">
    /// The value indicating whether true or false should be returned from this method when the
    /// <paramref name="parameter" /> is null. The default value is true.
    /// </param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the start, else false.
    /// An empty string will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtStart(this string? parameter, bool regardNullAsTrimmed = true) =>
        parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmedAtStart();

    /// <summary>
    /// Checks if the specified character span is trimmed at the start, i.e. it does not start with
    /// white space characters. Inputting an empty span will return true.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the start, else false.
    /// An empty span will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtStart(this ReadOnlySpan<char> parameter) =>
        parameter.Length == 0 || !parameter[0].IsWhiteSpace();
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the start, or otherwise throws a <see cref="StringException"/>.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">
    /// Thrown when <paramref name="parameter"/> is not trimmed at the start, i.e. they start with white space characters.
    /// Empty strings are regarded as trimmed.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtStart([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsTrimmedAtStart())
            Throw.NotTrimmedAtStart(parameter, parameterName, message);
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the start, or otherwise throws your custom exception.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or not trimmed at the start. Empty strings are regarded as trimmed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtStart([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (parameter is null || !parameter.AsSpan().IsTrimmedAtStart())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }
    
    /// <summary>
    /// Checks if the specified string is trimmed at the end, i.e. it does not end with
    /// white space characters. Inputting an empty string will return true.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regardNullAsTrimmed">
    /// The value indicating whether true or false should be returned from this method when the
    /// <paramref name="parameter" /> is null. The default value is true.
    /// </param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the start, else false.
    /// An empty string will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtEnd(this string? parameter, bool regardNullAsTrimmed = true) =>
        parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmedAtEnd();

    /// <summary>
    /// Checks if the specified character span is trimmed at the end, i.e. it does not end with
    /// white space characters. Inputting an empty span will return true.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>
    /// True if the <paramref name="parameter"/> is trimmed at the end, else false.
    /// An empty span will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtEnd(this ReadOnlySpan<char> parameter) =>
        parameter.Length == 0 || !parameter[parameter.Length - 1].IsWhiteSpace();
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the end, or otherwise throws a <see cref="StringException"/>.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="StringException">
    /// Thrown when <paramref name="parameter"/> is not trimmed at the end, i.e. they end with white space characters.
    /// Empty strings are regarded as trimmed.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtEnd([NotNull, ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
    {
        if (!parameter.MustNotBeNull(parameterName, message).IsTrimmedAtEnd())
            Throw.NotTrimmedAtEnd(parameter, parameterName, message);
        return parameter;
    }
    
    /// <summary>
    /// Ensures that the string is not null and trimmed at the end, or otherwise throws your custom exception.
    /// Empty strings are regarded as trimmed.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is null or not trimmed at the end. Empty strings are regarded as trimmed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static string MustBeTrimmedAtEnd([NotNull, ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
    {
        if (parameter is null || !parameter.AsSpan().IsTrimmedAtEnd())
            Throw.CustomException(exceptionFactory, parameter);
        return parameter;
    }
}