using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EqualityAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBe must throw an exception when the specified values are different.")]
        [InlineData(42, 0)]
        [InlineData(true, false)]
        [InlineData("Hello", "World!")]
        public void ValuesNotEqual<T>(T value, T other)
        {
            Action act = () => value.MustBe(other, parameterName: nameof(value));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(value)} must be {other}, but you specified {value}");
        }

        [Theory(DisplayName = "MustBe must not throw an exception when the specified values are equal.")]
        [InlineData(42)]
        [InlineData(55.89)]
        [InlineData("Hey")]
        public void ValuesEqual<T>(T value)
        {
            Action act = () => value.MustBe(value, parameterName: nameof(value));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustBe must throw an exception when the specified values are different, using an equality comparer for comparison.")]
        public void ValuesNotEqualWithEqualityComparer()
        {
            Action act = () => 55.0.MustBe(55.1, EqualityComparer<double>.Default);

            act.ShouldThrow<ArgumentException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Hello".MustBe("World", exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentException>(message => 42.MustBe(48, message: message)));
        }
    }
}