using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustNotBeNullOrWhiteSpaceTests
    {
        [Fact]
        public static void StringIsNull()
        {
            Action act = () => ((string) null).MustNotBeNullOrWhiteSpace("Foo");

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("Foo");
        }

        [Fact]
        public static void StringIsEmpty()
        {
            var value = string.Empty;

            Action act = () => value.MustNotBeNullOrWhiteSpace(nameof(value));

            act.Should().Throw<EmptyStringException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be an empty string, but it actually is.");
        }

        [Theory]
        [MemberData(nameof(WhiteSpaceData))]
        public static void StringIsWhiteSpace(string value)
        {
            Action act = () => value.MustNotBeNullOrWhiteSpace(nameof(value));

            act.Should().Throw<WhiteSpaceStringException>()
               .And.Message.Should().Contain($"{nameof(value)} must not contain only white space, but it actually is \"{value}\".");
        }

        public static readonly TheoryData<string> WhiteSpaceData =
            new()
            {
                Environment.NewLine,
                " ",
                "\t",
                "\t\t  ",
                "\r"
            };

        [Theory]
        [InlineData("a")]
        [InlineData("a ")]
        [InlineData("  1")]
        [InlineData("  \t{id:1}\t")]
        [InlineData("{\r\n\tid: 1\r\n}")]
        public static void NonWhiteSpace(string value) => value.MustNotBeNullOrWhiteSpace(nameof(value)).Should().BeSameAs(value);

        [Fact]
        public static void CustomExceptionStringNull() =>
            Test.CustomException((string) null,
                                 (@null, exceptionFactory) => @null.MustNotBeNullOrWhiteSpace(exceptionFactory));

        [Fact]
        public static void CustomExceptionEmptyString() =>
            Test.CustomException(string.Empty,
                                 (emptyString, exceptionFactory) => emptyString.MustNotBeNullOrWhiteSpace(exceptionFactory));

        [Theory]
        [MemberData(nameof(WhiteSpaceData))]
        public static void CustomExceptionWhiteSpace(string whiteSpaceString) =>
            Test.CustomException(whiteSpaceString,
                                 (@string, exceptionFactory) => @string.MustNotBeNullOrWhiteSpace(exceptionFactory));

        [Fact]
        public static void CallerArgumentExpression()
        {
            const string whiteSpaceString = "\t";

            var act = () => whiteSpaceString.MustNotBeNullOrWhiteSpace();

            act.Should().Throw<WhiteSpaceStringException>()
               .And.ParamName.Should().Be(nameof(whiteSpaceString));
        }
    }
}