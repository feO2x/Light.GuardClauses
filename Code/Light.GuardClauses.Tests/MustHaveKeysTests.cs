using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveKeysTests
    {
        [Theory(DisplayName = "MustHaveKeys must throw a KeyNotFoundException when at least one of the specified keys is not present in the dictionary.")]
        [MemberData(nameof(DoesNotHaveKeysData))]
        public void DoesNotHaveKeys(IDictionary<string, object> dictionary, IEnumerable<string> keys)
        {
            Action act = () => dictionary.MustHaveKeys(keys, nameof(dictionary));

            act.ShouldThrow<KeyNotFoundException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must contain all of the following keys:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(keys)}{Environment.NewLine}but does not.");
        }

        public static readonly TestData DoesNotHaveKeysData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["a"] = new object(), ["b"] = ConsoleColor.Black }, new[] { "a", "c" } },
                new object[] { new Dictionary<string, object> { ["First Key"] = "Value 1", ["SecondKey"] = "Value 2", ["Third Key"] = null }, new[] { "ThirdKey", "Another Key" } }
            };

        [Theory(DisplayName = "MustHaveKeys must not throw an exception when all the keys are present in the dictionary.")]
        [MemberData(nameof(HasKeysData))]
        public void HasKeys(IDictionary<string, object> dictionary, string[] keys)
        {
            Action act = () => dictionary.MustHaveKeys(keys);

            act.ShouldNotThrow();
        }

        public static readonly TestData HasKeysData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["1"] = "a", ["2"] = "b", ["3"] = "c" }, new[] { "1", "3" } },
                new object[] { new Dictionary<string, object> { ["Id"] = "BudgetInputForm", ["Layout"] = "Horizontal", ["NumberOfColumns"] = 3 }, new[] { "Id", "Layout", "NumberOfColumns" } }
            };

        [Theory(DisplayName = "MustHaveKeys must throw an ArgumentNullException when parameter or keys is null.")]
        [MemberData(nameof(ArgumentNullData))]
        public void ArgumentNull(IDictionary<char, object> dictionary, char[] keys)
        {
            Action act = () => dictionary.MustHaveKeys(keys);

            act.ShouldThrow<ArgumentNullException>();
        }

        public static readonly TestData ArgumentNullData =
            new[]
            {
                new object[] { null, new char[2] },
                new object[] { new Dictionary<char, object>(), null }
            };

        [Fact(DisplayName = "The caller can specify a custom message that MustHaveKeys must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall have the keys!";

            Action act = () => new Dictionary<char, object>().MustHaveKeys(new [] {'a', 'b'}, message: message);

            act.ShouldThrow<KeyNotFoundException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustHaveKeys must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => new Dictionary<char, object>().MustHaveKeys(new[] { 'a', 'b' }, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}