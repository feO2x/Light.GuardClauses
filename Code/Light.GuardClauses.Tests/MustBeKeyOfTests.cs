using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeKeyOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeKeyOf must throw an exception when the specified value is not within the keys of the given dictionary.")]
        [MemberData(nameof(NotInItemsTestData))]
        public void NotInItems(string value, Dictionary<string, string> dictionary)
        {
            Action act = () => value.MustBeKeyOf(dictionary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must be one of the dictionary keys{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(dictionary.Keys)}{Environment.NewLine}but you specified {value}.");
        }

        public static readonly TestData NotInItemsTestData =
            new[]
            {
                new object[] { "A", new Dictionary<string, string> { ["B"] = "Hello", ["C"] = "World" } },
                new object[] { "42", new Dictionary<string, string> { ["1"] = "a", ["2"] = "y" } }
            };

        [Theory(DisplayName = "MustBeKeyOf must not throw an exception when the specified value is one of the keys in the given dictionary.")]
        [MemberData(nameof(InItemsTestData))]
        public void InItems(int value, Dictionary<int, string> dictionary)
        {
            Action act = () => value.MustBeKeyOf(dictionary, nameof(value));

            act.ShouldNotThrow();
        }

        public static readonly TestData InItemsTestData =
            new[]
            {
                new object[] { 42, new Dictionary<int, string> { [42] = "Hey" } },
                new object[] { 2, new Dictionary<int, string> { [1] = "Hello", [2] = "World" } }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "a".MustBeKeyOf(new Dictionary<string, int> { ["b"] = 42 }, exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => "a".MustBeKeyOf(new Dictionary<string, int> { ["b"] = 42 }, message: message)));
        }
    }
}