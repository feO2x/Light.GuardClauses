using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DictionaryAssertionsTests
{
    public sealed class MustNotBeKeyOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeKeyOf must throw an ArgumentOutOfRangeException when the specified key is present in the dictionary.")]
        public void IsKeyOfDictionary()
        {
            const string key = "a";
            var dictionary = new Dictionary<string, object> { ["a"] = 42, ["b"] = 81 };

            Action act = () => key.MustNotBeKeyOf(dictionary, nameof(key));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(key)} must not be one of the dictionary keys:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(dictionary.Keys)}{Environment.NewLine}but you specified {key}");
        }

        [Fact(DisplayName = "MustNotBeKeyOf must not throw an exception when the specified key is not present in the dictionary.")]
        public void IsNotKeyOfDictionary()
        {
            const char key = 'a';
            var dictionary = new Dictionary<char, object> { ['b'] = "Option B", ['c'] = "Option C" };

            var result = key.MustNotBeKeyOf(dictionary);

            result.Should().Be(key);
        }

        [Fact(DisplayName = "MustNotBeKeyOf must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => "someKey".MustNotBeKeyOf<string, object>(null);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("dictionary");
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 'a'.MustNotBeKeyOf(new Dictionary<char, object> { ['a'] = null }, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 'a'.MustNotBeKeyOf(new Dictionary<char, object> { ['a'] = null }, message: message)));
        }
    }
}