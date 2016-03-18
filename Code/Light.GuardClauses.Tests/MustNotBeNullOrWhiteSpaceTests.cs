using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeNullOrWhiteSpaceTests
    {
        [Fact(DisplayName = "MustNotBeNullOrWhiteSpace must throw an ArgumentNullException when the parameter is null.")]
        public void StringIsNull()
        {
            string value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustNotBeNullOrWhiteSpace(nameof(value));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Fact(DisplayName = "MustNotBeNullOrWhiteSpace must throw an EmptyStringException when the string is empty.")]
        public void StringIsEmpty()
        {
            var value = string.Empty;

            Action act = () => value.MustNotBeNullOrWhiteSpace(nameof(value));

            act.ShouldThrow<EmptyStringException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Theory(DisplayName = "MustBeNullOrWhiteSpace must throw an StringIsOnlyWhiteSpaceException when the string contains only whitespace.")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\t")]
        [InlineData("\t\t  ")]
        [InlineData("\r")]
        [MemberData(nameof(StringIsWhiteSpaceTestData))]
        public void StringIsWhiteSpace(string value)
        {
            Action act = () => value.MustNotBeNullOrWhiteSpace(nameof(value));

            act.ShouldThrow<StringIsOnlyWhiteSpaceException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        public static readonly TestData StringIsWhiteSpaceTestData =
            new[]
            {
                new object[] { Environment.NewLine }
            };

        [Theory(DisplayName = "MustBeNullOrWhiteSpace must not throw an exception when the string contains at least one non-whitespace character")]
        [InlineData("a")]
        [InlineData("a ")]
        [InlineData("  1")]
        [InlineData("  \t{id:1}\t")]
        [InlineData("{\r\n\tid: 1\r\n}")]
        public void NonWhiteSpace(string value)
        {
            Action act = () => value.MustNotBeNullOrWhiteSpace(nameof(value));

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "The caller can specify a custom message that MustNotBeNullOrWhiteSpace must inject instead of the default one.")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\r\n")]
        public void CustomMessage(string invalidString)
        {
            const string message = "Thou shall have human-readable information!";

            Action act = () => invalidString.MustNotBeNullOrWhiteSpace(message: message);

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain(message);
        }

        [Theory(DisplayName = "The caller can specify a custom exception that MustNotBeNullOrWhiteSpace must raise instead of the default one.")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\r\n")]
        public void CustomException(string invalidString)
        {
            var exception = new Exception();

            Action act = () => invalidString.MustNotBeNullOrWhiteSpace(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}