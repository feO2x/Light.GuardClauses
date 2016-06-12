using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeGreaterThanOrEqualToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeGreaterThanOrEqualTo must throw an ArgumentOutOfRangeException when the specified parameter is less than the boundary value.")]
        [InlineData(1, 2)]
        [InlineData(-1, 1)]
        [InlineData(short.MinValue, short.MaxValue)]
        [InlineData('c', 'g')]
        [InlineData("abc", "def")]
        public void ParameterLess<T>(T first, T second) where T : IComparable<T>
        {
            Action act = () => first.MustBeGreaterThanOrEqualTo(second, nameof(first));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be greater than or equal to {second}, but you specified {first}.");
        }

        [Theory(DisplayName = "MustBeGreaterThanOrEqualTo must not throw an exception when the specified parameter is greater than or equal to the boundary value.")]
        [InlineData(14, 2)]
        [InlineData(15, 15)]
        [InlineData(long.MaxValue, long.MaxValue - 2)]
        [InlineData('G', 'G')]
        [InlineData("I'm", "greater")]
        public void ParameterGreaterOrEqual<T>(T first, T second) where T : IComparable<T>
        {
            Action act = () => first.MustBeGreaterThanOrEqualTo(second);

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 2.MustBeGreaterThanOrEqualTo(42, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 30.MustBeGreaterThanOrEqualTo(31, message: message)));
        }
    }
}