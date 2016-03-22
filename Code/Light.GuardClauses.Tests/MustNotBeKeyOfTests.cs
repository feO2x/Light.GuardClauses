using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeKeyOfTests
    {
        [Theory(DisplayName = "MustNotBeKeyOf must throw an ArgumentOutOfRangeException when the specified key is present in the dictionary.")]
        [MemberData(nameof(IsKeyData))]
        public void IsKey<T>(T key, IDictionary<T, object> dictionary)
        {
            Action act = () => key.MustNotBeKeyOf(dictionary, nameof(key));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(key)} must not be one of the dictionary keys{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(dictionary.Keys)}{Environment.NewLine}but you specified {key}");
        }

        public static readonly TestData IsKeyData =
            new[]
            {
                new object[] { "a", new Dictionary<string, object> { ["a"] = 42, ["b"] = 81 } },
                new object[] { 2, new Dictionary<int, object> { [1] = "Hello", [2] = "World", [3] = "How are you?" } },
                new object[] { Guid.Empty, new Dictionary<Guid, object> { [Guid.Empty] = "Empty Entry", [Guid.NewGuid()] = "Other Entry" } }
            };

        [Theory(DisplayName = "MustNotBeKeyOf must not throw an exception when the specified key is not present in the dictionary.")]
        [MemberData(nameof(IsNotKeyData))]
        public void IsNotKey<T>(T key, IDictionary<T, object> dictionary)
        {
            Action act = () => key.MustNotBeKeyOf(dictionary);

            act.ShouldNotThrow();
        }

        public static readonly TestData IsNotKeyData =
            new[]
            {
                new object[] { 'a', new Dictionary<char, object> { ['b'] = "Option B", ['c'] = "Option C" } },
                new object[] { 42L, new Dictionary<long, object> { [1L] = "One", [2L] = "Two", [3L] = "Lame" } }
            };

        [Fact(DisplayName = "MustNotBeKeyOf must throw an ArgumentNullException when the specified dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => "someKey".MustNotBeKeyOf<string, object>(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("dictionary");
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustNotBeKeyOf must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall be no key!";

            Action act = () => 'a'.MustNotBeKeyOf(new Dictionary<char, object> { ['a'] = null }, message: message);

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotBeKeyOf must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => 'a'.MustNotBeKeyOf(new Dictionary<char, object> { ['a'] = null }, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}