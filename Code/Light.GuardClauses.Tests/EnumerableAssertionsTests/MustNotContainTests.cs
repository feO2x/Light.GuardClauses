using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotContainTests : ICustomMessageAndExceptionTestDataProvider
    {
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

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotContain(2, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustNotContain(2, message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, message: message)));
        }
    }
}