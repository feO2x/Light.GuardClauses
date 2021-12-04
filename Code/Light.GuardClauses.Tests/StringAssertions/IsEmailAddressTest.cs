using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public sealed class IsEmailAddressTest
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
    [InlineData("あいうえお@domain.com")]
    [InlineData("email@domain.com (Joe Smith)")]
    [InlineData("email@domain")]
    [InlineData("email@-domain.com")]
    [InlineData("email@111.222.333.44444")]
    [InlineData("email@domain..com")]
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
    public void IsValidEmailAddress(string email)
    {
        var isValid = email.IsEmailAddress();

        isValid.Should().BeTrue();
    }
}