using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustContainKeyTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainKey must throw an exception when the specified key is not present in the dictionary.")]
        [MemberData(nameof(KeyNotPresentData))]
        public void KeyNotPresent(IDictionary<string, object> dictionary, string key)
        {
            Action act = () => dictionary.MustContainKey(key, nameof(dictionary));

            act.ShouldThrow<KeyNotFoundException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must contain key \"{key}\".");
        }

        public static readonly TestData KeyNotPresentData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["1"] = 1, ["2"] = 2 }, "3" },
                new object[] { new Dictionary<string, object> { ["a"] = 'a', ["b"] = 'b' }, "foo" }
            };

        [Theory(DisplayName = "MustContainKey must not throw an exception when the specified key is present in the dictionary.")]
        [MemberData(nameof(KeyPresentData))]
        public void KeyPresent<TKey>(IDictionary<TKey, object> dictionary, TKey key)
        {
            Action act = () => dictionary.MustContainKey(key);

            act.ShouldNotThrow();
        }

        public static readonly TestData KeyPresentData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["1"] = 1, ["2"] = 2 }, "1" },
                new object[] { new Dictionary<int, object> { [1] = 'a', [2] = 'b' }, 2 }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<string, string> { ["1"] = "Hey" }.MustContainKey("2", exception: exception)));

            testData.Add(new CustomMessageTest<KeyNotFoundException>(message => new Dictionary<string, string> { ["1"] = "Hey" }.MustContainKey("2", message: message)));
        }
    }
}