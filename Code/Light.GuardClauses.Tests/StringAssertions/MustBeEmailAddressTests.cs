using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public class MustBeEmailAddress
    {
        [Theory]
        [InlineData("email@domain.com")]
        public void ValidEmailAddress(string emailAddress) => emailAddress.MustBeEmailAddress().Should().Match(emailAddress);

        [Theory]
        [InlineData("plainaddress")]
        public void InvalidEmailAddress(string emailAddress)
        {
            Action act = () => emailAddress.MustBeEmailAddress();

            act.Should().Throw<InvalidEmailAddressException>()
               .And.Message.Should().Contain("The email address is not a valid email address");
        }

        [Theory]
        [InlineData("plainaddress")]
        public void InvalidEmailAddressArgumentName(string emailAddress)
        {
            Action act = () => emailAddress.MustBeEmailAddress(nameof(emailAddress));

            act.Should().Throw<InvalidEmailAddressException>()
               .And.Message.Should().Contain("emailAddress is not a valid email address");
        }

        [Theory]
        [InlineData("plainaddress")]
        public void InvalidEmailAddressCustomMessage(string emailAddress)
        {
            const string customMessage = "This email address is not valid";

            Action act = () => emailAddress.MustBeEmailAddress(nameof(emailAddress), customMessage);

            act.Should().Throw<InvalidEmailAddressException>()
               .And.Message.Should().Contain(customMessage);
        }
    }
}