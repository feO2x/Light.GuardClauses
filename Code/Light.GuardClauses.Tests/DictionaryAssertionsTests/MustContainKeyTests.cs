using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.DictionaryAssertionsTests
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

        [Fact(DisplayName = "MustContainKey must not throw an exception when the specified key is present in the dictionary.")]
        public void KeyPresent()
        {
            var dictionary = new Dictionary<int, object> { [1] = 'a', [2] = 'b' };

            var result = dictionary.MustContainKey(2);

            result.Should().BeSameAs(dictionary);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<string, string> { ["1"] = "Hey" }.MustContainKey("2", exception: exception)))
                    .Add(new CustomMessageTest<KeyNotFoundException>(message => new Dictionary<string, string> { ["1"] = "Hey" }.MustContainKey("2", message: message)));
        }
    }
}