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
    public sealed class MustNotContainPairTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainPair must throw a DictioanryException when the specified pair is part of the dictionary.")]
        [MemberData(nameof(PairContainedData))]
        public void PairContained<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Action act = () => dictionary.MustNotContainPair(key, value, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must not contain the key-value-pair \"{new StringBuilder().AppendKeyValuePair(key, value)}\", but it does.");
        }

        public static readonly TestData PairContainedData =
            new[]
            {
                new object[] { new Dictionary<int, string> { [1] = "Foo", [2] = "Bar" }, 1, "Foo" },
                new object[] { new Dictionary<char, int> { ['a'] = 42, ['b'] = 89, ['c'] = 45 }, 'c', 45 }
            };

        [Theory(DisplayName = "MustNotContainPair must not throw an exception when the specified pair is not part of the dictionary.")]
        [MemberData(nameof(PairNotContainedData))]
        public void PairNotContained<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Action act = () => dictionary.MustNotContainPair(key, value);

            act.ShouldNotThrow();
        }

        public static readonly TestData PairNotContainedData =
            new[]
            {
                new object[] { new Dictionary<int, string> { [1] = "Foo", [2] = "Bar", [3] = "Baz" }, 42, "Qux" },
                new object[] { new Dictionary<int, char> { [1] = 'A', [2] = 'B' }, -20, 'x' }
            };

        [Fact(DisplayName = "MustNotContainPair must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => ((IDictionary<string, string>) null).MustNotContainPair("Foo", "Bar");

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo" }.MustNotContainPair(1, "Foo", exception: exception)));

            testData.Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<int, string> { [2] = "Bar" }.MustNotContainPair(2, "Bar", message: message)));
        }
    }
}