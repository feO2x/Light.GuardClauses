using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DictionaryAssertionsTests
{
    public sealed class MustNotContainKeyTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotContainKey must throw a DictioanaryException when the specified key is part of the dictionary.")]
        public void HasKey()
        {
            const char key = 'a';
            var dictionary = new Dictionary<char, object> { ['a'] = 1, ['b'] = 2 };

            Action act = () => dictionary.MustNotContainKey(key, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must not contain key \"{key}\".");
        }

        [Fact(DisplayName = "MustNotContainKey must not throw an exception when the specified key is not part of the dictionary.")]
        public void DoesNotHaveKey()
        {
            const string key = "foo";
            var dictionary = new Dictionary<string, object> { ["Id 1"] = "value 1", ["Id 2"] = 2, ["Id 3"] = ConsoleColor.DarkBlue };

            var result = dictionary.MustNotContainKey(key);

            result.Should().BeSameAs(dictionary);
        }

        [Fact(DisplayName = "MustNotContainKey must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Dictionary<string, object> dictionary = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => dictionary.MustNotContainKey("foo", nameof(dictionary));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(dictionary));
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<char, string> { ['a'] = "value" }.MustNotContainKey('a', exception: exception)))
                    .Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<char, string> { ['a'] = "value" }.MustNotContainKey('a', message: message)));
        }
    }
}