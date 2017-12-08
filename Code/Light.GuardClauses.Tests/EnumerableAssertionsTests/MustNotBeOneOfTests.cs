using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class MustNotBeOneOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeOneOf must throw an exception when the specified value is within the given items.")]
        public void ParameterWithinItems()
        {
            const char value = 'a';
            var items = new[] { 'a', 'b', 'c' };

            Action act = () => value.MustNotBeOneOf(items, parameterName: nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be one of the items{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(items)}{Environment.NewLine}but you specified {value}.");
        }

        [Fact(DisplayName = "MustNotBeOneOf must not throw an exception when the specified value is not one of the given items.")]
        public void ParameterOutOfItems()
        {
            const char value = 'X';
            var items = new[] { 'S', 'T', 'U', 'F', 'F' };

            var result = value.MustNotBeOneOf(items, parameterName: nameof(value));

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "a".MustNotBeOneOf(new[] { "a", "b", "c" }, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => "a".MustNotBeOneOf(new[] { "a", "b", "c" }, message: message)));
        }
    }
}