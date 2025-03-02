using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public sealed class IsEmailAddressTests
{
    public static readonly TheoryData<string> InvalidEmailAddresses =
    [
        null,
        "plainaddress",
        "#@%^%#$@#$@#.com",
        "@domain.com",
        "Joe Smith <email@domain.com>",
        "email.domain.com",
        "email@domain@domain.com",
        ".email@domain.com",
        "email.@domain.com",
        "email..email@domain.com",
        "email@domain.com (Joe Smith)",
        "email@domain",
        "email@-domain.com",
        "email@111.222.333.44444",
        "email@domain..com",
        "email@256.256.256.256",
    ];

    [Theory]
    [MemberData(nameof(InvalidEmailAddresses))]
    public void IsNotValidEmailAddress(string email)
    {
        var isValid = email.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    public static readonly TheoryData<string> ValidEmailAddresses =
    [
        "email@domain.com",
        "firstname.lastname@domain.com",
        "email@subdomain.domain.com",
        "firstname+lastname@domain.com",
        "email@123.123.123.123",
        "1234567890@domain.com",
        "email@domain-one.com",
        "_______@domain.com",
        "email@domain.name",
        "email@domain.co.jp",
        "firstname-lastname@domain.com",
        "email@domain.museum", // Long TLD (>4 chars)
        "email@domain.travel", // Another long TLD
        "email@domain.photography", // Even longer TLD
        "email@[IPv6:2001:db8::1]", // IPv6 format
        "\"quoted\"@domain.com", // Quoted local part
        "user.name+tag+sorting@example.com", // Gmail-style + addressing
        "あいうえお@domain.com", // Unicode character test
    ];

    [Theory]
    [MemberData(nameof(ValidEmailAddresses))]
    public void IsValidEmailAddress(string email)
    {
        var isValid = email.IsEmailAddress();

        isValid.Should().BeTrue();
    }

#if NET8_0
    [Theory]
    [MemberData(nameof(InvalidEmailAddresses))]
    public void IsNotValidEmailAddress_ReadOnlySpan(string email)
    {
        var span = new ReadOnlySpan<char>(email?.ToCharArray() ?? []);
        var isValid = span.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(ValidEmailAddresses))]
    public void IsValidEmailAddress_ReadOnlySpan(string email)
    {
        var span = email.AsSpan();
        var isValid = span.IsEmailAddress();

        isValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidEmailAddresses))]
    public void IsNotValidEmailAddress_Span(string email)
    {
        var span = new Span<char>(email?.ToCharArray() ?? []);
        var isValid = span.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(ValidEmailAddresses))]
    public void IsValidEmailAddress_Span(string email)
    {
        var span = new Span<char>(email.ToCharArray());
        var isValid = span.IsEmailAddress();

        isValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidEmailAddresses))]
    public void IsNotValidEmailAddress_Memory(string email)
    {
        var memory = email?.ToCharArray().AsMemory() ?? Memory<char>.Empty;
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(ValidEmailAddresses))]
    public void IsValidEmailAddress_Memory(string email)
    {
        var memory = email.ToCharArray().AsMemory();
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidEmailAddresses))]
    public void IsNotValidEmailAddress_ReadOnlyMemory(string email)
    {
        var memory = new ReadOnlyMemory<char>(email?.ToCharArray() ?? []);
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(ValidEmailAddresses))]
    public void IsValidEmailAddress_ReadOnlyMemory(string email)
    {
        var memory = new ReadOnlyMemory<char>(email.ToCharArray());
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeTrue();
    }
#endif
}
