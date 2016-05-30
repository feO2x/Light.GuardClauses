using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeLessThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeLessThan must throw an exception when the specified value is below the given boundary.")]
        [InlineData(1, 2)]
        [InlineData(-87, 2)]
        [InlineData("a", "b")]
        [InlineData("X", "Y")]
        [InlineData(15U, 16U)]
        public void ParameterBelowBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeLessThan(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeLessThan must not throw an exception when the specified value is equal to or greater than the given boundary.")]
        [InlineData(1, 0)]
        [InlineData(42, 42)]
        [InlineData(-87, -88)]
        [InlineData("b", "a")]
        [InlineData("b", "b")]
        [InlineData("X", "f")]
        public void ParameterAtOrAboveBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeLessThan(boundary, nameof(value));

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeLessThan(43, exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeLessThan(43, message: message)));
        }
    }
}