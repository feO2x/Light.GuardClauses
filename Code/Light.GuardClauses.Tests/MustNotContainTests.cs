using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotContainTests
    {
        [Theory(DisplayName = "MustNotContain must throw an exception when the specified string contains the given text.")]
        [InlineData("abc", "b")]
        [InlineData("Say herro to my littre friend", "herro")]
        public void TextContained(string value, string containedText)
        {
            Action act = () => value.MustNotContain(containedText, nameof(value));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(value)} must not contain the text \"{containedText}\", but you specified \"{value}\".");
        }

        [Theory(DisplayName = "MustNotContain must not throw an exception when the specified string does not contain the given text.")]
        [InlineData("1, 2, 3", ".")]
        [InlineData("Say herro to my littre friend", "hello")]
        public void TextNotContained(string value, string containedText)
        {
            Action act = () => value.MustNotContain(containedText, nameof(value));

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "The caller can specify a custom message for the string overload that MustNotContain must inject instead of the default one.")]
        [MemberData(nameof(CustomTestData))]
        public void StringCustomMessage(string invalidString, string containedText)
        {
            const string message = "Thou shall not contain the other!";

            Action act = () => invalidString.MustNotContain(containedText, message: message);

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Be(message);
        }

        [Theory(DisplayName = "The caller can specify a custom exception for the string overload that MustNotContain must raise instead of the default one.")]
        [MemberData(nameof(CustomTestData))]
        public void StringCustomException(string invalidString, string containedText)
        {
            var exception = new Exception();

            Action act = () => invalidString.MustNotContain(containedText, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        public static readonly TestData CustomTestData =
            new[]
            {
                new object[] { "I am here", "am" },
                new object[] { null, "foo" },
                new object[] { "When you play the game of thrones you win, or you die. There is no middle ground.", "game of thrones" }
            };

        [Theory(DisplayName = "MustNotContain must throw an exception when the two strings have the same content with different capital letters.")]
        [InlineData("I AM YOUR MASTER", "am your")]
        [InlineData("Where is the LIGHT?", "light")]
        [InlineData("PWND", "pwnd")]
        public void CompareCaseInsensitive(string @string, string comparedText)
        {
            Action act = () => @string.MustNotContain(comparedText, ignoreCaseSensitivity: true);

            act.ShouldThrow<StringException>();
        }

        [Fact(DisplayName = "MustNotContain must throw an exception when the specified text is null.")]
        public void ContainedTextNull()
        {
            Action act = () => "someText".MustNotContain(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Contain("You called MustNotContain wrongly by specifying null for text.");
        }

        [Fact(DisplayName = "MustNotContain must throw an exception when the specified text is an empty string.")]
        public void ContainedTextEmpty()
        {
            Action act = () => "someText".MustNotContain(string.Empty);

            act.ShouldThrow<EmptyStringException>()
               .And.Message.Should().Contain("You called MustNotContain wrongly by specifying an empty string for text.");
        }

        [Theory(DisplayName = "MustNotContain must throw a CollectionException when the specified item is part of the collection.")]
        [MemberData(nameof(CollectionContainsItemData))]
        public void CollectionContainsItem<T>(T[] collection, T item)
        {
            Action act = () => collection.MustNotContain(item, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not contain value \"{item.ToStringOrNull()}\", but it does.");
        }

        public static readonly TestData CollectionContainsItemData =
            new[]
            {
                new object[] { new[] { 1, 2, 3 }, 2 },
                new object[] { new[] { "Hello", "World" }, "Hello" },
                new object[] { new[] { "There is something", null }, null }
            };

        [Theory(DisplayName = "MustNotContain must not throw an exception when the specified item is not part of the collection.")]
        [MemberData(nameof(CollectionDoesNotContainItemData))]
        public void CollectionDoesNotContainItem<T>(T[] collection, T item)
        {
            Action act = () => collection.MustNotContain(item);

            act.ShouldNotThrow();
        }

        public static readonly TestData CollectionDoesNotContainItemData =
            new[]
            {
                new object[] { new[] { 'a', 'b', 'c' }, 'd' },
                new object[] { new[] { 42, 87, -188 }, 10555 },
                new object[] { new[] { true }, false }
            };

        [Fact(DisplayName = "MustNotContain must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            object[] collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustNotContain("foo");

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "The caller can specify a custom message for the collection overload that MustNotContain must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall have not!";

            Action act = () => new[] { 1, 2, 3 }.MustNotContain(2, message: message);

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception for the collection overload that MustNotContain must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => new[] { 1, 2, 3 }.MustNotContain(2, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        [Theory(DisplayName = "MustNotContain must throw a CollectionException when the collection contains any of the items of the specified set.")]
        [MemberData(nameof(SetContainedData))]
        public void SetContained<T>(T[] collection, T[] set)
        {
            Action act = () => collection.MustNotContain(set, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not contain any of the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(set)}");
        }

        public static readonly TestData SetContainedData =
            new[]
            {
                new object[] { new[] { "Hello", "World" }, new[] { "Hello", "There" } },
                new object[] { new object[] { 1, 2, 3, 4, 5 }, new object[] { 2, 5 } },
                new object[] { new[] { new object(), null }, new object[] { null } }
            };

        [Theory(DisplayName = "MustNotContain must not throw an exception when the collection does not contain any items of the specified set.")]
        [MemberData(nameof(SetNotContainedData))]
        public void SetNotContained<T>(T[] collection, T[] set)
        {
            Action act = () => collection.MustNotContain(set);

            act.ShouldNotThrow();
        }

        public static readonly TestData SetNotContainedData =
            new[]
            {
                new object[] { new[] { "Hello", "There" }, new[] { "What's", "Up?" } },
                new object[] { new object[] { 1, 2, 3, 4 }, new object[] { 81, -34445, 20 } }
            };

        [Theory(DisplayName = "MustNotContain must throw an ArgumentNullException when the specified parameter or set is null.")]
        [InlineData(new object[0], null)]
        [InlineData(null, new object[0])]
        public void SetArgumentsNull(object[] collection, object[] set)
        {
            Action act = () => collection.MustNotContain(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "The caller can specify a custom message for the set overload that MustNotContain must inject instead of the default one.")]
        public void SetCustomMessage()
        {
            const string message = "Thou shall not have those items!";

            Action act = () => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, message: message);

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception for the set overload that MustNotContain must raise instead of the default one.")]
        public void SetCustomException()
        {
            var exception = new Exception();

            Action act = () => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}