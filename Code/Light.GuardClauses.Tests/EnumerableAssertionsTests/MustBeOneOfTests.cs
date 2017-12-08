using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class MustBeOneOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeOneOf must throw an exception when the specified value is not within the given items.")]
        public void NotInItems()
        {
            const char value = 'a';
            var items = new[] { 'b', 'c', 'd' };

            Action act = () => value.MustBeOneOf(items, parameterName: nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must be one of the items{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(items)}{Environment.NewLine}but you specified {value}.");
        }

        [Fact(DisplayName = "MustBeOneOf must not throw an exception when the specified value is one of the given items.")]
        public void InItems()
        {
            const int value = 42;
            var items = new[] { 41, 42, 43 };

            var result = value.MustBeOneOf(items, parameterName: nameof(value));

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 'a'.MustBeOneOf(new[] { 'x', 'y' }, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 'x'.MustBeOneOf(new[] { 'a', 'b', 'c' }, message: message)));
        }
    }
}