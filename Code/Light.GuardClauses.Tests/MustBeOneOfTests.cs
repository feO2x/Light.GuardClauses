using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeOneOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeOneOf must throw an exception when the specified value is not within the given items.")]
        [MemberData(nameof(NotInItemsTestData))]
        public void NotInItems<T>(T value, T[] items)
        {
            Action act = () => value.MustBeOneOf(items, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must be one of the items{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(items)}{Environment.NewLine}but you specified {value}.");
        }

        public static readonly TestData NotInItemsTestData =
            new[]
            {
                new object[] { 'a', new[] { 'b', 'c', 'd' } },
                new object[] { 5, new[] { 42, 87, 35 } },
                new object[] { "f", new[] { "ak", "k" } }
            };

        [Theory(DisplayName = "MustBeOneOf must not throw an exception when the specified value is one of the given items.")]
        [MemberData(nameof(InItemsTestData))]
        public void InItems<T>(T value, T[] items)
        {
            Action act = () => value.MustBeOneOf(items, nameof(value));

            act.ShouldNotThrow();
        }

        public static readonly TestData InItemsTestData =
            new[]
            {
                new object[] { 'U', new[] { 'S', 'T', 'U', 'F', 'F' } },
                new object[] { 42, new[] { 41, 42, 43 } }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 'a'.MustBeOneOf(new[] { 'x', 'y' }, exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 'x'.MustBeOneOf(new[] { 'a', 'b', 'c' }, message: message)));
        }
    }
}