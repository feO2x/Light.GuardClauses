using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EqualityAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBe must not throw an exception when the specified values are different.")]
        [InlineData(42, 0)]
        [InlineData(true, false)]
        [InlineData("Hello", "World!")]
        public void ValuesNotEqual<T>(T value, T other)
        {
            var result = value.MustNotBe(other);

            result.Should().Be(value);
        }

        [Theory(DisplayName = "MustNotBe must throw an ArgumentException when the specified values are equal.")]
        [InlineData(42)]
        [InlineData(55.89)]
        [InlineData("Hey")]
        public void ValuesEqual<T>(T value)
        {
            Action act = () => value.MustNotBe(value, parameterName: nameof(value));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be {value}, but you specified this very value.");
        }

        [Fact(DisplayName = "MustBe must throw an exception when the specified value is not the expected one, using an equality comparer for comparison.")]
        public void ValuesNotEqualWithEqualityComparer()
        {
            Action act = () => 55.MustNotBe(55, EqualityComparer<int>.Default);

            act.ShouldThrow<ArgumentException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Hello".MustNotBe("Hello", exception: exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => 42.MustNotBe(42, message: message)));
        }
    }
}