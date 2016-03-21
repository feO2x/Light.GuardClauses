using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustContainTests
    {
        [Theory(DisplayName = "MustContain must throw a StringException when the specified text is not part of the given string.")]
        [InlineData("abc", "d")]
        [InlineData("Hello, World!", "You")]
        [InlineData("1, 2, 3", ". ")]
        public void StringDoesNotContainText(string value, string containedText)
        {
            Action act = () => value.MustContain(containedText, nameof(value));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(value)} must contain the text \"{containedText}\", but you specified \"{value}\".");
        }

        [Theory(DisplayName = "MustContain must not throw an exception when the text is contained.")]
        [InlineData("abc", "a")]
        [InlineData("Hello, World!", "orl")]
        [InlineData("1, 2, 3", ", ")]
        public void StringContainsText(string value, string containedText)
        {
            Action act = () => value.MustContain(containedText, nameof(value));

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "The caller can specify a custom message that MustContain must inject instead of the default one.")]
        [InlineData(null, "")]
        [InlineData("", "a")]
        [InlineData("42", "b")]
        public void StringCustomMessage(string invalidString, string containedText)
        {
            const string message = "Thou shall have the contained text!";

            Action act = () => invalidString.MustContain(containedText, message: message);

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain(message);
        }

        [Theory(DisplayName = "The caller can specify a custom exception that MustContain must raise instead of the default one.")]
        [InlineData(null, "")]
        [InlineData("Hello there!", "world")]
        public void StringCustomException(string invalidString, string containedText)
        {
            var exception = new Exception();

            Action act = () => invalidString.MustContain(containedText, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        [Fact(DisplayName = "MustContain must throw an exception when the specified text is null.")]
        public void ContainedTextNull()
        {
            Action act = () => "someText".MustContain(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Contain("You called MustContain wrongly by specifying null for text.");
        }

        [Fact(DisplayName = "MustContain must throw an exception when the specified text is an empty string.")]
        public void ContainedTextEmpty()
        {
            Action act = () => "someText".MustContain(string.Empty);

            act.ShouldThrow<EmptyStringException>()
               .And.Message.Should().Contain("You called MustContain wrongly by specifying an empty string for text.");
        }

        [Theory(DisplayName = "MustContain must throw a CollectionException when the specified value is not part of the collection.")]
        [MemberData(nameof(CollectionDoesNotContainValueData))]
        public void CollectionDoesNotContainValue<T>(IReadOnlyCollection<T> collection, T value)
        {
            Action act = () => collection.MustContain(value, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain value \"{(value != null ? value.ToString() : "null")}\", but does not.");
        }

        public static readonly TestData CollectionDoesNotContainValueData =
            new[]
            {
                new object[] { new[] { 1, 2, 3 }, 42 },
                new object[] { new[] { "Hey", "There" }, "World" },
                new object[] { new[] { true }, false },
                new object[] { new[] { "Here", "I", "Am" }, null },
                new[] { new object[0], new object() }
            };

        [Fact(DisplayName = "MustContain must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            IReadOnlyCollection<string> collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustContain("foo");

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustContain must not throw an exception when the specified value is part of the collection.")]
        [MemberData(nameof(CollectionContainsValueData))]
        public void CollectionContainsValue<T>(IReadOnlyCollection<T> collection, T value)
        {
            Action act = () => collection.MustContain(value);

            act.ShouldNotThrow();
        }

        public static readonly TestData CollectionContainsValueData =
            new[]
            {
                new object[] { new [] {1, 2, 3}, 1 },
                new object[] { new [] {"How", "Are", "You"}, "Are" },
                new object[] { new [] {new DateTime(2016, 3, 21), new DateTime(1987, 2, 12) }, new DateTime(2016, 3, 21) }
            };

        [Fact(DisplayName = "The caller can specify a custom message that MustContain must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall have item!";

            Action act = () => new[] { 1, 2, 3 }.MustContain(42, message: message);

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustHaveKey must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => new[] { 1, 2, 3 }.MustContain(42, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}