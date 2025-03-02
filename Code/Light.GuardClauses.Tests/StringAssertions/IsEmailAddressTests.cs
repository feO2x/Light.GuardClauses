using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public sealed class IsEmailAddressTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("plainaddress")]
    [InlineData("#@%^%#$@#$@#.com")]
    [InlineData("@domain.com")]
    [InlineData("Joe Smith <email@domain.com>")]
    [InlineData("email.domain.com")]
    [InlineData("email@domain@domain.com")]
    [InlineData(".email@domain.com")]
    [InlineData("email.@domain.com")]
    [InlineData("email..email@domain.com")]
    [InlineData("email@domain.com (Joe Smith)")]
    [InlineData("email@domain")]
    [InlineData("email@-domain.com")]
    [InlineData("email@111.222.333.44444")]
    [InlineData("email@domain..com")]
    [InlineData("email@256.256.256.256")] // Invalid IP (values > 255)
    public void IsNotValidEmailAddress(string email)
    {
        var isValid = email.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("email@domain.com")]
    [InlineData("firstname.lastname@domain.com")]
    [InlineData("email@subdomain.domain.com")]
    [InlineData("firstname+lastname@domain.com")]
    [InlineData("email@123.123.123.123")]
    [InlineData("1234567890@domain.com")]
    [InlineData("email@domain-one.com")]
    [InlineData("_______@domain.com")]
    [InlineData("email@domain.name")]
    [InlineData("email@domain.co.jp")]
    [InlineData("firstname-lastname@domain.com")]
    [InlineData("email@domain.museum")] // Long TLD (>4 chars)
    [InlineData("email@domain.travel")] // Another long TLD
    [InlineData("email@domain.photography")] // Even longer TLD
    [InlineData("email@[IPv6:2001:db8::1]")] // IPv6 format
    [InlineData("\"quoted\"@domain.com")] // Quoted local part
    [InlineData("user.name+tag+sorting@example.com")] // Gmail-style + addressing
    [InlineData("あいうえお@domain.com")] // Unicode character test
    public void IsValidEmailAddress(string email)
    {
        var isValid = email.IsEmailAddress();

        isValid.Should().BeTrue();
    }
}