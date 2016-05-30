using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
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
            Action act = () => value.MustNotBeNullOrWhiteSpace(nameof(value));

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => string.Empty.MustNotBeNullOrWhiteSpace(exception: exception)));
            testData.Add(new CustomExceptionTest(exception => "    ".MustNotBeNullOrWhiteSpace(exception: exception)));
            testData.Add(new CustomExceptionTest(exception => "\t\r\n".MustNotBeNullOrWhiteSpace(exception: exception)));
            testData.Add(new CustomExceptionTest(exception => ((string) null).MustNotBeNullOrWhiteSpace(exception: exception)));

            testData.Add(new CustomMessageTest<EmptyStringException>(message => string.Empty.MustNotBeNullOrWhiteSpace(message: message)));
            testData.Add(new CustomMessageTest<StringIsOnlyWhiteSpaceException>(message => "    ".MustNotBeNullOrWhiteSpace(message: message)));
            testData.Add(new CustomMessageTest<StringIsOnlyWhiteSpaceException>(message => "\t\r\n".MustNotBeNullOrWhiteSpace(message: message)));
            testData.Add(new CustomMessageTest<ArgumentNullException>(message => ((string)null).MustNotBeNullOrWhiteSpace(message: message)));
        }
    }
}