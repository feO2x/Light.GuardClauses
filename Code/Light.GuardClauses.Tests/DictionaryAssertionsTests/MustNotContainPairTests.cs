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
    public sealed class MustNotContainPairTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotContainPair must throw a DictioanryException when the specified pair is part of the dictionary.")]
        public void PairContained()
        {
            const int key = 1;
            const string value = "Foo";
            var dictionary = new Dictionary<int, string> { [1] = "Foo", [2] = "Bar" };

            Action act = () => dictionary.MustNotContainPair(key, value, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must not contain the key-value-pair \"{new StringBuilder().AppendKeyValuePair(key, value)}\", but it does.");
        }

        [Fact(DisplayName = "MustNotContainPair must not throw an exception when the specified pair is not part of the dictionary.")]
        public void PairNotContained()
        {
            const int key = -20;
            const char value = 'x';
            var dictionary = new Dictionary<int, char> { [1] = 'A', [2] = 'B' };

            var result = dictionary.MustNotContainPair(key, value);

            result.Should().BeSameAs(dictionary);
        }

        [Fact(DisplayName = "MustNotContainPair must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => ((IDictionary<string, string>) null).MustNotContainPair("Foo", "Bar");

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo" }.MustNotContainPair(1, "Foo", exception: exception)))
                    .Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<int, string> { [2] = "Bar" }.MustNotContainPair(2, "Bar", message: message)));
        }
    }
}