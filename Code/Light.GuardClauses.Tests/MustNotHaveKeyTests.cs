using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotHaveKeyTests
    {
        [Theory(DisplayName = "MustNotHaveKey must throw a DictioanaryException when the specified key is part of the dictionary.")]
        [MemberData(nameof(HasKeyData))]
        public void HasKey<TKey>(IDictionary<TKey, object> dictionary, TKey key)
        {
            Action act = () => dictionary.MustNotHaveKey(key, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must not have key \"{key}\".");
        }

        public static readonly TestData HasKeyData =
            new[]
            {
                new object[] { new Dictionary<char, object> { ['a'] = 1, ['b'] = 2 }, 'a' },
                new object[] { new Dictionary<int, object> { [42] = "What", [18554] = "Is", [-332] = "Going", [15] = "On" }, -332 }
            };

        [Theory(DisplayName = "MustNotHaveKey must not throw an exception when the specified key is not part of the dictionary.")]
        [MemberData(nameof(DoesNotHaveKeyData))]
        public void DoesNotHaveKey<TKey>(IDictionary<TKey, object> dictionary, TKey key)
        {
            Action act = () => dictionary.MustNotHaveKey(key);

            act.ShouldNotThrow();
        }

        public static readonly TestData DoesNotHaveKeyData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["Id 1"] = "value 1", ["Id 2"] = 2, ["Id 3"] = ConsoleColor.DarkBlue }, "foo" },
                new object[] { new Dictionary<Guid, object> { [Guid.NewGuid()] = "Hey", [Guid.NewGuid()] = "there" }, Guid.Empty }
            };

        [Fact(DisplayName = "MustNotHaveKey must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Dictionary<string, object> dictionary = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => dictionary.MustNotHaveKey("foo", nameof(dictionary));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(dictionary));
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustNotHaveKey must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou must not have the key!";

            Action act = () => new Dictionary<string, object> { ["a"] = 1 }.MustNotHaveKey("a", message: message);

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain(message);
        }

        [Theory(DisplayName = "The caller can specify a custom exception that MustNotHaveKey must raise instead of the default one.")]
        [MemberData(nameof(CustomExceptionData))]
        public void CustomException(Dictionary<char, string> invalidDictionary, char key)
        {
            var exception = new Exception();

            Action act = () => invalidDictionary.MustNotHaveKey(key, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        public static readonly TestData CustomExceptionData =
            new[]
            {
                new object[] { new Dictionary<char, string> { ['a'] = "value" }, 'a' },
                new object[] { null, 'f' }
            };
    }
}