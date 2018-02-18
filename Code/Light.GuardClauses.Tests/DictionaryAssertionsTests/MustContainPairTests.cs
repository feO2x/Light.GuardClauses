using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DictionaryAssertionsTests
{
    public sealed class MustContainPairTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustContainPair must throw a Dictionary exception when the dictionary does not contain the specified key-value-pair.")]
        public void PairNotContained()
        {
            const string key = "Fourth";
            const string value = "What's up?";
            var dictionary = new Dictionary<string, string> { ["First"] = "Hello", ["Second"] = null, ["Third"] = "World" };

            Action act = () => dictionary.MustContainPair(key, value, nameof(dictionary));

            act.Should().Throw<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must contain the key-value-pair \"{new StringBuilder().AppendKeyValuePair(key, value)}\", but it does not.");
        }

        [Fact(DisplayName = "MustContainPair must not throw an exception when the dictionary contains the specified key-value-pair.")]
        public void PairContained()
        {
            var dictionary = new Dictionary<int, string> { [1] = "What do we say to the Lord of Death?", [2] = "Not today." };
            const int key = 2;
            const string value = "Not today.";

            var result = dictionary.MustContainPair(key, value);

            result.Should().BeSameAs(dictionary);
        }

        [Fact(DisplayName = "MustContainPair must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => ((IDictionary<int, int>) null).MustContainPair(1, 2);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo" }.MustContainPair(1, "Bar", exception: exception)))
                    .Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<int, string> { [1] = "Foo" }.MustContainPair(1, "Bar", message: message)));
        }
    }
}