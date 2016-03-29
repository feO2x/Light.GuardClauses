using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustContainPairTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainPair must throw a Dictionary exception when the dictionary does not contain the specified key-value-pair.")]
        [MemberData(nameof(PairNotContainedData))]
        public void PairNotContained<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Action act = () => dictionary.MustContainPair(key, value, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must contain the key-value-pair \"{new StringBuilder().AppendKeyValuePair(key, value)}\", but it does not.");
        }

        public static readonly TestData PairNotContainedData =
            new[]
            {
                new object[] { new Dictionary<string, int> { ["Foo"] = 1, ["Bar"] = 2 }, "Baz", 3 },
                new object[] { new Dictionary<string, string> { ["First"] = "Hello", ["Second"] = null, ["Third"] = "World" }, "Fourth", "What's up?" }
            };

        [Theory(DisplayName = "MustContainPair must not throw an exception when the dictionary contains the specified key-value-pair.")]
        [MemberData(nameof(PairContainedData))]
        public void PairContained<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Action act = () => dictionary.MustContainPair(key, value);

            act.ShouldNotThrow();
        }

        public static readonly TestData PairContainedData =
            new[]
            {
                new object[] { new Dictionary<char, int> { ['a'] = 1, ['b'] = 2 }, 'b', 2 },
                new object[] { new Dictionary<int, string> { [1] = "What do we say to the Lord of Death?", [1] = "Not today." }, 1, "Not today." }
            };

        [Fact(DisplayName = "MustContainPair must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => ((IDictionary<int, int>) null).MustContainPair(1, 2);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo" }.MustContainPair(1, "Bar", exception: exception)));

            testData.Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<int, string> { [1] = "Foo" }.MustContainPair(1, "Bar", message: message)));
        }
    }
}