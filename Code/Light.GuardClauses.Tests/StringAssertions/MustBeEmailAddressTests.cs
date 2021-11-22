using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeEmailAddressTests
    {
        [Theory]
        [InlineData("email@domain.com")]
        [InlineData("email@123.123.123.123")]
        public static void ValidEmailAddress(string emailAddress) => emailAddress.MustBeEmailAddress().Should().BeSameAs(emailAddress);

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("Joe Smith <email@domain.com>")]
        public static void InvalidEmailAddress(string emailAddress)
        {
            Action act = () => emailAddress.MustBeEmailAddress();

            act.Should().Throw<InvalidEmailAddressException>()
               .And.Message.Should().Contain($"emailAddress must be a valid email address, but it actually is \"{emailAddress}\".");
        }

        [Theory]
        [InlineData(".email@domain.com")]
        [InlineData("email.@domain.com")]
        public static void InvalidEmailAddressArgumentName(string emailAddress)
        {
            Action act = () => emailAddress.MustBeEmailAddress(nameof(emailAddress));

            act.Should().Throw<InvalidEmailAddressException>()
               .And.Message.Should().Contain($"emailAddress must be a valid email address, but it actually is \"{emailAddress}\".");
        }

        [Theory]
        [InlineData("email@domain")]
        [InlineData("email@-domain.com")]
        public static void InvalidEmailAddressCustomMessage(string emailAddress)
        {
            const string customMessage = "This email address is not valid";

            Action act = () => emailAddress.MustBeEmailAddress(nameof(emailAddress), customMessage);

            act.Should().Throw<InvalidEmailAddressException>()
               .And.Message.Should().Contain(customMessage);
        }

        private static readonly Regex CustomRegex = new (@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        [Fact]
        public static void InvalidEmailCustomRegex()
        {
            const string email = "email@domain@domain.com";

            Action act = () => email.MustBeEmailAddress(CustomRegex, "email");

            act.Should().Throw<InvalidEmailAddressException>()
               .And.Message.Should().Contain($"email must be a valid email address, but it actually is \"{email}\".");
        }

        [Fact]
        public static void EmailNull()
        {
            Action act = () => default(string).MustBeEmailAddress();

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(NullData))]
        public static void NullCustomRegex(string email, Regex regex)
        {
            Action act = () => email.MustBeEmailAddress(regex);

            act.Should().Throw<ArgumentNullException>();
        }

        public static readonly TheoryData<string, Regex> NullData =
            new()
            {
                { null, CustomRegex },
                { "invalidEmailAddress", null }
            };

        [Fact]
        public static void CustomException() =>
            Test.CustomException("email.domain.com",
                                 (input, exceptionFactory) => input.MustBeEmailAddress(exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidEmailAddressException>(message => "#@%^%#$@#$@#.com".MustBeEmailAddress(message: message));

        [Fact]
        public static void CustomExceptionCustomRegex() =>
            Test.CustomException("invalidEmailAddress",
                                 CustomRegex,
                                 (i, r, exceptionFactory) => i.MustBeEmailAddress(r, exceptionFactory));

        [Fact]
        public static void CustomMessageCustomRegex() =>
            Test.CustomMessage<InvalidEmailAddressException>(message => "invalidEmailAddress".MustBeEmailAddress(CustomRegex, message: message));
    }
}