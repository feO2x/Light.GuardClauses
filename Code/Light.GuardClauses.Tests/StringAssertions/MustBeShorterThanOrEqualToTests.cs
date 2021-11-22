using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeShorterThanOrEqualToTests
    {
        [Theory]
        [InlineData("Foo", 3)]
        [InlineData("Bar", 4)]
        [InlineData("We have 200,000 reasons to take the city.", 42)]
        public static void ShorterOrEqualTo(string @string, int length) =>
            @string.MustBeShorterThanOrEqualTo(length).Should().BeSameAs(@string);

        [Theory]
        [InlineData("Baz", 2)]
        [InlineData("Gorge", 3)]
        [InlineData("You told me to do nothing before and I listened to you. I’m not doing nothing again", 2)]
        public static void LongerThan(string @string, int length)
        {
            Action act = () => @string.MustBeShorterThanOrEqualTo(length, nameof(@string));

            act.Should().Throw<StringLengthException>()
               .And.Message.Should().Contain($"string must be shorter or equal to {length}, but it actually has length {@string.Length}.");
        }

        [Fact]
        public static void StringNull()
        {
            Action act = () => ((string) null).MustBeShorterThanOrEqualTo(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException("Meh",
                                 2,
                                 (s, _, exceptionFactory) => s.MustBeShorterThanOrEqualTo(2, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringLengthException>(message => "foo".MustBeShorterThanOrEqualTo(1, message: message));
    }
}
