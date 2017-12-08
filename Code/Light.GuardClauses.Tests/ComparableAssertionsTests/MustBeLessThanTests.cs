using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustBeLessThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeLessThan must throw an ArgumentOutOfRangeException when the specified parameter is not less than the boundary value.")]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(short.MaxValue, short.MinValue)]
        [InlineData('Z', 'Z')]
        public void ParameterNotLess(int first, int second)
        {
            Action act = () => first.MustBeLessThan(second, nameof(first));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be less than {second}, but you specified {first}.");
        }

        [Theory(DisplayName = "MustBeLessThan must not throw an ArgumentOutOfRangeException when the specified paramter is less than the boundary value.")]
        [InlineData(1L, 2L)]
        [InlineData(-444L, -410L)]
        [InlineData(long.MinValue, long.MinValue + 14)]
        [InlineData('A', 'a')]
        public void ParameterLess(long first, long second)
        {
            var result = first.MustBeLessThan(second);

            result.Should().Be(first);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 14.MustBeLessThan(2, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 2.MustBeLessThan(2, message: message)));
        }
    }
}