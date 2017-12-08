using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeNullOrWhiteSpaceTests : ICustomMessageAndExceptionTestDataProvider
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
            var result = value.MustNotBeNullOrWhiteSpace(nameof(value));

            result.Should().BeSameAs(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => string.Empty.MustNotBeNullOrWhiteSpace(exception: exception))
                    .AddExceptionTest(exception => "    ".MustNotBeNullOrWhiteSpace(exception: exception))
                    .AddExceptionTest(exception => "\t\r\n".MustNotBeNullOrWhiteSpace(exception: exception));

            testData.AddMessageTest<EmptyStringException>(message => string.Empty.MustNotBeNullOrWhiteSpace(message: message))
                    .AddMessageTest<StringIsOnlyWhiteSpaceException>(message => "    ".MustNotBeNullOrWhiteSpace(message: message))
                    .AddMessageTest<StringIsOnlyWhiteSpaceException>(message => "\t\r\n".MustNotBeNullOrWhiteSpace(message: message));
        }
    }
}