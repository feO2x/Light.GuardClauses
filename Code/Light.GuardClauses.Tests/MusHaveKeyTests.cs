using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MusHaveKeyTests
    {
        [Theory(DisplayName = "MustHaveKey must throw an exception when the specified key is not present in the dictionary.")]
        [MemberData(nameof(KeyNotPresentData))]
        public void KeyNotPresent(IDictionary<string, object> dictionary, string key)
        {
            Action act = () => dictionary.MustHaveKey(key, nameof(dictionary));

            act.ShouldThrow<KeyNotFoundException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must have key \"{key}\".");
        }

        public static readonly TestData KeyNotPresentData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["1"] = 1, ["2"] = 2 }, "3" },
                new object[] { new Dictionary<string, object> { ["a"] = 'a', ["b"] = 'b' }, "foo" }
            };

        [Theory(DisplayName = "MustHaveKey must not throw an exception when the specified key is present in the dictionary.")]
        [MemberData(nameof(KeyPresentData))]
        public void KeyPresent<TKey>(IDictionary<TKey, object> dictionary, TKey key)
        {
            Action act = () => dictionary.MustHaveKey(key);

            act.ShouldNotThrow();
        }

        public static readonly TestData KeyPresentData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["1"] = 1, ["2"] = 2 }, "1" },
                new object[] { new Dictionary<int, object> { [1] = 'a', [2] = 'b' }, 2 }
            };

        [Fact(DisplayName = "The caller can specify a custom message that MustHaveKey must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall have the key!";

            Action act = () => new Dictionary<string, string> { ["1"] = "Hey" }.MustHaveKey("2", message: message);

            act.ShouldThrow<KeyNotFoundException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustHaveKey must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => new Dictionary<string, string> { ["1"] = "Hey" }.MustHaveKey("2", exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}