using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustHaveLengthTests
    {
        [Theory]
        [InlineData("Foo", 3)]
        [InlineData("Gorge", 5)]
        [InlineData("", 0)]
        [InlineData("I will do what queens do. I will rule.", 38)]
        public static void LengthEqual(string @string, int length) =>
            @string.MustHaveLength(length).Should().BeSameAs(@string);

        [Theory]
        [InlineData("Bar", 2)]
        [InlineData("Baz", 4)]
        [InlineData("", -1)]
        [InlineData("We have 200,000 reasons to take the city.", 931)]
        public static void LengthNotEqual(string @string, int length)
        {
            var act = () => @string.MustHaveLength(length, nameof(@string));

            act.Should().Throw<StringLengthException>()
               .And.Message.Should().Contain($"string must have length {length}, but it actually has length {@string.Length}.");
        }

        [Fact]
        public static void StringNull()
        {
            var act = () => ((string) null).MustHaveLength(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException("Foo",
                                 5,
                                 (s, l, exceptionFactory) => s.MustHaveLength(l, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringLengthException>(message => "Foo".MustHaveLength(42, message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            const string foo = "Foo";

            var act = () => foo.MustHaveLength(4);

            act.Should().Throw<StringLengthException>()
               .And.ParamName.Should().Be(nameof(foo));
        }
    }
}