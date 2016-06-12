using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeLessThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeLessThan must throw an ArgumentOutOfRangeException when the specified parameter is not less than the boundary value.")]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(short.MaxValue, short.MinValue)]
        [InlineData('Z', 'Z')]
        [InlineData("I'm", "greater than you")]
        public void ParameterNotLess<T>(T first, T second) where T : IComparable<T>
        {
            Action act = () => first.MustBeLessThan(second, nameof(first));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be less than {second}, but you specified {first}.");
        }

        [Theory(DisplayName = "MustBeLessThan must not throw an ArgumentOutOfRangeException when the specified paramter is less than the boundary value.")]
        [InlineData(1, 2)]
        [InlineData(-444, -410)]
        [InlineData(long.MinValue, long.MinValue + 14)]
        [InlineData('A', 'a')]
        [InlineData("Hello", "There")]
        public void ParameterLess<T>(T first, T second) where T : IComparable<T>
        {
            Action act = () => first.MustBeLessThan(second);

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 14.MustBeLessThan(2, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 2.MustBeLessThan(2, message: message)));
        }
    }
}