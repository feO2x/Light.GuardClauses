using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.DictionaryAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotContainKeysTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainKeys must throw a DictionaryException when the dictionary contains any of the specified keys.")]
        [MemberData(nameof(HasKeysData))]
        public void HasKeys(IDictionary<string, object> dictionary, IEnumerable<string> keys)
        {
            Action act = () => dictionary.MustNotContainKeys(keys, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must not contain any of the following keys:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(keys)}{Environment.NewLine}but it does.");
        }

        public static readonly TestData HasKeysData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["First Key"] = "Foo", ["Second Key"] = "Bar", ["Third Key"] = "Baz" }, new[] { "First Key" } },
                new object[] { new Dictionary<string, object> { ["First Key"] = "Foo", ["Second Key"] = "Bar", ["Third Key"] = "Baz" }, new[] { "Third Key", "Other Key" } },
                new object[] { new Dictionary<string, object> { ["First Key"] = "Foo", ["Second Key"] = "Bar", ["Third Key"] = "Baz" }, new[] { "Third Key", "Second Key", "First Key" } },
                new object[] { new Dictionary<string, object> { ["1"] = 1, ["2"] = 2 }, new[] { "1", "2", "1" } }
            };

        [Theory(DisplayName = "MustNotContainKeys must not throw an exception when the dictionary does not contain any of the specified keys.")]
        [MemberData(nameof(DoesNotHaveKeysData))]
        public void DoesNotHaveKeys(IDictionary<string, object> dictionary, string[] keys)
        {
            var result = dictionary.MustNotContainKeys(keys);

            result.Should().BeSameAs(dictionary);
        }

        public static readonly TestData DoesNotHaveKeysData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["First Key"] = "Foo", ["Second Key"] = "Bar", ["Third Key"] = "Baz" }, new[] { "Other Key" } },
                new object[] { new Dictionary<string, object> { ["First Key"] = "Foo", ["Second Key"] = "Bar", ["Third Key"] = "Baz" }, new[] { "Other Key", "And another Key" } },
                new object[] { new Dictionary<string, object> { ["1"] = 1, ["2"] = 2 }, new[] { "anything", "else" } }
            };

        [Theory(DisplayName = "MustNotContainKeys must throw an ArgumentNullException when dictionary or keys is null.")]
        [MemberData(nameof(ArgumentNullData))]
        public void ArgumentNull(IDictionary<char, object> dictionary, char[] keys)
        {
            Action act = () => dictionary.MustNotContainKeys(keys);

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
            testData.Add(new CustomExceptionTest(exception => new Dictionary<char, string> { ['a'] = "What?" }.MustNotContainKeys(new[] { 'a' }, exception: exception)))
                    .Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<char, string> { ['a'] = "What?" }.MustNotContainKeys(new[] { 'a' }, message: message)));
        }
    }
}