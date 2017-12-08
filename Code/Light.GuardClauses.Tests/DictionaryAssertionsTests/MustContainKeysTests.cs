using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.DictionaryAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustContainKeysTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainKeys must throw a KeyNotFoundException when at least one of the specified keys is not present in the dictionary.")]
        [MemberData(nameof(DoesNotHaveKeysData))]
        public void DoesNotHaveKeys(IDictionary<string, object> dictionary, IEnumerable<string> keys)
        {
            Action act = () => dictionary.MustContainKeys(keys, nameof(dictionary));

            act.ShouldThrow<KeyNotFoundException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must contain all of the following keys:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(keys)}{Environment.NewLine}but it does not.");
        }

        public static readonly TestData DoesNotHaveKeysData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["a"] = new object(), ["b"] = ConsoleColor.Black }, new[] { "a", "c" } },
                new object[] { new Dictionary<string, object> { ["First Key"] = "Value 1", ["SecondKey"] = "Value 2", ["Third Key"] = null }, new[] { "ThirdKey", "Another Key" } }
            };

        [Theory(DisplayName = "MustContainKeys must not throw an exception when all the keys are present in the dictionary.")]
        [MemberData(nameof(HasKeysData))]
        public void HasKeys(IDictionary<string, object> dictionary, string[] keys)
        {
            Action act = () => dictionary.MustContainKeys(keys);

            act.ShouldNotThrow();
        }

        public static readonly TestData HasKeysData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["1"] = "a", ["2"] = "b", ["3"] = "c" }, new[] { "1", "3" } },
                new object[] { new Dictionary<string, object> { ["Id"] = "BudgetInputForm", ["Layout"] = "Horizontal", ["NumberOfColumns"] = 3 }, new[] { "Id", "Layout", "NumberOfColumns" } }
            };

        [Theory(DisplayName = "MustContainKeys must throw an ArgumentNullException when parameter or keys is null.")]
        [MemberData(nameof(ArgumentNullData))]
        public void ArgumentNull(IDictionary<char, object> dictionary, char[] keys)
        {
            Action act = () => dictionary.MustContainKeys(keys);

            act.ShouldThrow<ArgumentNullException>();
        }

        public static readonly TestData ArgumentNullData =
            new[]
            {
                new object[] { null, new char[2] },
                new object[] { new Dictionary<char, object>(), null }
            };

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<char, object>().MustContainKeys(new[] { 'a', 'b' }, exception: exception)));

            testData.Add(new CustomMessageTest<KeyNotFoundException>(message => new Dictionary<char, object>().MustContainKeys(new[] { 'a', 'b' }, message: message)));
        }
    }
}