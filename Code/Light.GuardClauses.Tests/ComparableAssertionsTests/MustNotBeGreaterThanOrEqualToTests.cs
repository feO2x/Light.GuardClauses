using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeGreaterThanOrEqualToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must throw an exception when the specified value is greater than or equal to the given boundary.")]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        [InlineData(-87, -88)]
        [InlineData("a", "a")]
        [InlineData("B", "A")]
        public void ParameterAtOrAboveBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeGreaterThanOrEqualTo(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than or equal to {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must not throw an exception when the specified value is less than the given boundary.")]
        [InlineData(0, 1)]
        [InlineData(-80, -70)]
        [InlineData("A", "B")]
        [InlineData("a", "A")]
        public void ParameterBelowBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeGreaterThanOrEqualTo(boundary);

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeGreaterThanOrEqualTo(41, exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeGreaterThanOrEqualTo(42, message: message)));
        }
    }
}