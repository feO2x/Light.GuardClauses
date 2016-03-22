using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
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

        [Theory(DisplayName = "The caller can specify a custom message for the string overload that MustContain must inject instead of the default one.")]
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

        [Theory(DisplayName = "The caller can specify a custom exception for the string overload that MustContain must raise instead of the default one.")]
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
                new object[] { new[] { 1, 2, 3 }, 1 },
                new object[] { new[] { "How", "Are", "You" }, "Are" },
                new object[] { new[] { new DateTime(2016, 3, 21), new DateTime(1987, 2, 12) }, new DateTime(2016, 3, 21) }
            };

        [Fact(DisplayName = "The caller can specify a custom message for the collection overload that MustContain must inject instead of the default one.")]
        public void CollectionCustomMessage()
        {
            const string message = "Thou shall have item!";

            Action act = () => new[] { 1, 2, 3 }.MustContain(42, message: message);

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception for the collection overload that MustContain must raise instead of the default one.")]
        public void CollectionCustomException()
        {
            var exception = new Exception();

            Action act = () => new[] { 1, 2, 3 }.MustContain(42, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        [Theory(DisplayName = "MustContain must throw a CollectionException when the specified subset is not part of the collection.")]
        [MemberData(nameof(IsNoSupersetData))]
        public void IsNotSuperset<T>(T[] collection, T[] invalidSubset)
        {
            Action act = () => collection.MustContain(invalidSubset, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain the following values{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(invalidSubset)}");
        }

        public static readonly TestData IsNoSupersetData =
            new[]
            {
                new object[] { new object[] { 1, 2, 3, 4, 5 }, new object[] { 3, 4, 5, 6 } },
                new object[] { new object[] { 'x', 'z', 'y', 'b' }, new object[] { 'z', 'y', 'f' } }
            };

        [Theory(DisplayName = "MustContain must not throw an exception when every item in the specified subset is contained in the collection.")]
        [MemberData(nameof(IsSupersetData))]
        public void IsSuperset<T>(T[] collection, T[] subset)
        {
            Action act = () => collection.MustContain(subset);

            act.ShouldNotThrow();
        }

        public static readonly TestData IsSupersetData =
            new[]
            {
                new object[] { new object[] { 1, 2, 3, 4 }, new object[] { 2, 3 } },
                new object[] { new[] { "a", "X", "z" }, new[] { "a", "a", "a" } },
                new object[] { new[] { "a", null }, new string[] { null } },
                new object[] { new[] { "1", "2", "3" }, new[] { "3", "1", "2", "3" } }
            };

        [Theory(DisplayName = "MustContain must throw an ArgumentNullException when either the collection or the subset are null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void SupersetOrCollectionNull(object[] collection, object[] subset)
        {
            Action act = () => collection.MustContain(subset);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "The caller can specify a custom message for the subset overload that MustContain must inject instead of the default one.")]
        public void SubsetCustomMessage()
        {
            const string message = "Thou shall have subset!";

            Action act = () => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, message: message);

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception for the subset overload that MustContain must raise instead of the default one.")]
        public void SubsetCustomException()
        {
            var exception = new Exception();

            Action act = () => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}